using System.Collections.Generic;
using ExcelParser.Csv.Models;

namespace ExcelParser.Excel
{
    public class WorkSheetDto
    {
        public WorkSheetDto()
        {
            Headers = new Dictionary<string, ClassMapHeaderDefinition>();
            Data = new List<List<CellDto>>();
        }

        public WorkSheetDto(string name, Dictionary<string, ClassMapHeaderDefinition> headers)
        {
            Name = name;
            Headers = headers;
            Data = new List<List<CellDto>>();
        }

        public WorkSheetDto(string name, Dictionary<string, ClassMapHeaderDefinition> headers, IEnumerable<IEnumerable<CellDto>> data)
        {
            Name = name;
            Headers = headers;
            Data = data;
        }

        public string Name { get; set; }

        public Dictionary<string, ClassMapHeaderDefinition> Headers { get; set; }
        public IEnumerable<IEnumerable<CellDto>> Data { get; set; }
    }
}