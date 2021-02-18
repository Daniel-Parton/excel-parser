using System;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace ExcelParser.Csv.CustomConverters
{
    public class DateTimeConverter : ITypeConverter
    {
        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return value.ToString();
        }

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if(!DateTime.TryParse(text, out var dateValue))
            {
                throw new TypeConverterException(this, memberMapData, text, row.Context, $"Not a valid date");
            }

            return dateValue;
        }
    }
}