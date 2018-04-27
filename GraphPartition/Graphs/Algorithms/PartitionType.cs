using System;
using Graphs.GraphProperties;

namespace Graphs.Algorithms
{
    public enum PartitionType
    {
        BigPartition,
        SmallPartition1,
        SmallPartition2
    }

    public static class PartitionTypeUtils
    {
        public static PartitionType[] All { get; } = {
            PartitionType.BigPartition,
            PartitionType.SmallPartition1,
            PartitionType.SmallPartition2
        };

        public static int Size(this PartitionType @this, Graph graph)
        {
            switch (@this)
            {
                case PartitionType.BigPartition:
                    return graph.Nodes.Length / 2;
                case PartitionType.SmallPartition1:
                    return graph.Nodes.Length / 4;
                case PartitionType.SmallPartition2:
                    return graph.Nodes.Length / 4;
                default:
                    throw new ArgumentOutOfRangeException(nameof(@this), @this, null);
            }
        }
        public static string AsString(this PartitionType @this)
        {
            switch (@this)
            {
                case PartitionType.BigPartition:
                    return "Big_Partition N/2";
                case PartitionType.SmallPartition1:
                    return "Small_Partition_1 N/4";
                case PartitionType.SmallPartition2:
                    return "Small_Partition_2, N/4";
                default:
                    throw new ArgumentOutOfRangeException(nameof(@this), @this, null);
            }
        }
    }
}
