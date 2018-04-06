using System;
using System.Collections.Generic;
using Graphs.GraphProperties;

namespace Graphs.Algorithms
{
    public sealed partial class GraphPartitionSolution
    {
        public double NegativePrice { get; }
        public Dictionary<PartitionType, HashSet<Node>> Partitions { get; }
        public Graph Graph { get; }

        public GraphPartitionSolution(Dictionary<PartitionType, HashSet<Node>> partitions, Graph graph)
        {
            Partitions = partitions;
            Graph = graph;
            NegativePrice = Price(graph);
        }


        public double Price(Graph graph)
        {
            var sumPrice = 0.0;
            foreach (var edge in graph.Edges)
                if (PartitionTypeOf(edge.Node1) == PartitionTypeOf(edge.Node2))
                    sumPrice += edge.Weight;
            return sumPrice;
        }

        public PartitionType PartitionTypeOf(Node node)
        {
            foreach (var partitionType in Partitions.Keys)
                if (Partitions[partitionType].Contains(node))
                    return partitionType;
            throw new ArgumentException("Node " + node + " is not in any partition");
        }


        private GraphPartitionSolution Clone()
        {
            var newDict = new Dictionary<PartitionType, HashSet<Node>>(this.Partitions.Count);
            foreach (var partition in Partitions.Keys)
                newDict[partition] = new HashSet<Node>(this.Partitions[partition]);

            return new GraphPartitionSolution(newDict, Graph);

        }
    }
}
