using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphs.GraphProperties;
using Optimizations.BranchAndBoundAlgorithm;

namespace Graphs.Algorithms
{
    public sealed class PartialGraphPartition : IPartialSolution<PartialGraphPartition, GraphPartitionSolution>
    {
        public double MinBound { get; }
        public Graph Graph { get; }
        public ImmutableStack<Node> NodesRemaining { get; }
        public ImmutableDictionary<PartitionType, ImmutableHashSet<Node>> Partitions { get; }

        public PartialGraphPartition(Graph graph, ImmutableStack<Node> nodesRemaining, ImmutableDictionary<PartitionType, ImmutableHashSet<Node>> partitions)
        {
            Graph = graph;
            NodesRemaining = nodesRemaining;
            Partitions = partitions;
            MinBound = CalcMinBound();
        }


        public int CompareTo(PartialGraphPartition other) => this.MinBound.CompareTo(other.MinBound);

        public static PartialGraphPartition CreateEmpty(Graph graph, IEnumerable<Node> nodes)
        {
            var nodesStack = nodes.Aggregate(ImmutableStack<Node>.Empty, (stack, node) => stack.Push(node));
            var partitions = ImmutableDictionary<PartitionType, ImmutableHashSet<Node>>.Empty;
            foreach (var partitionType in PartitionTypeUtils.All)
                partitions = partitions.Add(partitionType, ImmutableHashSet<Node>.Empty);
            return new PartialGraphPartition(graph, nodesStack, partitions);
        }


        private double CalcMinBound()
        {
            throw new NotImplementedException();
        }

        public GraphPartitionSolution ConstructSolution(Random rnd)
        {
            throw new NotImplementedException();
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
                        yield return new PartialGraphPartition(Graph, nodesRemaining, newPartitions);
                    }
            }
        }
    }
}
