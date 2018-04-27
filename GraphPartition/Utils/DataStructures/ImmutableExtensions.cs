using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Utils.DataStructures
{
    public static class ImmutableExtensions
    {
        public static ImmutableHashSet<T> AddRange<T>(this ImmutableHashSet<T> @this, IEnumerable<T> items)
        {
            return items.Aggregate(@this, (values, item) => values.Add(item));
        }
    }
}
