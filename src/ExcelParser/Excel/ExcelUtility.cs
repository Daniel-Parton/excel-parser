using System;
using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;
using ExcelParser.Extensions;

namespace ExcelParser.Excel
{
    public interface IExcelUtility
    {
        byte[] ToExcelFileBytes(WorkBookDto workBook);
        byte[] ToExcelFileBytes(XLWorkbook workBook);
        XLWorkbook ToExcelWorkkbook(Stream stream);
        XLWorkbook ToExcelWorkkbook(byte[] bytes);
    }

    public class ExcelUtility : IExcelUtility
    {
        public byte[] ToExcelFileBytes(WorkBookDto dto)
        {
            if (dto == null || dto.Sheets.IsNullOrEmpty()) return null;

            using (var workBook = new XLWorkbook())
            {
                var sheetIndex = 1;

                //Add Sheet
                foreach (var sheetDto in dto.Sheets)
                {
                    var sheetName = sheetDto.Name.IsEmpty() ? $"Sheet {sheetIndex}" : sheetDto.Name;
                    var tempWorkSheet = workBook.Worksheets.Add(sheetName);

                    //Add Headers
                    var columnIndex = 1;
                    foreach (var header in sheetDto.Headers)
                    {
                        var commentLines = new List<string>();
                        var tempCell = tempWorkSheet.Cell(1, columnIndex);
                        tempCell.Value = header.Key;

                        if (header.Value.Required)
                        {
                            commentLines.Add("Required");
                        }

                        if (!header.Value.Comment.IsEmpty())
                        {
                            commentLines.Add(header.Value.Comment);
                        }
                        if (!header.Value.SingleEnumValue.IsNullOrEmpty())
                        {
                            var innerLines = new List<string>();
                            innerLines.Add("Choose from one of the following:");
                            foreach (var value in header.Value.SingleEnumValue)
                            {
                                innerLines.Add(value);
                            }
                            commentLines.Add(string.Join(Environment.NewLine, innerLines));
                        }

                        if (!header.Value.MultiEnumValue.IsNullOrEmpty())
                        {
                            var innerLines = new List<string>();
                            innerLines.Add("Choose from one or more of the following: (Seperate with '|')");
                            foreach (var value in header.Value.MultiEnumValue)
                            {
                                innerLines.Add(value);
                            }
                            commentLines.Add(string.Join(Environment.NewLine, innerLines));
                        }


                        if (!commentLines.IsNullOrEmpty())
                        {
                            tempCell.Comment.AddText(string.Join(Environment.NewLine, commentLines));
                            tempCell.Comment.Style.Size.SetAutomaticSize();
                        }
                        columnIndex++;
                    }

                    //Add Data Rows
                    var rowIndex = 2;
                    foreach (var rowData in sheetDto.Data)
                    {
                        //Add Data Cells
                        columnIndex = 1;
                        foreach (var cellData in rowData)
                        {
                            tempWorkSheet.Cell(rowIndex, columnIndex).Value = cellData.Data;
                            if(!cellData.FontColor.IsEmpty()) tempWorkSheet.Cell(rowIndex, columnIndex).Style.Font.FontColor = XLColor.FromHtml(cellData.FontColor);
                            if(!cellData.BackgroundColor.IsEmpty()) tempWorkSheet.Cell(rowIndex, columnIndex).Style.Fill.BackgroundColor = XLColor.FromHtml(cellData.BackgroundColor);
                            columnIndex++;
                        }

                        rowIndex++;
                    }
                    tempWorkSheet.Columns().AdjustToContents();

                    sheetIndex++;
                }

                return ToExcelFileBytes(workBook);
            }
        }

        public byte[] ToExcelFileBytes(XLWorkbook workBook)
        {
            using (var stream = new MemoryStream())
            {
                workBook.SaveAs(stream);
                stream.Position = 0;
                return stream.ToArray();
            }
        }

        public XLWorkbook ToExcelWorkkbook(Stream stream)
        {
            var workBook = new XLWorkbook(stream);
            return workBook;
        }

        public XLWorkbook ToExcelWorkkbook(byte[] bytes)
        {
            return ToExcelWorkkbook(new MemoryStream(bytes));
        }
    }
}