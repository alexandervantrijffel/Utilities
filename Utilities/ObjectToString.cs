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
            sb.Append(o.GetType().Name);
            sb.Append("\r\n");
            var rex = new Regex(@"<([\w\s]+)>");
            foreach (FieldInfo f in fields)
            {
                sb.Append("\t");
                sb.Append(rex.Matches(f.Name)[0].Groups[1].Value);
                sb.Append(": ");
                sb.Append(f.GetValue(o));
                sb.Append("\r\n");
            }
            return sb.ToString();
        }
    }
}
