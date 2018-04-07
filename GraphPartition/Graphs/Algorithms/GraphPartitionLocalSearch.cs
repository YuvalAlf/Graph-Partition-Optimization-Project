﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphs.GraphProperties;
using Optimizations.LocalSearchAlgorithm;

namespace Graphs.Algorithms
{
    public sealed partial class GraphPartitionSolution : ILocalSearch<GraphPartitionSolution>
    {
        public IEnumerable<GraphPartitionSolution> Neighbors()
        {
            for (int i = 0; i < this.Graph.Nodes.Length - 1; i++)
            for (int j = i + 1; j < this.Graph.Nodes.Length; j++)
            {
                var node1 = this.Graph.Nodes[i];
                var node2 = this.Graph.Nodes[j];
                var partitionType1 = PartitionTypeOf(node1);
                var partitionType2 = PartitionTypeOf(node2);
                if (partitionType1 != partitionType2)
                    yield return this.ReplacePartitionTypeOf(node1, node2);
            }

            foreach (var node1 in this.Graph.Nodes)
            foreach (var node2 in this.Graph.Nodes)
            foreach (var node3 in this.Graph.Nodes)
                yield return this.ReplacePartitionTypeOf(node1, node2).ReplacePartitionTypeOf(node2, node3);
        }

        private GraphPartitionSolution ReplacePartitionTypeOf(Node node1, Node node2)
        {
            var partitionType1 = PartitionTypeOf(node1);
            var partitionType2 = PartitionTypeOf(node2);

            var newDict = new Dictionary<PartitionType, HashSet<Node>>(this.Partitions.Count);
            foreach (var partition in Partitions.Keys)
                newDict[partition] = new HashSet<Node>(this.Partitions[partition]);
            
            newDict[partitionType1].Remove(node1);
            newDict[partitionType2].Remove(node2);
            newDict[partitionType2].Add(node1);
            newDict[partitionType1].Add(node2);
            return new GraphPartitionSolution(newDict, Graph);
        }
    }
}