using System;
using System.Collections.Generic;
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
        public static PartitionType[] All => all;

        private static PartitionType[] all = {
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
    }
}
