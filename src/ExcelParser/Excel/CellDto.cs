
namespace ExcelParser.Excel
{
    public class CellDto
    {
        public CellDto(string data)
        {
            Data = data;
        }

        public CellDto(string data, string fontColor)
        {
            Data = data;
            FontColor = fontColor;
        }

        public string Data { get; set; }
        public string FontColor { get; set; }
        public string BackgroundColor { get; set; }
    }
}