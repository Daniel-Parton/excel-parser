using System;
using System.Collections.Generic;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using ExcelParser.Extensions;
using ExcelParser.Helpers;
using Newtonsoft.Json;

namespace ExcelParser.Csv.CustomConverters
{
    public class CsvEnumCollectionConverter<T> : ITypeConverter where T : struct, IConvertible
    {
        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            if (value == null) return null;
            if (value is IEnumerable<T> enumerable)
            {
                return string.Join("|", enumerable.Select(e => e.ToString()));
            }

            return JsonConvert.SerializeObject(value);
        }

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            var fields = text.Split('|');
            var value = new List<T>();
            foreach (var field in fields.Select(e => e.Trim()))
            {
                if (!field.TryGetEnum<T>(out var parsedValue))
                {
                    var allEnums = EnumHelper.ToList<T>();
                    throw new TypeConverterException(this, memberMapData, text, row.Context, $"{field} Must be one of these values: {string.Join(" | ", allEnums)}");
                }
                value.Add(parsedValue);
            }

            return value;
        }
    }
}