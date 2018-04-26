using ArgumentOutOfRangeException = System.ArgumentOutOfRangeException;

namespace Graphs.Algorithms.LocalSearch
{
    public enum GraphPartitionNeighborhoodOption
    {
        OneSwap,
        CircularSwap,
        TwoSwaps
    }

    public static class GraphPartitionNeighborhoodOptionUtils
    {
        public static GraphPartitionNeighborhoodOption[] All = {
            GraphPartitionNeighborhoodOption.OneSwap,
            GraphPartitionNeighborhoodOption.CircularSwap,
            GraphPartitionNeighborhoodOption.TwoSwaps
        };

        public static string AsString(this GraphPartitionNeighborhoodOption option)
        {
            switch (option)
            {
                case GraphPartitionNeighborhoodOption.OneSwap:
                    return "One Swap";
                case GraphPartitionNeighborhoodOption.CircularSwap:
                    return "Circular Swap";
                case GraphPartitionNeighborhoodOption.TwoSwaps:
                    return "Two Swaps";
                default:
                    throw new ArgumentOutOfRangeException(nameof(option), option, null);
            }
        }
    }
}
