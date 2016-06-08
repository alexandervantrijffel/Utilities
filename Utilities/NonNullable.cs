namespace Structura.Shared.Utilities
{
    public class NonNullable<T> : Maybe<T> where T : class
    {
        public NonNullable(T value) : base(value)
        {
            // test value, throws exception if it is empty
            var val = Value;
        }
    }
}