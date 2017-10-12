using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Structura.Shared.Utilities
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            if (!string.IsNullOrEmpty(input) && Char.IsLower(input[0]))
            {
                return Char.ToUpper(input[0]) + input.Substring(1);
            }
            return input;
        }
        public static bool StartsWithVowel(this string input)
        {
            const string vowels = "aeiou";
            return !string.IsNullOrEmpty(input) && vowels.Contains(Char.ToLower(input[0]));
        }

        public static string ToBase(this long input, string baseChars)
        {
            string r = string.Empty;
            int targetBase = baseChars.Length;
            do
            {
                r = string.Format("{0}{1}",
                                  baseChars[(int)(input % targetBase)],
                                  r);
                input /= targetBase;
            } while (input > 0);

            return r;
        }

        public static long FromBase(this string input, string baseChars)
        {
            var srcBase = baseChars.Length;
            var r = new string(input.ToCharArray().Reverse().ToArray());
            return
                r.Select(t => baseChars.IndexOf(t)).Select((charIndex, i) => charIndex * (long)Math.Pow(srcBase, i)).Sum();
        }

        public static string TrimEndString(this string input, string suffixToRemove)
        {
            if (input != null && suffixToRemove != null && input.EndsWith(suffixToRemove))
            {
                return input.Substring(0, input.Length - suffixToRemove.Length);
            }
            return input;
        }

        public static bool TryParseAsDateTimeUtc(this string s, out DateTime dt)
        {
            var utcFormats = new[] {
                          "yyyy-MM-ddThh:mm:ssZ",
                          "yyyy-MM-ddThh:mm:ss.ffZ",
                          "yyyy-MM-ddThh:mm:ss.fffZ",
                          "yyyy-MM-ddThh:mm:ss.ffffZ",
                          "yyyy-MM-ddThh:mm:ss.fffffZ",
                          "yyyy-MM-ddThh:mm:ss.ffffffZ",
                          "yyyy-MM-ddThh:mm:ss.fffffffZ",
                          "o"
                };
            return DateTime.TryParseExact(s, utcFormats, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out dt);
        }

        public static string RemoveSubstring(this string s, string stringToBeRemoved)
        {
            return s.Replace(stringToBeRemoved, string.Empty);
        }

        public static string ToAbsolutePath(this string relativeOrAbsolutePath)
        {
            if (relativeOrAbsolutePath == null) throw new ArgumentNullException(nameof(relativeOrAbsolutePath));

            var pathToMakeAbsolute = Path.IsPathRooted(relativeOrAbsolutePath)
                ? relativeOrAbsolutePath
                : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeOrAbsolutePath);

            return Path.GetFullPath(pathToMakeAbsolute);
        }
    }
}
