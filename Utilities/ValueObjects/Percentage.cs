using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Structura.SharedComponents.Utilities.ValueObjects
{
    // ReSharper disable PossibleInvalidOperationException
    [ComplexType]
    [Serializable]
    [TypeConverter(typeof(PercentageTypeConverter))]
    public class Percentage : IComparable<Percentage>, IComparable
    {
        private double? value;
        [Obsolete("Use ToDecimal() or ToPercents() method for an explicite intention. This property is public to enable persistence with Entity Framework.", true)]
        public double? Value
        {
            get { return value; }
            private set { this.value = value; }
        }

        private Percentage(double value, bool maximizeToHundred = false)
        {
            if (maximizeToHundred && (value < 0 || value > 1))
            {
                throw new ArgumentException("A percentage must be between 0 and 100.");
            }
            this.value = value;
        }

        internal Percentage()
        {
        }
        /// <summary>
        /// Checks whether the value is not null or 0
        /// </summary>
        /// <param name="percentage"></param>
        /// <returns></returns>
        public static bool IsGreatherThanZero(Percentage value)
        {
            return value != null && value != NullValue && value > FromDecimal(0);
        }
        /// <summary>
        /// Example: 1000 (10%)
        /// </summary>
        /// <param name="basepoints"></param>
        /// <returns></returns>
        public static Percentage FromBps(int basepoints)
        {
            return new Percentage(basepoints / 10000.0);
        }
        
        /// <summary>
        /// Example: 10 (10%)
        /// </summary>
        /// <param name="decimalRepresentation"></param>
        /// <returns></returns>
        public static Percentage FromPercents(double percents, bool maximizeToHundred = false)
        {
            return new Percentage(percents / 100.0, maximizeToHundred);
        }

        /// <summary>
        /// Example: 0.1 (10%)
        /// </summary>
        public static Percentage FromDecimal(double decimalRepresentation)
        {
            return new Percentage(decimalRepresentation);
        }

        public static Percentage NullValue
        {
            get { return new Percentage {value = null}; }
        }

        public static Percentage Parse(string value)
        {
            double d;
            var trimmed = value.Trim();

            if (trimmed == string.Empty)
            {
                return NullValue;
            }

            if (trimmed.EndsWith("bps"))
            {
                trimmed = trimmed.Substring(0, trimmed.Length - 3);
                int i;
                if (!int.TryParse(trimmed, out i))
                {
                    throw new ArgumentException("Cannot parse percentage value: " + value);
                }
                return FromBps(i);

            }
            if (trimmed.EndsWith("%"))
            {
                trimmed = trimmed.Substring(0, trimmed.Length - 1);
                if (!double.TryParse(trimmed, out d))
                {
                    throw new ArgumentException("Cannot parse percentage value: " + value);
                }
                return FromPercents(d);
            }

            //temp (?) accept value with a missing % sign as a value in percents (because of how it's passed from UI inputs)...
            if (!double.TryParse(trimmed, out d))
            {
                throw new ArgumentException("Cannot parse percentage value: " + value);
            }
            //...here
            return FromPercents(d);
        }
        
        public override bool Equals(object obj)
        {
            // false for e.g. "bike"
            if (!ReferenceEquals(obj, null) && !(obj is Percentage))
            {
                return false;
            }
            //at that moment obj is either null or some Percentage

            var other = (Percentage) obj;
            if (ReferenceEquals(other, null) || !other.value.HasValue)
            {
                return !value.HasValue;
            }

            if (value.HasValue ^ other.value.HasValue)
            {
                return false;
            }
            
            return value.Value.AlmostEquals(other.value.Value);
        }

        public static bool operator ==(Percentage p1, Percentage p2)
        {
            if (ReferenceEquals(p1, null) && ReferenceEquals(p2, null))
            {
                return true;
            }
            if (ReferenceEquals(p1, null))
            {
                return p2.Equals(p1);
            }
            return p1.Equals(p2);
        }

        public static bool operator !=(Percentage p1, Percentage p2)
        {
            return !(p1 == p2);
        }

        public override int GetHashCode()
        {
            if (!value.HasValue)
            {
                return 0;
            }

            return value.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            if (!(obj is Percentage))
            {
                throw new ArgumentException(string.Format("Percentage expected but {0} passed in", obj.GetType().Name));
            }
            return CompareTo((Percentage) obj);
        }

        public static bool operator >(Percentage a, Percentage b)
        {
            return !a.Equals(b) && a.value > b.value;
        }

        public static bool operator <(Percentage a, Percentage b)
        {
            return !a.Equals(b) && a.value < b.value;
        }

        public static bool operator >=(Percentage a, Percentage b)
        {
            return a.value >= b.value;
        }

        public static bool operator <=(Percentage a, Percentage b)
        {
            return a.value <= b.value;
        }

        public static Percentage operator +(Percentage a, Percentage b)
        {
            return new Percentage(a.value.Value + b.value.Value);
        }

        public static Percentage operator -(Percentage a, Percentage b)
        {
            return new Percentage(a.value.Value - b.value.Value);
        }

        public static Percentage operator *(Percentage a, double multiplier)
        {
            return new Percentage(a.value.Value * multiplier);
        }

        public static Percentage operator *(Percentage a, decimal multiplier)
        {
            return new Percentage(a.value.Value * (double)multiplier);
        }

        public static Percentage operator *(Percentage a, Percentage b)
        {

            return new Percentage(a.value.Value * b.value.Value);
        }

        public static Percentage operator /(Percentage a, double divisor)
        {
            return new Percentage(a.value.Value / divisor);
        }

        public static Percentage operator /(Percentage a, decimal divisor)
        {
            return new Percentage(a.value.Value / (double)divisor);
        }

        public static double operator /(double value, Percentage divisor)
        {
            return value / (divisor.value.Value);
        }

        public static double operator /(Percentage a, Percentage divisor)
        {
            return a.value.Value / divisor.value.Value;
        }

        //todo remove after moving to decimal
        public double Multiply(double input)
        {
            return value.Value * input;
        }

        public decimal Multiply(decimal input)
        {
            return (decimal)(value.Value * (double)input);
        }

        public Percentage Round(int digits)
        {
            if (value == null)
            {
                return NullValue;
            }
            return new Percentage(Math.Round(value.Value, digits + 2));
        }

        public Percentage Ceiling()
        {
            return FromPercents(Math.Ceiling(Math.Round(value.Value * 100, 9)));
        }

        public static Percentage Max(Percentage val1, Percentage val2)
        {
            return val1.value > val2.value ? val1 : val2;
        }
        
        public int CompareTo(Percentage other)
        {
            if (other == null)
            {
                return 1;
            }

            return value.Value.CompareTo(other.value);
        }

        public override string ToString()
        {
            if (value.HasValue)
            {
                return string.Format("{0:0.#####}", value * 100);
            }
            return string.Empty;
        }

        /// <summary>
        /// Supprted formats:
        /// % -> 3%
        /// bps -> 300bps 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="includeUnitSign"></param>
        /// <returns></returns>
        public string ToString(string format, bool includeUnitSign)
        {
            if (!value.HasValue)
            {
                return string.Empty;
            }

            string valuePart;
            string signPart;
            
            if (format == "%")
            {
                valuePart = ToPercents().ToString();
                signPart = "%";
            }
            else if (format == "bps")
            {
                valuePart = ToBps().ToString();
                signPart = "%";
            }
            else
            {
                throw new ArgumentException(format + " is not a supported format");
            }

            signPart = includeUnitSign ? signPart : string.Empty;

            return valuePart + signPart;
        }
        [DebuggerStepThrough]
        public double ToDouble()
        {
            return value.Value;
        }
        [DebuggerStepThrough]
        public double ToPercents()
        {
            return value.Value*100;
        }
        [DebuggerStepThrough]
        public int ToBps()
        {
            return (int)(value.Value * 10000);
        }
        
        public static implicit operator Percentage(string stringRep)
        {
            //add culture invariant as an arg in Parse
            return Parse(stringRep);
        }
    }

    public class PercentageTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return Percentage.Parse((string)value);
        }
    }
}
