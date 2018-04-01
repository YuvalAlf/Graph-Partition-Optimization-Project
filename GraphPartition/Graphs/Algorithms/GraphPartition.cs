using System;
using System.Collections.Generic;
using Graphs.GraphProperties;

namespace Graphs.Algorithms
{
    public sealed class GraphPartition
    {
        public Dictionary<PartitionType, HashSet<Node>> Partitions { get; }

        public GraphPartition(Dictionary<PartitionType, HashSet<Node>> partitions)
        {
            Partitions = partitions;
        }


        public double Price(Graph graph)
        {
            var sumPrice = 0.0;
            foreach (var edge in graph.Edges)
                if (PartitionTypeOf(edge.Node1) == PartitionTypeOf(edge.Node2))
                    sumPrice += edge.Weight;
            return sumPrice;
        }

        private PartitionType PartitionTypeOf(Node node)
        {
            foreach (var partitionType in Partitions.Keys)
                if (Partitions[partitionType].Contains(node))
                    return partitionType;
            throw new ArgumentException("Node " + node + " is not in any partition");
        }
    }
}
