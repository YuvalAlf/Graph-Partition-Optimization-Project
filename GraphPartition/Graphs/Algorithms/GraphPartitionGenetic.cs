using System;
using System.Collections.Generic;
using System.Linq;
using Graphs.GraphProperties;
using Optimizations.GeneticAlgorithm;
using Utils.ExtensionMethods;

namespace Graphs.Algorithms
{
    public sealed partial class GraphPartitionSolution : IGeneticSolver<GraphPartitionSolution>
    {
        public GraphPartitionSolution MateMutate(GraphPartitionSolution otherSolution, double mutationRate, Random rnd)
        {
            var partitions = new Dictionary<PartitionType, HashSet<Node>>();
            foreach (var partitionType in PartitionTypeUtils.All)
                partitions[partitionType] = new HashSet<Node>();
            var chosenPartiton = PartitionTypeUtils.All.ChooseRandomly(rnd);
            partitions[chosenPartiton] = this.Partitions[chosenPartiton];
            var nodesRemaining = new LinkedList<Node>();
            foreach (var node in this.Graph.Nodes)
            {
                var nodePartition = otherSolution.PartitionTypeOf(node);
                if (!partitions[chosenPartiton].Contains(node))
                    if (nodePartition != chosenPartiton)
                        partitions[nodePartition].Add(node);
                    else
                        nodesRemaining.AddLast(node);
            }

            foreach (var partitionType in PartitionTypeUtils.All)
            {
                var desiredSize = partitionType.Size(Graph);
                while (partitions[partitionType].Count < desiredSize)
                {
                    partitions[partitionType].Add(nodesRemaining.First.Value);
                    nodesRemaining.RemoveFirst();
                }
            }

            return new GraphPartitionSolution(partitions, Graph);
        }

        public static Func<Random, GraphPartitionSolution> GenerateRandom(Graph graph) => rnd =>
        {
            var nodes = new List<Node>(graph.Nodes.OrderBy(_ => rnd.Next()));
            var partitions = new Dictionary<PartitionType, HashSet<Node>>();
            foreach (var partitionType in PartitionTypeUtils.All)
            {
                partitions[partitionType] = new HashSet<Node>(nodes.Take(partitionType.Size(graph)));
                nodes.RemoveRange(0, partitionType.Size(graph));
            }

            return new GraphPartitionSolution(partitions, graph);
        };
    }
}
