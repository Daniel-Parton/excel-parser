using System.Collections.Generic;
using ExcelParser.Extensions;

namespace ExcelParser.Csv.Models
{
    public class ParsedItemResponse<T> : RawParsedItemResponse
    {
        public ParsedItemResponse(int rowIndex) : base(rowIndex)
        {
            TypedErrors = new List<KeyValuePair<string, string>>();
        }
        public ParsedItemResponse(int rowIndex, IEnumerable<string> data) : base(rowIndex, data)
        {
            TypedErrors = new List<KeyValuePair<string, string>>();
        }
        public ParsedItemResponse(int rowIndex, IEnumerable<string> data, ICollection<ParsedItemError> errors) : base(rowIndex, data, errors)
        {
            TypedErrors = new List<KeyValuePair<string, string>>();
        }

        public T Data { get; set; }
        public ICollection<KeyValuePair<string, string>> TypedErrors { get; set; }
        public new bool HasErrors => base.HasErrors || !TypedErrors.IsNullOrEmpty();
    }
}
