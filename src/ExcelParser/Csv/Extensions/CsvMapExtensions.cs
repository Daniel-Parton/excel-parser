using System.Collections.Generic;
using System.Linq;
using CsvHelper.Configuration;

namespace ExcelParser.Csv.Extensions
{
    public static class CsvMapExtensions
    {
        public static IEnumerable<string> GetHeadersForCsv(this ClassMap classMap)
        {
            return classMap.MemberMaps.Select(a => a.Data.Names.First());
        }

        public static IEnumerable<string> GetHeaders(this ClassMap classMap)
        {
            return classMap.MemberMaps.Select(a => a.Data.Names.First());
        }
    }
}