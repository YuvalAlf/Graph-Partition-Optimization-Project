using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Graphs.GraphProperties;
using MoreLinq;
using Optimizations.BranchAndBoundAlgorithm;
using Utils;
using Utils.DataStructures;
using Utils.ExtensionMethods;

namespace Graphs.Algorithms.BranchAndBound
{
    public sealed class PartialGraphPartition : IPartialSolution<PartialGraphPartition, UpperBoundScheme, GraphPartitionSolution>
    {
        public double Price => PriceOfPartition.Values.Sum();
        public double AveragePrice => PriceOfPartition.Keys.Average(part => PriceOfPartition[part] / GeneralUtils.Choose(Partitions[part].Count, 2));

        public double MinBound { get; }

        public Graph Graph { get; }

        public ImmutableStack<Node> NodesRemaining { get; }
        public ImmutableDictionary<PartitionType, double> PriceOfPartition { get; }
        public ImmutableDictionary<PartitionType, ImmutableHashSet<Node>> Partitions { get; }

        public PartialGraphPartition(double minBound, Graph graph, ImmutableStack<Node> nodesRemaining, ImmutableDictionary<PartitionType, double> priceOfPartition, ImmutableDictionary<PartitionType, ImmutableHashSet<Node>> partitions)
        {
            MinBound = minBound;
            Graph = graph;
            NodesRemaining = nodesRemaining;
            PriceOfPartition = priceOfPartition;
            Partitions = partitions;
        }

        public int CompareTo(PartialGraphPartition other) => this.MinBound.CompareTo(other.MinBound);

        public static PartialGraphPartition CreateEmpty(Graph graph, Random rnd)
        {
            var nodesStack = graph.Nodes.Shuffle(rnd).Aggregate(ImmutableStack<Node>.Empty, (stack, node) => stack.Push(node));

            var partitionsPrices = ImmutableDictionary<PartitionType, double>.Empty;
            var partitions = ImmutableDictionary<PartitionType, ImmutableHashSet<Node>>.Empty;
            foreach (var partitionType in PartitionTypeUtils.All)
            {
                partitionsPrices = partitionsPrices.Add(partitionType, 0.0);
                partitions = partitions.Add(partitionType, ImmutableHashSet<Node>.Empty);
            }

            var half = graph.Nodes.Length / 2;
            var minBoundPartition = MinPrice(ImmutableHashSet<Node>.Empty, graph, ImmutableHashSet<Node>.Empty, half);
            var minBound = minBoundPartition * 2;

            return new PartialGraphPartition(minBound, graph, nodesStack, partitionsPrices, partitions);
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
                partialSolution = partialSolution.Children().MinBy(c => c.AveragePrice);
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
                        
                        var nodePrice = Partitions[partitionType].Sum(adjacent => Graph.Weights[nextNode][adjacent]);
                        var newPriceOfPartition = PriceOfPartition.SetItem(partitionType, PriceOfPartition[partitionType] + nodePrice);
                        var half = Graph.Nodes.Length / 2;
                        var set1 = MinPrice(newPartitions[PartitionType.Partition1], Graph, newPartitions[PartitionType.Partition2], half);
                        var set2 = MinPrice(newPartitions[PartitionType.Partition2], Graph, newPartitions[PartitionType.Partition1], half);
                        var newMinBound = set1 + set2;
                        yield return new PartialGraphPartition(newMinBound, Graph, nodesRemaining, newPriceOfPartition, newPartitions);
                    }
            }
        }

        private static double MinPrice(ImmutableHashSet<Node> nodesIn, Graph graph,
            ImmutableHashSet<Node> prohibitedNodes, int setSize)
        {
            ImmutableDictionary<Node, double> priceOfNode = ImmutableDictionary<Node, double>.Empty;
            var usedNodes = nodesIn.Union(prohibitedNodes);
            foreach (var node in nodesIn)
            {
                var price = 0.0;
                foreach (var adjacentNode in nodesIn)
                {
                    if (!node.Equals(adjacentNode))
                        price += graph.Weights[node][adjacentNode];
                }

                price += graph.Weights[node].RemoveKeys(usedNodes).Values.PartialSort(setSize - nodesIn.Count).Sum();

                priceOfNode = priceOfNode.Add(node, price);
            }
            foreach (var graphNode in graph.Nodes)
                if (!usedNodes.Contains(graphNode))
                {
                    var price = 0.0;
                    foreach (var adjacentNode in nodesIn)
                        price += graph.Weights[graphNode][adjacentNode];
                    if (setSize > nodesIn.Count)
                        price += graph.Weights[graphNode].RemoveKeys(usedNodes).Values.PartialSort(setSize - nodesIn.Count - 1).Sum();

                    priceOfNode = priceOfNode.Add(graphNode, price);
                }

            double inPrice = nodesIn.Sum(n => priceOfNode[n]) / 2.0;
            double restPrice = priceOfNode.RemoveRange(nodesIn).Values.PartialSort(setSize - nodesIn.Count).Sum() / 2.0;
            return inPrice + restPrice;
        }
    }
}
