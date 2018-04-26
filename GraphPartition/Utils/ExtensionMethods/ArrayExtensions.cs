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
        public static T[] Copy<T>(this T[] @this) 
            => @this.Clone() as T[];

        public static S[] Map<T,S>(this T[] @this, Func<T, S> mapFunc) 
            => @this.Select(mapFunc).ToArray();

        public static T ChooseRandomly<T>(this T[] @this, Random rnd) 
            => @this[rnd.Next(@this.Length)];

        public static T[] InitArray<T>(int length, Func<int, T> initilizor)
            => Enumerable.Range(0, length).Select(initilizor).ToArray();
    }
}
