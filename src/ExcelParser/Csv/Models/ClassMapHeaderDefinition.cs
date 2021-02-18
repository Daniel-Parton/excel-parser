using System.Collections.Generic;

namespace ExcelParser.Csv.Models
{
    public class ClassMapHeaderDefinition
    {
        public string TypedPropertyName { get; set; }
        public string Comment { get; set; }
        public IEnumerable<string> SingleEnumValue { get; set; }
        public IEnumerable<string> MultiEnumValue { get; set; }
        public bool Required { get; set; }
    }
}
