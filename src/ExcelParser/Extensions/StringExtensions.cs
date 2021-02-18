
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExcelParser.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }

        public static bool TryGetEnum<T>(this string text, out T enumValue) where T : struct, IConvertible
        {
            enumValue = default(T);

            try
            {
                enumValue = (T)Enum.Parse(typeof(T), text);
            }
            catch (Exception)
            {
                return false;
            }

            return text.IsValidEnum<T>();
        }

        public static bool IsValidEnum<T>(this string text) where T : struct, IConvertible
        {
            if (text.IsEmpty()) return false;

            try
            {
                T parsedValue = (T)Enum.Parse(typeof(T), text);
                return Enum.IsDefined(typeof(T), parsedValue);
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}