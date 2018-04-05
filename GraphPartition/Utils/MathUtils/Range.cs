using System;

namespace Utils.MathUtils
{
    public struct Range
    {
        public double Min { get; }
        public double Max { get; }

        public Range(double min, double max)
        {
            Min = min;
            Max = max;
        }

        public static Range Create(double a, double b)
        {
            var (min, max) = GeneralUtils.MinMax(a, b);
            return new Range(min, max);
        }

        public bool Contains(double value) => Min <= value && Max >= value;
        public bool IsOverlapping(Range range) => Math.Max(this.Min, range.Min) <= Math.Min(this.Max, range.Max);
        public Range GetOverlap(Range range) => new Range(Math.Max(this.Min, range.Min), Math.Min(this.Max, range.Max));


    }
}
