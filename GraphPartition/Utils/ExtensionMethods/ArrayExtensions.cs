using System;
using System.Linq;

namespace Utils.ExtensionMethods
{
    public static class ArrayExtensions
    {
        public static S Min<T, S>(this T[] @this, Func<T, S> selector, S @default)
        {
            if (@this.Length == 0)
                return @default;
            return @this.Min(selector);
        }
        public static S Max<T, S>(this T[] @this, Func<T, S> selector, S @default)
        {
            if (@this.Length == 0)
                return @default;
            return @this.Max(selector);
        }
    }
}
