namespace Optimizations.LocalSearchAlgorithm
{
    public sealed class LocalSearchSettings
    {
        public static LocalSearchSettings Default => new LocalSearchSettings(1);
        public int AmountInParralel { get; }

        public LocalSearchSettings(int amountInParralel)
        {
            AmountInParralel = amountInParralel;
        }
    }
}
