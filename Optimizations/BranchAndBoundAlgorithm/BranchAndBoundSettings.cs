namespace Optimizations.BranchAndBoundAlgorithm
{
    public sealed class BranchAndBoundSettings<UpperBound>
    {
        public static BranchAndBoundSettings<UpperBound> Default  => new BranchAndBoundSettings<UpperBound>(default(UpperBound));

        public UpperBound UpperBoundScheme { get; }

        public BranchAndBoundSettings(UpperBound upperBoundScheme)
        {
            UpperBoundScheme = upperBoundScheme;
        }
    }
}
