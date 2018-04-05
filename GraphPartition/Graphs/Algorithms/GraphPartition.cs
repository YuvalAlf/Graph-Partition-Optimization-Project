using System;
using System.Collections.Generic;
using System.Linq;
using Graphs.GraphProperties;
using Optimizations.BranchAndBound;
using Optimizations.GeneticAlgorithm;
using Optimizations.LocalSearch;
using Utils.ExtensionMethods;

namespace Graphs.Algorithms
{
    public sealed class GraphPartitionSolution : IGeneticSolution<GraphPartitionSolution>
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

        public static IEnumerable<GraphPartitionSolution> RunGenetic(GeneticSettings geneticSettings, Random rnd, Graph graph, Func<bool> kill, object run)
        {
            var genetic = new Genetic<GraphPartitionSolution>(r => GraphPartitionSolution.GenerateRandom(graph, r));
            return genetic.Run(geneticSettings, run, kill, rnd);

        }

        public static GraphPartitionSolution GenerateRandom(Graph graph, Random rnd)
        {
            var nodes = new List<Node>(graph.Nodes.OrderBy(_ => rnd.Next()));
            var partitions = new Dictionary<PartitionType, HashSet<Node>>();
            foreach (var partitionType in PartitionTypeUtils.All)
            {
                partitions[partitionType] = new HashSet<Node>(nodes.Take(partitionType.Size(graph)));
                nodes.RemoveRange(0, partitionType.Size(graph));
            }

            return new GraphPartitionSolution(partitions, graph);

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
