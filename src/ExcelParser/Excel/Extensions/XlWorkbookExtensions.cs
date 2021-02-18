using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;
using ExcelParser.Extensions;

namespace ExcelParser.Excel.Extensions
{
    public static class XlWorkbookExtensions
    {
        public static bool HasAllSheets(this XLWorkbook workbook, IEnumerable<string> sheetNames)
        {
            if (sheetNames.IsNullOrEmpty()) return true;
            if (workbook == null || workbook.Worksheets.IsNullOrEmpty()) return false;

            return sheetNames.All(h => workbook.Worksheets.Any(ws => ws.Name == h));
        }

        public static bool HasAllSheets(this XLWorkbook workbook, IEnumerable<string> sheetNames, out ICollection<string> missingSheetNames)
        {
            missingSheetNames = new List<string>();
            if (sheetNames.IsNullOrEmpty()) return true;
            if (workbook == null || workbook.Worksheets.IsNullOrEmpty())
            {
                missingSheetNames = sheetNames.ToList();
                return false;
            }

            bool hasSheets = true;
            foreach (var sheetName in sheetNames)
            {
                //Missing sheet
                if (workbook.Worksheets.All(ws => ws.Name != sheetName))
                {
                    hasSheets = false;
                    missingSheetNames.Add(sheetName);
                }
            }

            return hasSheets;
        }
    }
}