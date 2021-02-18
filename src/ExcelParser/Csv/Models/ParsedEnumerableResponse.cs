using System.Collections.Generic;
using System.Linq;

namespace ExcelParser.Csv.Models
{
    public class ParsedEnumerableResponse<T>
    {
        public ICollection<string> FoundHeaders { get; set; } = new List<string>();
        public ICollection<ParsedItemResponse<T>> Items { get; set; } = new List<ParsedItemResponse<T>>();
        public int ErrorCount => Items.Count(e => e.HasErrors);
    }
}
