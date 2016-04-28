namespace Structura.Shared.Utilities.ValueObjects
{
    public abstract class ValueObjectBase<T> where T : class
    {
        public abstract override string ToString();
        public abstract override int GetHashCode();

        public override bool Equals(object obj)
        {
            return Equals(obj as T);
        }

        public bool Equals(T other)
        {
            return null != other && GetHashCode() == other.GetHashCode();
        }

        protected static bool OperatorEquals(T a, T b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if ((a == null) || (b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Equals(b);
        }
    }
}