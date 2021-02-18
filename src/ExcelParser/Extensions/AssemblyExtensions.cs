
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExcelParser.Extensions
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type> GetAllClassesFromType<T>(this AppDomain apDomain)
        {
            return apDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(T).IsAssignableFrom(p) && !p.IsInterface && p.IsClass && !p.IsAbstract);
        }
    }
}