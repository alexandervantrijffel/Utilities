using System;
using System.Collections.Generic;
using System.Linq;

namespace Structura.SharedComponents.Utilities
{
    public static class EnumerableExtensions
    {
        public static IOrderedEnumerable<TSource> Sort<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, bool descending = false)
        {
            return descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);
        }
    }
}
