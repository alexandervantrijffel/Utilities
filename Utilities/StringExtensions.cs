using System;
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
    }
}
