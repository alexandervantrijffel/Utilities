using System;
using System.Diagnostics;

namespace Structura.Shared.Utilities
{
    [Serializable]
    public class Maybe<T> where T : class
    {
        private readonly T _value;

        public Maybe(T value)
        {
            _value = value;
        }

        public bool HasValue => _value != null;

        /// <summary>
        /// Throws revealing exception if the value is empty.
        /// </summary>
        public T Value
        {
            get
            {
                if (!HasValue)
                {
                    var method = new StackFrame(1, true).GetMethod().Name;
                    var type = typeof(T).Name;
                    throw new InvalidOperationException(
						$"Maybe.Value requested in {method} while the value is null. Type of requested value is {type}.");
                }

                return _value;
            }
        }

        public override bool Equals(object obj)
        {
            // false for e.g. "bike"
            if (!ReferenceEquals(obj, null) && !(obj is Maybe<T>))
            {
                return false;
            }
            //at that moment obj is either null or some Percentage

            var other = (Maybe<T>)obj;
            if (ReferenceEquals(other, null) || !other.HasValue)
            {
                return !HasValue;
            }

            if (HasValue ^ other.HasValue)
            {
                return false;
            }

            return Value.Equals(other.Value);
        }

        public static bool operator ==(Maybe<T> m1, Maybe<T> m2)
        {
            if (ReferenceEquals(m1, null) && ReferenceEquals(m2, null))
            {
                return true;
            }
            if (ReferenceEquals(m1, null))
            {
                return m2.Equals(m1);
            }
            return m1.Equals(m2);
        }

        public static bool operator !=(Maybe<T> x, Maybe<T> y)
        {
            return !(x == y);
        }

        public override int GetHashCode()
        {
            return HasValue ? Value.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return HasValue ? Value.ToString() : string.Empty;
        }
    }
}

