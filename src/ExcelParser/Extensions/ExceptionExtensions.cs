
using System;

namespace ExcelParser.Extensions
{
    public static class ExceptionExtensions
    {

        public static string GetDeepestMessage(this Exception ex)
        {
            var realException = ex;
            while (realException.InnerException != null)
            {
                realException = realException.InnerException;
            }

            return realException.Message;
        }
    }
}