using System.Collections.Generic;
using System.Linq;
using CsvHelper;
using CsvHelper.TypeConversion;
using ExcelParser.Extensions;

namespace ExcelParser.Csv.Extensions
{
    public static class CsvHelperExceptionExtensions
    {
        public static string GetFriendlyErrorMessage(this CsvHelperException exception)
        {
            if (exception is TypeConverterException tex)
            {
                return $"{tex.MemberMapData.Names[0]}: ${tex.GetDeepestMessage()}";
            }

            if (exception is FieldValidationException fvex)
            {
                var reader = fvex.Context.Reader;
                var headerNames = new List<string>();
                for (var i = 0; i < reader.Parser.Record.Length; i++)
                {
                    if(reader.Parser[i] == fvex.Field) headerNames.Add(reader.HeaderRecord[i]); 
                }

                if (headerNames.Any())
                {
                    return $"Error converting: {string.Join(",", headerNames)} to typed value";
                }
            }

            return $"{exception.GetDeepestMessage()}";
        }
    }
}