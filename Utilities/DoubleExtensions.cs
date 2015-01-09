using System;

namespace Structura.SharedComponents.Utilities
{
    public static class DoubleExtensions
    {
        // Tests if a value is not equal to another value if rounding
        // difference are not taken into account
        public static bool AlmostEquals(this double initialValue, double value)
        {
            var difference = Math.Round(initialValue - value, 15);
            return difference.Equals(0);
        }
    }
}
