using System;
using System.Collections.Generic;
using Graphs.GraphProperties;
using Optimizations.BranchAndBound;
using Optimizations.GeneticAlgorithm;
using Optimizations.LocalSearch;

namespace Graphs.Algorithms
{
    public sealed class GraphPartitionSolution
    {
        public Dictionary<PartitionType, HashSet<Node>> Partitions { get; }

        public GraphPartitionSolution(Dictionary<PartitionType, HashSet<Node>> partitions)
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

        public static IEnumerable<GraphPartitionSolution> RunGenetic(GeneticSettings geneticSettings, Graph graph)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<GraphPartitionSolution> RunBranchAndBound(BranchAndBoundSettings branchAndBoundSettings, Graph graph)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<GraphPartitionSolution> RunLocalSearch(LocalSearchSettings localSearchSettings, Graph graph)
        {
            throw new NotImplementedException();
        }
    }
}
