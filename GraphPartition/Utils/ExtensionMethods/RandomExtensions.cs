using System;
using System.Linq;
using Utils.MathUtils;

namespace Utils.ExtensionMethods
{
    public static class RandomExtensions
    {
        public static double Next(this Random @this, Range range) 
            => @this.NextDouble() * (range.Max - range.Min) + range.Min;


    }
}
