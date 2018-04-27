using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using Graphs.Algorithms.Genetic;
using Graphs.Algorithms.LocalSearch;
using Graphs.GraphProperties;
using MoreLinq;
using Optimizations.GeneticAlgorithm;
using Utils.ExtensionMethods;

namespace Graphs.Algorithms
{
    public sealed partial class GraphPartitionSolution : IGeneticSolver<GraphPartitionSolution, MatingScheme, GraphPartitionNeighborhoodOption, SelectionScheme>
    {
        public GraphPartitionSolution Mate(MatingScheme mating, GraphPartitionSolution otherSolution, Random rnd)
        {
            switch (mating)
            {
                case MatingScheme.IntersectionMating:
                    return IntersectionMating(this, otherSolution, rnd);
                case MatingScheme.CopyOnePartition:
                    return CopyOnePartitionMating(this, otherSolution, rnd);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static GraphPartitionSolution IntersectionMating(GraphPartitionSolution mom, GraphPartitionSolution dad, Random rnd)
        {
            var graph = mom.Graph;
            var newPartitions = ImmutableDictionary<PartitionType, ImmutableHashSet<Node>>.Empty;
            var remainingNodes = new HashSet<Node>(graph.Nodes);
            foreach (var partition in PartitionTypeUtils.All)
            {
                var intersection = mom.Partitions[partition].Intersect(dad.Partitions[partition]);
                intersection.ForEach(node => remainingNodes.Remove(node));
                newPartitions = newPartitions.Add(partition, intersection);
            }

            newPartitions = FillWithRemaining(newPartitions, graph, remainingNodes, rnd);
            
            return new GraphPartitionSolution(newPartitions, graph);
        }
        
        private static GraphPartitionSolution CopyOnePartitionMating(GraphPartitionSolution mom, GraphPartitionSolution dad, Random rnd)
        {
            var graph = mom.Graph;
            var copiedPartition = PartitionTypeUtils.All.ChooseRandomly(rnd);
            var newPartitions = ImmutableDictionary<PartitionType, ImmutableHashSet<Node>>.Empty;
            var remainingNodes = new HashSet<Node>(graph.Nodes);
            newPartitions = newPartitions.Add(copiedPartition, mom.Partitions[copiedPartition]);
            mom.Partitions[copiedPartition].ForEach(node => remainingNodes.Remove(node));
            foreach (var partition in PartitionTypeUtils.All.Where(p => p != copiedPartition))
            {
                var intersection = dad.Partitions[partition].Intersect(remainingNodes);
                newPartitions = newPartitions.Add(partition, intersection);
                intersection.ForEach(node => remainingNodes.Remove(node));
            }

            newPartitions = FillWithRemaining(newPartitions, graph, remainingNodes, rnd);

            return new GraphPartitionSolution(newPartitions, graph);
        }

        private static ImmutableDictionary<PartitionType, ImmutableHashSet<Node>> FillWithRemaining(ImmutableDictionary<PartitionType, ImmutableHashSet<Node>> partitions, Graph graph, IEnumerable<Node> remainingNodes, Random rnd)
        {
            var nodes = new Stack<Node>(remainingNodes.Shuffle(rnd));
            while (nodes.Count > 0)
                foreach (var partitionType in PartitionTypeUtils.All)
                    while (partitions[partitionType].Count < partitionType.Size(graph))
                        partitions = partitions.SetItem(partitionType, partitions[partitionType].Add(nodes.Pop()));

            return partitions;
        }

        public GraphPartitionSolution Mutate(GraphPartitionNeighborhoodOption mutation, Random rnd)
        {
            return this.Neighbors(rnd, mutation).First();
        }

        public Func<Random, (GraphPartitionSolution, GraphPartitionSolution)> SelectionMethod(SelectionScheme selection, GraphPartitionSolution[] population)
        {
            var mapFunc = FitnessFunc(selection);
            var proportions = population.Select(s => mapFunc(s.NegativePrice)).ToArray();
            var average = proportions.Average();
            var probabilities = proportions.Select(p => p / average).ToArray();
            Array.Sort(proportions);

            int ChooseSolutionIndex(Random rnd, int exceptIndex)
            {
                var sumOfLeft = 1.0;
                for (int i = 0; i < population.Length - 1; i++)
                {
                    if (i == exceptIndex)
                        continue;
                    
                    var probability = probabilities[i] / sumOfLeft;
                    if (rnd.NextDouble() < probability)
                        return i;
                    sumOfLeft -= probabilities[i];
                }

                return population.Length - 1;
            }


            return rnd =>
            {
                var momIndex = ChooseSolutionIndex(rnd, int.MinValue);
                var dadIndex = ChooseSolutionIndex(rnd, momIndex);
                return (population[momIndex], population[dadIndex]);
            };
        }

        private Func<double, double> FitnessFunc(SelectionScheme selection)
        {
            switch (selection)
            {
                case SelectionScheme.Equal:
                    return d => 1.0;
                case SelectionScheme.Inversed:
                    return d => 1.0 / Math.Pow(1.0 + d, 1.0);
                case SelectionScheme.InversedSquarred:
                    return d => 1.0 / Math.Pow(1.0 + d, 2.0);
                case SelectionScheme.InversedCubed:
                    return d => 1.0 / Math.Pow(1.0 + d, 3.0);
                case SelectionScheme.InversedLog:
                    return d => 1.0 / Math.Pow(Math.Log(1.0 + d), 1.0);
                case SelectionScheme.InversedLogSquarred:
                    return d => 1.0 / Math.Pow(Math.Log(1.0 + d), 2.0);
                case SelectionScheme.InversedLogCubed:
                    return d => 1.0 / Math.Pow(Math.Log(1.0 + d), 3.0);
                default:
                    throw new ArgumentOutOfRangeException(nameof(selection), selection, null);
            }
        }
    }
}
