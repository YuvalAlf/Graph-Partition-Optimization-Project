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
    }
}
