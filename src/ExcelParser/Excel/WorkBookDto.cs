using System.Collections.Generic;

namespace ExcelParser.Excel
{
    public class WorkBookDto
    {
        public WorkBookDto()
        {
            Sheets = new List<WorkSheetDto>();
        }

        public IEnumerable<WorkSheetDto> Sheets { get; set; }
    }
}