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

        public bool Contains(double value)
        {
            return Min <= value && Max >= value;
        }

        public static Range Create(double a, double b)
        {
            var (min, max) = GeneralUtils.MinMax(a, b);
            return new Range(min, max);
        }

        public bool IsOverlapping(Range range)
        {
            return Math.Max(this.Min, range.Min) <= Math.Min(this.Max, range.Max);
        }

        public Range GetOverlap(Range range)
        {
            return new Range(Math.Max(this.Min, range.Min), Math.Min(this.Max, range.Max));
        }
    }
}
