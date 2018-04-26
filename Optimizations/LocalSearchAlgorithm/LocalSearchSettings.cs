namespace Optimizations.LocalSearchAlgorithm
{
    public sealed class LocalSearchSettings<NeighborhoodOption>
    {
        public static LocalSearchSettings<NeighborhoodOption> Default 
            => new LocalSearchSettings<NeighborhoodOption>(1, default(NeighborhoodOption));

        public int AmountInParralel { get; }
        public NeighborhoodOption NeighborsOption { get; }

        public LocalSearchSettings(int amountInParralel, NeighborhoodOption option)
        {
            AmountInParralel = amountInParralel;
            NeighborsOption = option;
        }
    }
}
