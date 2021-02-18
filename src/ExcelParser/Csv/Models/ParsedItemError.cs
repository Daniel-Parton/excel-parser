using System.Collections.Generic;

namespace ExcelParser.Csv.Models
{
    public class ParsedItemError
    {
        public ParsedItemError() { }

        public ParsedItemError(string name, string error)
        {
            Name = name;
            Error = error;
        }
        public string Name { get; set; }
        public string Error { get; set; }
    }
}
