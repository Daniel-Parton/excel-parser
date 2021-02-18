using System.Collections.Generic;
using ExcelParser.Extensions;

namespace ExcelParser.Csv.Models
{
    public class RawParsedItemResponse
    {
        public RawParsedItemResponse(int rowIndex)
        {
            RowIndex = rowIndex;
            RawData = new List<string>();
            Errors = new List<ParsedItemError>();
        }

        public RawParsedItemResponse(int rowIndex, IEnumerable<string> data)
        {
            RowIndex = rowIndex;
            RawData = data;
            Errors = new List<ParsedItemError>();
        }

        public RawParsedItemResponse(int rowIndex, IEnumerable<string> data, ICollection<ParsedItemError> errors)
        {
            RowIndex = rowIndex;
            RawData = data;
            Errors = errors;
        }

        public int RowIndex { get; set; }
        public IEnumerable<string> RawData { get; set; }
        public ICollection<ParsedItemError> Errors { get; set; }
        public bool HasErrors => !Errors.IsNullOrEmpty();
    }
}
