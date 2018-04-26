using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Graphs.Algorithms.Genetic;
using Graphs.Algorithms.LocalSearch;
using Graphs.GraphProperties;
using MoreLinq;
using Optimizations.GeneticAlgorithm;
using Utils.ExtensionMethods;

namespace Graphs.Algorithms
{
    public sealed partial class GraphPartitionSolution : IGeneticSolver<GraphPartitionSolution, Mating, GraphPartitionNeighborhoodOption>
    {
        public GraphPartitionSolution Mate(Mating mating, GraphPartitionSolution otherSolution, Random rnd)
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

        public GraphPartitionSolution Mutate(GraphPartitionNeighborhoodOption mutation, Random rnd)
        {
            return this.Neighbors(rnd, mutation).First();
        }
    }
}
