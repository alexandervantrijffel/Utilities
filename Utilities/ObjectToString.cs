using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="propertiesToExclude">An array of property names that should not be added to the result string</param>
        /// <returns></returns>
        public static string PropertiesToString(this object o, string[] propertiesToExclude)
        {
            var propertiesString = new StringBuilder();
            foreach (PropertyInfo prop in o.GetType().GetProperties())
            {
                if (null != propertiesToExclude && propertiesToExclude.Contains(prop.Name))
                    continue;

                if (propertiesString.Length > 0)
                    propertiesString.Append(", ");

                propertiesString.Append(prop.Name);
                propertiesString.Append(": ");
                propertiesString.Append(prop.GetValue(o, null));
            }
            return propertiesString.ToString();
        }

        public static string DumpTypeAndFields(dynamic o)
        {
            var fields = o.GetType().GetFields(BindingFlags.Public |
                                               BindingFlags.NonPublic |
                                               BindingFlags.Instance);
            var sb = new StringBuilder();
            sb.Append(((string)o.GetType().Name).TrimEndString("_impl"));
            sb.Append("\r\n");
            var rex = new Regex(@"<([\w\s]+)>");
            foreach (FieldInfo f in fields)
            {
                var matches = rex.Matches(f.Name);
                                                // support properties                    support fields
                sb.Append((matches.Count > 0) ? rex.Matches(f.Name)[0].Groups[1].Value : f.Name.Replace("field_", string.Empty));
                sb.Append(": ");
                var theValue = f.GetValue(o);
                if (theValue == null)
                {
                    sb.Append("NULL");
                }
                else if (typeof(IEnumerable).IsAssignableFrom(theValue.GetType()))
                {
                    sb.Append(string.Join(" ,", DumpCollection(theValue, 5)));
                }
                else
                {
                    sb.Append(theValue);
                }
                sb.Append("\r\n");
            }
            return sb.ToString();
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
                yield return val.ToString();
            }
        }
    }
}
