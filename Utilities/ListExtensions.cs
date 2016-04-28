using System;
using System.Collections.Generic;
using System.Linq;

namespace Structura.Shared.Utilities
{
    public static class ListExtensions
    {
        public static bool TryUpdateExisting<T>(this IList<T> list, Func<T, bool> selector, T replacement)
        {
            var existing = list.FirstOrDefault(selector);
            if (existing != null)
            {
                list[list.IndexOf(existing)] = replacement;
            }
            return existing != null;
        }
    }
}
