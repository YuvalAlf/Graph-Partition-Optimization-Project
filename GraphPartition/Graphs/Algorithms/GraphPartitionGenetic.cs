using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Graphs.GraphProperties;
using MoreLinq;
using Optimizations.GeneticAlgorithm;
using Utils.ExtensionMethods;

namespace Graphs.Algorithms
{
    public sealed partial class GraphPartitionSolution : IGeneticSolver<GraphPartitionSolution>
    {
        public GraphPartitionSolution Mate(GraphPartitionSolution otherSolution, Random rnd)
        {
            var newPartitions = ImmutableDictionary<PartitionType, ImmutableHashSet<Node>>.Empty;
            var remainingNodes = new HashSet<Node>(Graph.Nodes);
            foreach (var partition in PartitionTypeUtils.All)
            {
                var intersection = this.Partitions[partition].Intersect(otherSolution.Partitions[partition]);
                intersection.ForEach(node => remainingNodes.Remove(node));
                newPartitions = newPartitions.Add(partition, intersection);
            }

            var remainingNodesStack = new Stack<Node>(remainingNodes.Shuffle(rnd));

            while (remainingNodesStack.Count > 0)
            foreach (var partitionType in PartitionTypeUtils.All)
                if (newPartitions[partitionType].Count < partitionType.Size(Graph))
                    newPartitions = newPartitions.SetItem(partitionType, newPartitions[partitionType].Add(remainingNodesStack.Pop()));

            return new GraphPartitionSolution(newPartitions, Graph);
        }

        public GraphPartitionSolution Mutate(Random rnd)
        {
            var node1 = Graph.Nodes.ChooseRandomly(rnd);
            var node2 = Graph.Nodes.ChooseRandomly(rnd);

            return this.ReplacePartitionTypeOf(node1, node2);
        }
    }
}
