using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Graphs.GraphProperties;
using MoreLinq;
using Optimizations.BranchAndBoundAlgorithm;
using Utils.ExtensionMethods;

namespace Graphs.Algorithms.BranchAndBound
{
    public sealed class PartialGraphPartition : IPartialSolution<PartialGraphPartition, UpperBoundScheme, GraphPartitionSolution>
    {
        public double MinBound { get; }
        public double Price { get; }
        public Graph Graph { get; }
        public ImmutableStack<Node> NodesRemaining { get; }
        public Dictionary<Node, HashSet<(Node, double)>> Neighbors { get; }
        public Dictionary<Node, double> MinBoundOfNode { get; }
        public ImmutableDictionary<PartitionType, ImmutableHashSet<Node>> Partitions { get; }

        public PartialGraphPartition(
            Graph graph,
            ImmutableStack<Node> nodesRemaining,
            ImmutableDictionary<PartitionType, ImmutableHashSet<Node>> partitions,
            Dictionary<Node, HashSet<(Node, double)>> neighbors,
            Dictionary<Node, double> minBoundOfNode,
            double minBound,
            double price)
        {
            Graph = graph;
            NodesRemaining = nodesRemaining;
            Partitions = partitions;
            Neighbors = neighbors;
            MinBound = minBound;
            Price = price;
            MinBoundOfNode = minBoundOfNode;
        }

        public int CompareTo(PartialGraphPartition other) => this.MinBound.CompareTo(other.MinBound);

        public static PartialGraphPartition CreateEmpty(Graph graph, Random rnd)
        {
            var nodesStack = graph.Nodes.Shuffle(rnd).Aggregate(ImmutableStack<Node>.Empty, (stack, node) => stack.Push(node));
           

            var partitions = ImmutableDictionary<PartitionType, ImmutableHashSet<Node>>.Empty;
            foreach (var partitionType in PartitionTypeUtils.All)
                partitions = partitions.Add(partitionType, ImmutableHashSet<Node>.Empty);
            
            var neighbors = new Dictionary<Node, HashSet<(Node, double)>>(graph.Nodes.Length);
            foreach (var node in graph.Nodes)
                neighbors[node] = new HashSet<(Node, double)>();
            foreach (var edge in graph.Edges)
            {
                neighbors[edge.Node1].Add((edge.Node2, edge.Weight));
                neighbors[edge.Node2].Add((edge.Node1, edge.Weight));
            }

            var minBoundOfNode = new Dictionary<Node, double>(graph.Nodes.Length);
            var quarter = graph.Nodes.Length / 4;
            foreach (var node in graph.Nodes)
                minBoundOfNode[node] = neighbors[node].Select(tuple => tuple.Item2).OrderBy(x => x).Take(quarter).Sum() / 2.0;

            var minBound = minBoundOfNode.Values.Sum();

            return new PartialGraphPartition(graph, nodesStack, partitions, neighbors, minBoundOfNode, minBound, 0.0);
        }

        public GraphPartitionSolution ConstructSolution(UpperBoundScheme upperBoundScheme, Random rnd)
        {
            switch (upperBoundScheme)
            {
                case UpperBoundScheme.RandomizedSolution:
                    return ConstructRandomizedSolution(rnd);
                case UpperBoundScheme.GreedySolution:
                    return ConstructGreedySolution();
                default:
                    throw new ArgumentOutOfRangeException(nameof(upperBoundScheme), upperBoundScheme, null);
            }
        }

        private GraphPartitionSolution ConstructGreedySolution()
        {
            var partialSolution = this;
            while (true)
            {
                if (partialSolution.NodesRemaining.IsEmpty)
                    return new GraphPartitionSolution(partialSolution.Partitions, Graph);
                partialSolution = partialSolution.Children().MinBy(c => c.Price);
            }
        }

        private GraphPartitionSolution ConstructRandomizedSolution(Random rnd)
        {
            var partialSolution = this;
            while (true)
            {
                var children = partialSolution.Children().ToArray();
                if (children.Length == 0)
                    return new GraphPartitionSolution(partialSolution.Partitions, Graph);
                partialSolution = children.ChooseRandomly(rnd);
            }
        }

        public IEnumerable<PartialGraphPartition> Children()
        {
            if (!NodesRemaining.IsEmpty)
            {
                var nodesRemaining = NodesRemaining.Pop(out Node nextNode);
                foreach (var partitionType in PartitionTypeUtils.All)
                    if (this.Partitions[partitionType].Count < partitionType.Size(Graph))
                    {
                        var newPartitions = Partitions.SetItem(partitionType, Partitions[partitionType].Add(nextNode));
                        
                        var nodePrice = 0.0;
                        foreach (var (neighbor, weight) in Neighbors[nextNode])
                            if (newPartitions[partitionType].Contains(neighbor))
                                nodePrice += weight;

                        var newPrice = this.Price + nodePrice;
                        var newMinBound = this.MinBound + nodePrice - MinBoundOfNode[nextNode];
                        yield return new PartialGraphPartition(Graph, nodesRemaining, newPartitions, Neighbors, MinBoundOfNode, newMinBound, newPrice);
                    }
            }
        }
    }
}
