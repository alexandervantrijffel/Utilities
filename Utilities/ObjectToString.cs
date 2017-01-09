using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Structura.Shared.Utilities
{
    public static class ObjectToString
    {
        public static string GetObjectFields(object o)
        {
            return JsonConvert.SerializeObject(o, 
                Formatting.Indented,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include });
        }

        public static string DumpTypeAndFields(dynamic o)
        {
            Type type = o.GetType();
            if (type.IsPrimitive || type.Namespace.StartsWith("System"))
            {
                return GetValue(o);
            }
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var sb = new StringBuilder();
            sb.Append(((string)type.Name).TrimEndString("_impl"));
            sb.Append("\r\n");
            var rex = new Regex(@"<([\w\s]+)>");
            foreach (FieldInfo f in fields)
            {
                var matches = rex.Matches(f.Name);
                // support properties                    support fields
                sb.Append((matches.Count > 0) ? rex.Matches(f.Name)[0].Groups[1].Value : f.Name.Replace("field_", string.Empty));
                sb.Append(": ");
                var theValue = f.GetValue(o);
                sb.Append(GetValue(theValue));
                sb.Append("\r\n");
            }
            return sb.ToString();
        }

        private static string GetValue(dynamic theValue)
        {
            if (((Type)theValue.GetType()).IsClass && Object.ReferenceEquals(null, theValue))
            {
                return "NULL";
            }
            if (!(theValue is string) && typeof(IEnumerable).IsAssignableFrom(theValue.GetType()))
            {
                return string.Join(" ,", DumpCollection(theValue, 5));
            }
            return theValue.ToString();
        }

        private static IEnumerable<string> DumpCollection(IEnumerable theValue, int maximumItemsToInclude)
        {
            int counter = 0;
            foreach (var val in theValue)
            {
                if (maximumItemsToInclude > 0 && counter++ >= maximumItemsToInclude)
                {
                    yield return $" (output truncated to {maximumItemsToInclude} items)";
                    yield break;
                }
                yield return DumpTypeAndFields(val);
            }
        }
    }
}
