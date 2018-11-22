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

        /// <summary>
        /// Remove special character from string
        /// </summary>
        /// <param name="input">Input/string complete to be removed the special characters</param>
        /// <param name="acceptWhiteSpace">True if white space will be a valid character</param>
        /// <returns>String without special characters respecting the acception of white space</returns>
        public static string RemoveSpecialCharacters(this string input, bool acceptWhiteSpace)
        {
            string ret = input;

            if (string.IsNullOrEmpty(ret))
                return ret;

            ret = ret.Replace("'", " ").Replace("`", " ").Replace("´", " ").Replace("~", " ").Replace("^", " ").Replace(@"\", " ").Replace("¨", " ");

            if (acceptWhiteSpace)
                ret = System.Text.RegularExpressions.Regex.Replace(ret, @"[^0-9a-zA-ZéúíóáÉÚÍÓÁèùìòàÈÙÌÒÀõãñÕÃÑêûîôâÊÛÎÔÂëÿüïöäËYÜÏÖÄçÇ\s]+?", string.Empty);
            else
                ret = System.Text.RegularExpressions.Regex.Replace(ret, @"[^0-9a-zA-ZéúíóáÉÚÍÓÁèùìòàÈÙÌÒÀõãñÕÃÑêûîôâÊÛÎÔÂëÿüïöäËYÜÏÖÄçÇ]+?", string.Empty);

            return ret;
        }

        /// <summary>
        /// Remove emoticons (emoji) from string
        /// </summary>
        /// <param name="input">Input/string complete to be removed the emoticons</param>        
        /// <returns>Just string without emoticons</returns>
        public static string RemoveEmoticons(this string input)
        {
            string ret = string.Empty;

            /* ########################################################################
             #                                                                        #
             #  Obs: Description of each "group" Regex to be removed                  #
             #                                                                        #
             #   "[\x1F600-\x1F64F]" ==> Emoticons                                    #
             #   "[\x1F300-\x1F5FF]" ==> Miscellaneous Symbols and Pictographs        #
             #   "[\x1F680-\x1F6FF]" ==> Match Transport And Map Symbols              #
             #   "[\x1F1E0-\x1F1FF]" ==> Match flags (iOS)                            #
             #                                                                        #
              ######################################################################### */

            string regexEmoticons = "[\x1F600-\x1F64F\x1F300-\x1F5FF\x1F680-\x1F6FF\x1F1E0-\x1F1FF?!( !@#$%¨&*\"'!;.:,?+\\/\t\r\v\f\n)?!-]";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(regexEmoticons);


            System.Text.RegularExpressions.MatchCollection matches = regex.Matches(ret);
            foreach (System.Text.RegularExpressions.Match match in matches)
                ret += match.Value;

            return ret;
        }

        /// <summary>
        /// Convert string to decimal exchanging comma to dot
        /// </summary>
        /// <param name=value"></param>
        /// <returns>Retorna o número pronto para ser usado</returns>
        public static decimal ExchangeCommaToDotAndReturnsDecimal(this string value)
        {
            decimal conversor = 0.00M;
            //Substitui as "," por "." e converte para AllowDecimalPoint
            if (decimal.TryParse(value.Replace(',', '.'), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, out conversor))
            {
                value = conversor.ToString();
            }
            //Retorna o decimal
            return Decimal.Parse(value);
        }

        public enum DecimalSeparator
        {
            Comma,
            Dot
        }

        /// <summary>
        /// Convert decimal to string according to the desired decimal separator
        /// </summary>
        /// <param name="value">Decimal value</param>
        /// <param name="decimalSeparator">Separador desejado</param>
        /// <returns>Retorna o decimal em formato de string com o separador de decimal desejado</returns>
        public static string ReturnsStringWithDesiredDecimalSeparator(this decimal value, DecimalSeparator decimalSeparator = DecimalSeparator.Dot)
        {
            try
            {
                if (decimalSeparator == DecimalSeparator.Dot)
                    //To decimal be dotasso pass the culture as Invariant
                    return value.ToString(CultureInfo.InvariantCulture.NumberFormat);
                else
                    //To decimal be comma pass the culture as pt-BR
                    return value.ToString(new CultureInfo("pt-BR"));
            }
            catch
            {
                return value.ToString();
            }
        }
    }
}
