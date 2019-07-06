using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TA.Domains.Extensions
{
    public static class ObjectExtensions
    {
        public static async Task ForEach<T>(this IEnumerable<T> items, Func<T, Task> action)
        {
            foreach (var item in items)
            {
                await action(item);   
            }
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);   
            }
        }

        public static IEnumerable<object> GetKeyProperties(this object value)
        {
            var valueType = value.GetType();

            return valueType.GetProperties()
                .Where(a => a.GetCustomAttribute(typeof(KeyAttribute)) != null)
                .Select(pi => pi.GetValue(value)).ToArray();
        }

        public static bool IsDefault(this object val)
        {
            switch (val)
            {
                case byte byteVal:
                    return byteVal == default;
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