using System;
using System.Collections.Generic;
using System.Linq;

namespace Structura.SharedComponents.Utilities
{
    public static class ListExtensions
    {
        public static void AddOrUpdate<T>(this IList<T> coll, T item, Func<T, T, bool> equalityComparer)
        {
            for (int i = 0; i < coll.Count; i++)
            {
                if (equalityComparer(coll[i], item))
                {
                    coll[i] = item;
                }
            }
            coll.Add(item);
        }

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
