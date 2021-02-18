using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CsvHelper.Configuration;
using ExcelParser.Csv.Exceptions;
using ExcelParser.Extensions;

namespace ExcelParser.Csv.Models
{
    public interface ICustomClassMap
    {
        Dictionary<string, ClassMapHeaderDefinition> GetHeaderDefinitions();
    }

    public abstract class CustomClassMap<T> : ClassMap<T>, ICustomClassMap
    {
        protected string GetPropName<TProp>(Expression<Func<T, TProp>> func)
        {

            var expression = (MemberExpression)func.Body;
            return expression.Member.Name;
        }

        protected abstract Dictionary<string, ClassMapHeaderDefinition> HeaderDefinitions { get; set; }
        
        public Dictionary<string, ClassMapHeaderDefinition> GetHeaderDefinitions()
        {
            return HeaderDefinitions;
        }

        protected void Init()
        {
            if (MemberMaps.IsNullOrEmpty())
            {
                throw new CustomClassMapSetupException($"CustomClassMap must have some configurations for MemberMaps");
            }

            foreach (var memberMap in MemberMaps)
            {
                var name = memberMap.Data.Names.FirstOrDefault();
                if (name.IsEmpty())
                {
                    throw new CustomClassMapSetupException($"Cannot find a name for member map");
                }
                if (!HeaderDefinitions.ContainsKey(name))
                {
                    throw new CustomClassMapSetupException($"Cannot find key {name} when setting up CustomClassMap");
                }

                HeaderDefinitions[name].TypedPropertyName = memberMap.Data.Member.Name;
                HeaderDefinitions[name].Required = !memberMap.Data.IsOptional;
            }
        }
        
    }
}