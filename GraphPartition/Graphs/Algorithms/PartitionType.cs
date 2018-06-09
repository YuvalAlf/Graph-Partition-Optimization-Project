using System;
using Graphs.GraphProperties;

namespace Graphs.Algorithms
{
    public enum PartitionType
    {
        Partition1,
        Partition2
    }

    public static class PartitionTypeUtils
    {
        public static PartitionType[] All { get; } = {
            PartitionType.Partition1,
            PartitionType.Partition2
        };

        public static int Size(this PartitionType @this, Graph graph)
        {
            return graph.Nodes.Length / 2;
        }
        public static string AsString(this PartitionType @this)
        {
            switch (@this)
            {
                case PartitionType.Partition1:
                    return "Partition1";
                case PartitionType.Partition2:
                    return "Partition2";
                default:
                    throw new ArgumentOutOfRangeException(nameof(@this), @this, null);
            }
        }
    }
}
