using System;
using System.Collections.Generic;
using System.Linq;
using Graphs.Algorithms.LocalSearch;
using Graphs.GraphProperties;
using Optimizations.LocalSearchAlgorithm;
using Utils.ExtensionMethods;
using static Graphs.Algorithms.PartitionType;

namespace Graphs.Algorithms
{
    public sealed partial class GraphPartitionSolution : ILocalSearchSolver<GraphPartitionSolution, GraphPartitionNeighborhoodOption>
    {
        public IEnumerable<GraphPartitionSolution> Neighbors(Random rnd, GraphPartitionNeighborhoodOption solution)
        {
            int count = 0;
            int size = solution.Amount(Graph.Nodes.Length);
            switch (solution)
            {
                case GraphPartitionNeighborhoodOption.OneSwap:
                    while (count++ < size)
                        yield return GenOneSwap(rnd);
                    break;
                case GraphPartitionNeighborhoodOption.CircularSwap:
                    while (count++ < size)
                        yield return GenCircularSwap(rnd);
                    break;
                case GraphPartitionNeighborhoodOption.TwoSwaps:
                    while (count++ < size)
                        yield return GenTwoSwap(rnd);
                    break;
                case GraphPartitionNeighborhoodOption.AllTogether:
                    while (count++ < size)
                    {
                        yield return GenOneSwap(rnd);
                        yield return GenTwoSwap(rnd);
                        yield return GenCircularSwap(rnd);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(solution), solution, null);
            }
        }

        private GraphPartitionSolution GenOneSwap(Random rnd)
        {
            var node1 = this.Graph.Nodes.ChooseRandomly(rnd);
            var node2 = this.Graph.Nodes.ChooseRandomly(rnd);
            return this.ReplacePartitionTypeOf(node1, node2);
        }

        private GraphPartitionSolution GenTwoSwap(Random rnd)
        {
            return this.GenOneSwap(rnd).GenOneSwap(rnd);
        }

        private GraphPartitionSolution GenCircularSwap(Random rnd)
        {
            var node1 = Partitions[BigPartition].ElementAt(rnd.Next(BigPartition.Size(Graph)));
            var node2 = Partitions[SmallPartition1].ElementAt(rnd.Next(SmallPartition1.Size(Graph)));
            var node3 = Partitions[SmallPartition2].ElementAt(rnd.Next(SmallPartition2.Size(Graph)));
            return ReplacePartitionTypeOf(node2, node3).ReplacePartitionTypeOf(node1, node3);
        }
        
        private GraphPartitionSolution ReplacePartitionTypeOf(Node node1, Node node2)
        {
            var partitionType1 = PartitionTypeOf(node1);
            var partitionType2 = PartitionTypeOf(node2);
            if (partitionType1 == partitionType2)
                return this;
            var newPartitions = this.Partitions;
            newPartitions = newPartitions.SetItem(partitionType1, newPartitions[partitionType1].Remove(node1).Add(node2));
            newPartitions = newPartitions.SetItem(partitionType2, newPartitions[partitionType2].Remove(node2).Add(node1));
            return new GraphPartitionSolution(newPartitions, Graph);
        }
    }
}
