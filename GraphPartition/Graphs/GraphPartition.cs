using System;
using System.Collections.Generic;

namespace Graphs
{
    public sealed class GraphPartition
    {
        public GraphPartition(Dictionary<PartitionType, HashSet<int>> partitions)
        {
            Partitions = partitions;
        }

        public Dictionary<PartitionType, HashSet<int>> Partitions { get; }

        public double Price(Graph graph)
        {
            var sumPrice = 0.0;
            foreach (var edge in graph.Edges)
                if (PartitionTypeOf(edge.Node1) == PartitionTypeOf(edge.Node2))
                    sumPrice += edge.Weight;
            return sumPrice;
        }

        private PartitionType PartitionTypeOf(int node)
        {
            foreach (var partitionType in Partitions.Keys)
                if (Partitions[partitionType].Contains(node))
                    return partitionType;
            throw new ArgumentException("Node " + node + " is not in any partition");
        }
    }
}
