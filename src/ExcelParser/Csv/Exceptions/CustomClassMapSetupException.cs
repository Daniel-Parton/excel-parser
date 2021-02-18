using System;

namespace ExcelParser.Csv.Exceptions
{
    public class CustomClassMapSetupException : Exception
    {
        public CustomClassMapSetupException() { }
        public CustomClassMapSetupException(string message) : base(message) { }
    }
}
