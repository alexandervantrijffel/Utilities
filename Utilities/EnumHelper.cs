using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Structura.SharedComponents.Utilities
{
	public static class EnumHelper
    {
        public class EnumValueInfo
        {
            public string DisplayName { get; set; }
            public string EnumTypeName { get; set; }
            public string Name { get; set; }
            public int Value { get; set; }

            public EnumValueInfo(string enumTypeName, string name, string displayName, int value)
            {
                DisplayName = displayName;
                EnumTypeName = enumTypeName;
                Name = name;
                Value = value;
            }

            public override string ToString()
            {
                return string.Format(" ({1}.{2}={0}) ", Value, EnumTypeName, Name);
            }
        }
        public class EnumInfo
        {
            public Type EnumerationType { get; set; }
            public ICollection<EnumValueInfo> Values { get; set; }

            public EnumInfo(Type enumerationType)
            {
                EnumerationType = enumerationType;
                Values = new List<EnumValueInfo>();
            }
        }
        public static IEnumerable<EnumInfo> GetEnumsInAssembly(Type typeInAssembly)
        {
            return typeInAssembly.Assembly.GetTypes().Where(t => t.IsEnum).Select(theEnum => GetEnumInfoForType(theEnum));
        }

        public static EnumInfo GetEnumInfoForType(Type theType)
        {
            var result = new EnumInfo(theType);
            foreach (var memberInfo in theType.GetMembers(BindingFlags.Public | BindingFlags.Static))
            {
                result.Values.Add(new EnumValueInfo(theType.Name,
                        memberInfo.Name, memberInfo.GetDisplayNameOrName(),
                        (int)Enum.Parse(theType, memberInfo.Name)));
            }
            return result;
        }

        public static T ToEnum<T>(this string stringValue) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            Type enumType;
            if (typeof(T).IsGenericType &&
                typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                enumType = typeof(T).GetGenericArguments().Single();
                if (string.IsNullOrWhiteSpace(stringValue))
                {
                    return default(T);
                }
            }
            else
            {
                enumType = typeof(T);
            }

            if (stringValue.Equals("N/a", StringComparison.InvariantCultureIgnoreCase)) return (T)Enum.ToObject(typeof(T), 0);

            var enumValues = Enum.GetValues(enumType);

            foreach (var enumValue in enumValues)
            {
                var enumName = Enum.GetName(enumType, enumValue);
                if (stringValue.Equals(enumName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return (T)enumValue;
                }
                var displayName = GetDisplayName(enumValue, false);
                if (stringValue.Equals(displayName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return (T)enumValue;
                }
            }
            throw new ArgumentException(string.Format("Cannot find a value of '{0}' in enum {1}", stringValue, enumType.Name));
        }

        public static string GetDisplayName(object e, bool throwIfNotfound)
        {
            var enumValue = e.ToString();
            var m = e.GetType().GetMember(enumValue);
            if (!m.Any())
            {
                return null;
            }
            var member = m.First();
            var attribs = member.GetCustomAttributes(typeof(DisplayAttribute), false);


            if (attribs.Length == 0)
            {
                return enumValue;
            }

            var firstAttr = (DisplayAttribute)attribs[0];
            if (firstAttr.Name == null)
            {
                if (throwIfNotfound)
                {
                    throw new Exception(String.Format("Display attribute on {0}.{1} doesn't have a Name defined.",
                        e.GetType().Name, e));
                }
                return enumValue;
            }
            return firstAttr.Name;
        }

        public static int GetElementCount(Type enumType)
        {
            return Enum.GetNames(enumType).Length;
        }
    }
}
