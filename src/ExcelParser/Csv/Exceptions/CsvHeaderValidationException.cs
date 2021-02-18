using System;
using ExcelParser.Csv.Models;

namespace ExcelParser.Csv.Exceptions
{
    public class CsvHeaderValidationException : Exception
    {
        public CsvHeaderValidationException() { }
        public CsvHeaderValidationException(string message) : base(message) { }
    }
}
