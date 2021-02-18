using System;
using System.Collections.Generic;
using System.Linq;

namespace ExcelParser.Helpers
{
    public static class EnumHelper
    {
        public static List<T> ToList<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException(string.Format("{0} must be an enumerated type", typeof(T).FullName));
            }

            return ((T[])Enum.GetValues(typeof(T))).ToList();
        }
    }
}