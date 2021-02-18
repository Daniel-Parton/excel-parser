using System;
using System.Linq;
using ClosedXML.Excel;

namespace ExcelParser.Excel.Extensions
{
    public static class XlWorkSheetExtensions
    {
        public static string UsedCellsToCsv(this IXLWorksheet workSheet)
        {
            var lastCellAddress = workSheet.RangeUsed().LastCell().Address;
            var csvRows = workSheet
                .Rows(1, lastCellAddress.RowNumber)
                .Select(row =>
                {
                    var mappedCells = row
                        .Cells(1, lastCellAddress.ColumnNumber)
                        .Select(cell =>
                        {
                            var cellValue = cell.GetValue<string>();
                            return cellValue.Contains(",") ? $"\"{cellValue}\"" : cellValue;
                        });
                    return string.Join(",", mappedCells);
                });
            return string.Join(Environment.NewLine, csvRows);
        }
    }
}