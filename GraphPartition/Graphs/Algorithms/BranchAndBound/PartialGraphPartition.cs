using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Graphs.GraphProperties;
using Optimizations.BranchAndBoundAlgorithm;
using Utils.ExtensionMethods;

namespace Graphs.Algorithms.BranchAndBound
{
    public sealed class PartialGraphPartition : IPartialSolution<PartialGraphPartition, UpperBoundScheme, GraphPartitionSolution>
    {
        public double MinBound { get; }
        public Graph Graph { get; }
        public ImmutableStack<Node> NodesRemaining { get; }
        public Dictionary<Node, HashSet<(Node, double)>> Neighbors { get; }
        public ImmutableDictionary<PartitionType, ImmutableHashSet<Node>> Partitions { get; }

        public PartialGraphPartition(Graph graph, ImmutableStack<Node> nodesRemaining, ImmutableDictionary<PartitionType, ImmutableHashSet<Node>> partitions, Dictionary<Node, HashSet<(Node, double)>> neighbors, double minBound)
        {
            Graph = graph;
            NodesRemaining = nodesRemaining;
            Partitions = partitions;
            Neighbors = neighbors;
            MinBound = minBound;
        }

        public int CompareTo(PartialGraphPartition other) => this.MinBound.CompareTo(other.MinBound);

        public static PartialGraphPartition CreateEmpty(Graph graph)
        {
            var nodesStack = graph.Nodes.Aggregate(ImmutableStack<Node>.Empty, (stack, node) => stack.Push(node));
            var partitions = ImmutableDictionary<PartitionType, ImmutableHashSet<Node>>.Empty;
            foreach (var partitionType in PartitionTypeUtils.All)
                partitions = partitions.Add(partitionType, ImmutableHashSet<Node>.Empty);
            
            var neighbors = new Dictionary<Node, HashSet<(Node, double)>>(graph.Nodes.Length);
            foreach (var node in graph.Nodes)
            {
                neighbors[node] = new HashSet<(Node, double)>();
                foreach (var edge in graph.Edges)
                {
                    if (edge.Node1.Equals(node))
                        neighbors[node].Add((edge.Node2, edge.Weight));
                    if (edge.Node2.Equals(node))
                        neighbors[node].Add((edge.Node1, edge.Weight));
                }
            }

            return new PartialGraphPartition(graph, nodesStack, partitions, neighbors, 0.0);
        }

        public GraphPartitionSolution ConstructSolution(UpperBoundScheme upperBoundScheme, Random rnd)
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
                var nodesRemaining = NodesRemaining.Pop(out var nextNode);
                foreach (var partitionType in PartitionTypeUtils.All)
                    if (this.Partitions[partitionType].Count < partitionType.Size(Graph))
                    {
                        var newPartitions = Partitions.SetItem(partitionType, Partitions[partitionType].Add(nextNode));
                        var newPrice = this.MinBound;
                        foreach (var (neighbor, weight) in Neighbors[nextNode])
                            if (Partitions[partitionType].Contains(neighbor))
                                newPrice += weight;
                        yield return new PartialGraphPartition(Graph, nodesRemaining, newPartitions, Neighbors, newPrice);
                    }
            }
        }
    }
}
