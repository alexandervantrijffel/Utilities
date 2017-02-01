using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Structura.Shared.Utilities
{
    public class ContentsAsString
    {
        private readonly int _maxCollectionItemsToDump;
        private readonly int _maxFieldOutputLength;
        public const int DONOTTRUNCATECOLLECTIONS = -1;
        public const int DONOTTRUNCATEFIELD = -1;

        private IList<object> _objectsDumped;

        public string Value { get; }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(ContentsAsString cas)  // implicit digit to byte conversion operator
        {
            return cas.Value;
        }

        public ContentsAsString(dynamic @object, int maxCollectionItemsToDump = DONOTTRUNCATECOLLECTIONS, int maxFieldOutputLength = DONOTTRUNCATEFIELD)
        {
            _objectsDumped = new List<object>();
            _maxCollectionItemsToDump = maxCollectionItemsToDump;
            _maxFieldOutputLength = maxFieldOutputLength;
            Value = DumpTypeAndFields(@object);
        }

        string DumpTypeAndFields(dynamic o)
        {
            if (Object.ReferenceEquals(null, o))
            {
                return "NULL";
            }
            // process only once, prevent stack overflow
            if (_objectsDumped.Contains((object)o)) { return string.Empty; }
            _objectsDumped.Add((object)o);
            Type type = o.GetType();
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
            {
                KeyValuePair<object, object> keyValue = CastFrom(o);
                return GetValue(keyValue.Key) + ", " + GetValue(keyValue.Value);
            }
            if (type.IsPrimitive || (!string.IsNullOrWhiteSpace(type.Namespace) && type.Namespace.StartsWith("System")))
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

        string GetValue(dynamic theValue)
        {
            if (Object.ReferenceEquals(null, theValue))
            {
                return "NULL";
            }
            Type itsType = theValue.GetType();
            if (!(theValue is string) && typeof(IEnumerable).IsAssignableFrom(itsType))
            {
                return "[" + string.Join(" ,, ", DumpCollection(theValue, _maxCollectionItemsToDump)) + "]";
            }
            if (itsType.IsEnum)
            {
                return Enum.GetName(itsType, theValue);
            }
            if (!itsType.IsPrimitive && !itsType.Namespace.StartsWith("System"))
            {
                return DumpTypeAndFields(theValue);
            }
            string value = theValue.ToString();
            if (_maxFieldOutputLength != DONOTTRUNCATEFIELD && _maxFieldOutputLength > 0 && value.Length > _maxFieldOutputLength)
            {
                return value.Substring(0, _maxFieldOutputLength) +
                       $" (truncated at {_maxFieldOutputLength} characters - {value.Length} total length)";
            }
            return value;
        }

        IEnumerable<string> DumpCollection(IEnumerable theValue, int maximumItemsToInclude)
        {
            int counter = 0;
            foreach (var val in theValue)
            {
                if (maximumItemsToInclude != DONOTTRUNCATECOLLECTIONS && maximumItemsToInclude > 0 && counter++ >= maximumItemsToInclude)
                {
                    yield return $" (output truncated to {maximumItemsToInclude} items)";
                    yield break;
                }
                yield return DumpTypeAndFields(val);
            }
        }

        static KeyValuePair<object, object> CastFrom(Object obj)
        {
            var type = obj.GetType();
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
            {
                var key = type.GetProperty("Key");
                var value = type.GetProperty("Value");
                var keyObj = key.GetValue(obj, null);
                var valueObj = value.GetValue(obj, null);
                return new KeyValuePair<object, object>(keyObj, valueObj);
            }
            throw new ArgumentException(" ### -> public static KeyValuePair<object , object > CastFrom(Object obj) : Error : obj argument must be KeyValuePair<,>");
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
