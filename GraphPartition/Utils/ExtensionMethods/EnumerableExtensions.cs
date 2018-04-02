using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils.ExtensionMethods
{
    public static class EnumerableExtensions
    {
        public static (T, double) MinBy<T>(this IEnumerable<T> @this, Func<T, double> minBy)
            where T : class 
        {
            T bestT = null;
            double bestVal = double.MaxValue;
            foreach (var item in @this)
            {
                var val = minBy(item);
                if (val < bestVal)
                {
                    bestVal = val;
                    bestT = item;
                }
            }

            return (bestT, bestVal);
        }
        public static S Min<T, S>(this IEnumerable<T> @this, Func<T, S> selector, S @default)
        {
            return @this.Select(selector).Append(@default).Min();
        }
        public static S Max<T, S>(this IEnumerable<T> @this, Func<T, S> selector, S @default)
        {
            return @this.Select(selector).Append(@default).Max();
        }
    }
}
