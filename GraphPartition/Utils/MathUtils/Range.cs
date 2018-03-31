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
    }
}
