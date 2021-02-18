using System;
using System.Collections.Generic;
using ExcelParser.Csv.CustomConverters;
using ExcelParser.Csv.Models;

namespace ExcelParser.Test.Csv
{
    public enum TestUploadEnum
    {
        Value1,
        Value2
    }

    public static class TestUploadConstants
    {
        public const string StringHeader = "String";
        public const string NumberHeader = "Number";
        public const string DateHeader = "Date";
        public const string EnumHeader = "Enum";
        public const string EnumCollectionHeader = "EnumCollection";
    }

    public class TestCsvUpload
    {
        public TestCsvUpload()
        {
            EnumValueCollection = new List<TestUploadEnum>();
        }

        public string StringValue { get; set; }
        public decimal NumberValue { get; set; }
        public DateTime DateValue { get; set; }
        public TestUploadEnum EnumValue { get; set; }
        public ICollection<TestUploadEnum> EnumValueCollection { get; set; }
    }

    public sealed class TestCsvMap : CustomClassMap<TestCsvUpload>
    {
        protected override Dictionary<string, ClassMapHeaderDefinition> HeaderDefinitions { get; set; }

        public TestCsvMap()
        {

            HeaderDefinitions = new Dictionary<string, ClassMapHeaderDefinition>
            {
                {TestUploadConstants.StringHeader, new ClassMapHeaderDefinition() },
                {TestUploadConstants.NumberHeader, new ClassMapHeaderDefinition() },
                {TestUploadConstants.DateHeader, new ClassMapHeaderDefinition() },
                {TestUploadConstants.EnumHeader, new ClassMapHeaderDefinition() },
                {TestUploadConstants.EnumCollectionHeader, new ClassMapHeaderDefinition() },
            };

            Map(m => m.StringValue)
                .Name(TestUploadConstants.StringHeader);

            Map(m => m.NumberValue)
                .Name(TestUploadConstants.NumberHeader);

            Map(m => m.DateValue)
                .Name(TestUploadConstants.DateHeader)
                .TypeConverter<DateTimeConverter>();

            Map(m => m.EnumValue)
                .Name(TestUploadConstants.EnumHeader)
                .TypeConverter<CsvEnumConverter<TestUploadEnum>>();

            Map(m => m.EnumValueCollection)
                .Name(TestUploadConstants.EnumCollectionHeader)
                .TypeConverter<CsvEnumCollectionConverter<TestUploadEnum>>();
        }

    }
}
