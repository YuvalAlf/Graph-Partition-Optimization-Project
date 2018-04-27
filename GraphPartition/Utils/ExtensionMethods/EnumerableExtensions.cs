using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils.ExtensionMethods
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> @this, Random rnd)
            => @this.OrderBy(_ => rnd.Next());
    }
}
