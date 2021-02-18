using ExcelParser.Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using ExcelParser.Csv.Extensions;
using ExcelParser.Csv.Models;
using ExcelParser.Extensions;
using Xunit;

namespace ExcelParser.Test.Csv
{
    public class CsvUtilityTests : IDisposable
    {
        private readonly ICsvUtility _csvUtility;

        public CsvUtilityTests()
        {
            _csvUtility = new CsvUtility();
        }

        [Fact]
        public void ParseCsvFile_WhenBadValuesGiven_ErrorsExist()
        {
            var classMap = new TestCsvMap();
            var headerRow = classMap.GetHeaders();
            var dataRow = new List<string>();
            foreach (var row in headerRow)
            {
                switch (row)
                {
                    case TestUploadConstants.StringHeader: dataRow.Add("1"); break;
                    case TestUploadConstants.NumberHeader: dataRow.Add("Nopers"); break;
                    case TestUploadConstants.DateHeader: dataRow.Add("123"); break;
                    case TestUploadConstants.EnumHeader: dataRow.Add("123"); break;
                    case TestUploadConstants.EnumCollectionHeader: dataRow.Add("123"); break;
                }
            }

            var result = MapDataAndParse(classMap, headerRow, new[] { dataRow });

            //Expecting every row except for the string to have errors
            var expectedErrors = headerRow.Count() - 1;
            foreach (var item in result.Items)
            {
                Assert.True(item.Errors.Count == headerRow.Count() - 1, $"Must have {expectedErrors} errors but was: {item.Errors.Count}");
            }
        }

        [Fact]
        public void ParseCsvFile_WhenAllGoodValues_NoErrors()
        {
            var classMap = new TestCsvMap();
            var headerRow = classMap.GetHeaders();
            var dataRow = new List<string>();
            foreach (var row in headerRow)
            {
                switch (row)
                {
                    case TestUploadConstants.StringHeader: dataRow.Add("blah blah blah"); break;
                    case TestUploadConstants.NumberHeader: dataRow.Add(123.ToString()); break;
                    case TestUploadConstants.DateHeader: dataRow.Add(DateTime.Now.ToString("O")); break;
                    case TestUploadConstants.EnumHeader: dataRow.Add(TestUploadEnum.Value1.ToString()); break;
                    case TestUploadConstants.EnumCollectionHeader: dataRow.Add(string.Join("|", new[] { TestUploadEnum.Value1, TestUploadEnum.Value2 })); break;
                }
            }

            var result = MapDataAndParse(classMap, headerRow, new[] { dataRow });
            AssertNoErrors(result);
        }

        [Fact]
        public void AllCustomClassMapChildren_MemberMapsCountMustEqualHeaderDefinitions()
        {
            var types = AppDomain.CurrentDomain.GetAllClassesFromType<ICustomClassMap>();
            var errors = new List<string>();
            foreach (var type in types)
            {
                var classMapInstance = Activator.CreateInstance(type);
                var headersCount = ((ICustomClassMap)classMapInstance).GetHeaderDefinitions().Count;
                var memberMapsCount = ((ClassMap)classMapInstance).MemberMaps.Count;
                if (memberMapsCount != headersCount)
                {
                    errors.Add($"ClassMap: {type.Name} has a mismatch in member maps.  Expected: {headersCount}.  Actual: {memberMapsCount}");
                }
            }

            Assert.True(errors.IsNullOrEmpty(), string.Join(Environment.NewLine, errors));
        }

        private ParsedEnumerableResponse<T> MapDataAndParse<T>(CustomClassMap<T> classMap, IEnumerable<string> headerRow, IEnumerable<IEnumerable<string>> rowData)
        {
            var csv = string.Join(",", headerRow);
            foreach (var row in rowData)
            {
                csv += Environment.NewLine;
                csv += string.Join(",", row);

            }
            return _csvUtility.ParseCsvFile(new MemoryStream(Encoding.ASCII.GetBytes(csv)), classMap);
        }

        private void AssertNoErrors<T>(ParsedEnumerableResponse<T> result)
        {
            foreach (var item in result.Items)
            {
                Assert.True(!item.HasErrors, $"Must have no errors.  Errors: {string.Join(", ", item.Errors.Select(e => $"{e.Name} - {e.Error}")) }");
                Assert.True(item.Data != null, "Data must not be null");
            }
        }

        public void Dispose()
        {
        }
    }
}
