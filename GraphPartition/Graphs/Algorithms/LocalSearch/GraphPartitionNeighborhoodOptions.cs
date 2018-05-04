using System;
using ArgumentOutOfRangeException = System.ArgumentOutOfRangeException;

namespace Graphs.Algorithms.LocalSearch
{
    public enum GraphPartitionNeighborhoodOption
    {
        OneSwap,
        CircularSwap,
        TwoSwaps,
        AllTogether
    }

    public static class GraphPartitionNeighborhoodOptionUtils
    {
        public static GraphPartitionNeighborhoodOption[] All { get; } = {
            GraphPartitionNeighborhoodOption.OneSwap,
            GraphPartitionNeighborhoodOption.CircularSwap,
            GraphPartitionNeighborhoodOption.TwoSwaps,
            GraphPartitionNeighborhoodOption.AllTogether
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
                case GraphPartitionNeighborhoodOption.AllTogether:
                    return "All together";
                default:
                    throw new ArgumentOutOfRangeException(nameof(option), option, null);
            }
        }
        public static int Amount(this GraphPartitionNeighborhoodOption option, int amountOfNodes)
        {
            var amountOfPairs = amountOfNodes * (amountOfNodes - 1) / 2;
            var amountOfTriplets = amountOfNodes * (amountOfNodes - 1) * (amountOfNodes - 2) / 6;
            var amountOfQuads = amountOfNodes * (amountOfNodes - 1) * (amountOfNodes - 2) * (amountOfNodes - 3) / 24;
            switch (option)
            {
                case GraphPartitionNeighborhoodOption.OneSwap:
                    return 5*(int)(amountOfPairs * Math.Log(amountOfPairs));
                case GraphPartitionNeighborhoodOption.CircularSwap:
                    return (int)(amountOfTriplets * Math.Log(amountOfTriplets));
                case GraphPartitionNeighborhoodOption.TwoSwaps:
                    return (int)(amountOfQuads * Math.Log(amountOfQuads));
                case GraphPartitionNeighborhoodOption.AllTogether:
                    return GraphPartitionNeighborhoodOption.TwoSwaps.Amount(amountOfNodes);
                default:
                    throw new ArgumentOutOfRangeException(nameof(option), option, null);
            }
        }
    }
}
