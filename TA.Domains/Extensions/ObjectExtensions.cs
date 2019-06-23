using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace TA.Domains.Extensions
{
    public static class ObjectExtensions
    {
        public static IEnumerable<object> GetKeyProperties(this object value)
        {
            return value.GetType().GetProperties().Where(a => a.GetCustomAttribute(typeof(KeyAttribute)) != null).Select(pi => pi.GetValue(value));
        }

        
        public static bool IsDefault(this object val)
        {
            switch (val)
            {
                case Guid gUid:
                    return gUid == default;
                case short shortVal :
                    return shortVal == default;
                case int intVal:
                    return intVal == default;
                case long longVal:
                    return longVal == default;
                case decimal decimalVal:
                    return decimalVal == default;
                case float floatVal:
                    return floatVal == default;
                case string stringVal:
                    return string.IsNullOrEmpty(stringVal);
                default:
                    return false;
            }
        }
    }
}