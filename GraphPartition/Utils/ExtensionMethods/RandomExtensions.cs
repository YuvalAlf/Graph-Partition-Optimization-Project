using System;
using Utils.MathUtils;

namespace Utils.ExtensionMethods
{
    public static class RandomExtensions
    {
        public static double Next(this Random @this, Range range)
        {
            return @this.NextDouble() * (range.Max - range.Min) + range.Min;
        }
        public static T ChooseRandomly<T>(this T[] @this, Random rnd)
        {
            return @this[rnd.Next(@this.Length)];
        }
    }
}
