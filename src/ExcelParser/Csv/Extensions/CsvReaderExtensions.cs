using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using ExcelParser.Csv.Exceptions;
using ExcelParser.Csv.Models;
using ExcelParser.Extensions;

namespace ExcelParser.Csv.Extensions
{
    public static class CsvReaderExtensions
    {
        public static void ActuallyValidateHeader(this CsvReader parser, ClassMap map)
        {
            var baseErrorMessage = "Error validating header from csv.  ";

            var csvHeaders = parser.Context.Reader.HeaderRecord;
            if (csvHeaders.IsNullOrEmpty())
            {
                throw new CsvHeaderValidationException($"{baseErrorMessage}Could not find any header records");
            }

            var actualHeaders = map.GetHeaders();
            var missingHeaders = actualHeaders.ToList();

            foreach (var actualHeader in actualHeaders)
            {
                var trimmedActual = actualHeader.Trim().ToLower(CultureInfo.InvariantCulture);
                if (csvHeaders.Any(h => h.Trim().ToLower(CultureInfo.InvariantCulture) == trimmedActual))
                {
                    missingHeaders.Remove(actualHeader);
                }
            }

            if (missingHeaders.Any())
            {
                throw new CsvHeaderValidationException($"{baseErrorMessage}Missing the following header/s: {string.Join(", ", missingHeaders) }");
            }
        }

        public static ParsedItemResponse<T> SafeParseRecord<T>(this CsvReader parser, CustomClassMap<T> classMap,
            Expression<Func<T>> initialValueFunc = null, Expression<Func<T, ICollection<KeyValuePair<string, string>>>> typedValidator = null)
        {
            var parsedItem = new ParsedItemResponse<T>(parser.Context.Reader.Parser.Row - 1, parser.Parser.Record);

            var valueType = typeof(T);

            var value = initialValueFunc == null ? (T)Activator.CreateInstance(valueType) : initialValueFunc.Compile()();

            foreach (var memberMap in classMap.MemberMaps)
            {
                var data = memberMap.Data;
                var valueProperty = valueType.GetProperty(data.Member.Name);
                var vaulePropertyType = data.Member.MemberType();
                var csvHeaderName = data.Names.First();

                try
                {
                    var propertyValue = parser.GetField(vaulePropertyType, csvHeaderName, data.TypeConverter);

                    //If we have an error dont waste memory using reflection as the item is incomplete
                    if (!parsedItem.HasErrors)
                    {
                        valueProperty.SetValue(value, propertyValue);
                    }


                }
                catch (Exception ex)
                {
                    if (!data.IsOptional)
                    {
                        if (ex is TypeConverterException || ex is FormatException)
                        {
                            parsedItem.Errors.Add(new ParsedItemError(csvHeaderName, $"Failed to convert {csvHeaderName} to {valueProperty.Name}"));
                        }
                        else if (ex is CsvHelperException csvHelperException)
                        {
                            parsedItem.Errors.Add(new ParsedItemError(csvHeaderName, csvHelperException.GetFriendlyErrorMessage()));
                        }
                        else
                        {
                            parsedItem.Errors.Add(new ParsedItemError(csvHeaderName, "Unhandled exception parsing record"));
                        }
                    }
                }
            }

            if (!parsedItem.HasErrors)
            {
                parsedItem.Data = value;
                ProcessTypedErrors(classMap, parsedItem, typedValidator);
            }

            return parsedItem;
        }

        private static void ProcessTypedErrors<T>(CustomClassMap<T> classMap, ParsedItemResponse<T> parsedItem,
            Expression<Func<T, ICollection<KeyValuePair<string, string>>>> typedValidator = null)
        {
            if (typedValidator == null || parsedItem.Data == null)
            {
                return;
            }


            //Run typed validator.  Should return a list of key value pairs. Key bing the typed property name, value being the error
            parsedItem.TypedErrors = typedValidator.Compile()(parsedItem.Data);
            if (!parsedItem.TypedErrors.IsNullOrEmpty())
            {
                //Now we want to add the errors to the parsed item with the key being the classmap header definition.
                //The custom class map should have the typed property name so we can map it
                var csvHeaderInfo = classMap.GetHeaderDefinitions();
                foreach (var typedError in parsedItem.TypedErrors.ToList())
                {
                    var headerKey = csvHeaderInfo
                        .Where(e => e.Value.TypedPropertyName == typedError.Key)
                        .Select(e => e.Key)
                        .FirstOrDefault();

                    if (!headerKey.IsEmpty())
                    {
                        parsedItem.Errors.Add(new ParsedItemError(headerKey, typedError.Value));
                    }

                    parsedItem.TypedErrors.Remove(typedError);
                }
            }
        }
    }
}