using System;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using ExcelParser.Extensions;
using ExcelParser.Helpers;

namespace ExcelParser.Csv.CustomConverters
{
    public class CsvEnumConverter<T> : ITypeConverter where T : struct, IConvertible
    {
        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return value.ToString();
        }

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if(!text.TryGetEnum<T>(out var parsedValue))
            {
                var allEnums = EnumHelper.ToList<T>();
                throw new TypeConverterException(this, memberMapData, text, row.Context, $"Must be one of these values: ${string.Join(" | ", allEnums)}");
            }

            return parsedValue;
        }
    }
}