using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using CsvHelper;
using CsvHelper.Configuration;
using ExcelParser.Csv.Extensions;
using ExcelParser.Csv.Models;

namespace ExcelParser.Csv
{
    public interface ICsvUtility
    {
        ParsedEnumerableResponse<T> ParseCsvFile<T>(Stream fileStream, CustomClassMap<T> classMap,
            Expression<Func<T>> initialValueFunc = null, Expression<Func<T, ICollection<KeyValuePair<string, string>>>> typedValidator = null);

        string ToCsvData<T>(IEnumerable<ParsedItemResponse<T>> itemsWithErrors, ClassMap<T> classMap);

        string ToCsvData<T>(IEnumerable<T> data, ClassMap<T> classMap);
    }

    public class CsvUtility : ICsvUtility
    {
        public ParsedEnumerableResponse<T> ParseCsvFile<T>(Stream fileStream, CustomClassMap<T> classMap,
            Expression<Func<T>> initialValueFunc = null, Expression<Func<T, ICollection<KeyValuePair<string, string>>>> typedValidator = null)
        {
            var response = new ParsedEnumerableResponse <T>();

            using (fileStream)
            using (var reader = new StreamReader(fileStream))
            using (var parser = InitCsvReader(reader, classMap))
            {
                response.FoundHeaders = parser.Context.Reader.HeaderRecord;
                while (parser.Read())
                {
                    response.Items.Add(parser.SafeParseRecord(classMap, initialValueFunc, typedValidator));
                }
            }

            return response;
        }

        public string ToCsvData<T>(IEnumerable<ParsedItemResponse<T>> itemsWithErrors, ClassMap<T> classMap)
        {
            var csvString = string.Join(",", classMap.GetHeaders().Concat(new[] { "Errors" }));

            foreach (var errorRow in itemsWithErrors)
            {
                csvString += Environment.NewLine;
                var errorRowWithError = errorRow.RawData.Concat(new[] { string.Join(" | ", errorRow.Errors) }).ToList();
                csvString += $"{string.Join(",", errorRowWithError)}";
            }

            return csvString;
        }

        public string ToCsvData<T>(IEnumerable<T> data, ClassMap<T> classMap)
        {
            using (var writer = new StringWriter())
            using (var csvWriter = InitCsvWriter(writer, classMap))
            {
                csvWriter.WriteRecords(data);
                return writer.ToString();
            }
        }

        private CsvReader InitCsvReader(TextReader reader, ClassMap classMap)
        {
            var csv = new CsvReader(reader, GetConfig());
            csv.Context.RegisterClassMap(classMap);
            csv.Read();
            csv.ReadHeader();
            csv.ActuallyValidateHeader(classMap);;
            return csv;
        }

        private CsvWriter InitCsvWriter(TextWriter writer, ClassMap classMap)
        {
            var csv = new CsvWriter(writer, GetConfig());
            csv.Context.RegisterClassMap(classMap);
            return csv;
        }

        private CsvConfiguration GetConfig()
        {
            return new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                IgnoreBlankLines = true,
                TrimOptions = TrimOptions.Trim
            };
        }
    }
}