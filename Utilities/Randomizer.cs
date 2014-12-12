using System;

namespace Structura.SharedComponents.Utilities
{
    public class Randomizer
    {
        public static int NextInt(int minValue, int maxValue)
        {
            return new Random((int)DateTime.UtcNow.Ticks).Next(minValue, maxValue);
        }

        public static string NextString(int length)
        {
            var r = new Random((int)DateTime.UtcNow.Ticks);
            var letters = new char[length];
            for (int i = 0; i < length; i++)
            {
                letters[i] = GenerateChar(r);
            }
            return new string(letters);
        }

        private static char GenerateChar(Random r)
        {
            // 'Z' + 1 because the range is exclusive
            return (char)(r.Next('A', 'Z' + 1));
        }
    }
}
