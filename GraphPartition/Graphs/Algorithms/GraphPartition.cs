using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Graphs.GraphProperties;
using Utils.DataStructures;
using Utils.ExtensionMethods;

namespace Graphs.Algorithms
{
    public sealed partial class GraphPartitionSolution
    {
        public double NegativePrice { get; }
        public ImmutableDictionary<PartitionType, ImmutableHashSet<Node>> Partitions { get; }
        public Graph Graph { get; }

        public GraphPartitionSolution(ImmutableDictionary<PartitionType, ImmutableHashSet<Node>> partitions, Graph graph)
        {
            Partitions = partitions;
            Graph = graph;
            NegativePrice = CalcSumOfWeights();
        }

        public double CalcSumOfWeights()
        {
            var sumOfWeights = 0.0;
            foreach (var edge in Graph.Edges)
                if (PartitionTypeOf(edge.Node1, Partitions) == PartitionTypeOf(edge.Node2, Partitions))
                    sumOfWeights += edge.Weight;
            return sumOfWeights;
        }

        public static Func<Random, GraphPartitionSolution> GenerateRandom(Graph graph) => rnd =>
        {
            var nodes = new Stack<Node>(graph.Nodes.Shuffle(rnd));
            var partitions = ImmutableDictionary<PartitionType, ImmutableHashSet<Node>>.Empty;

            foreach (var partitionType in PartitionTypeUtils.All)
                partitions = partitions.Add(partitionType, ImmutableHashSet<Node>.Empty.AddRange(nodes.MultiPop(partitionType.Size(graph))));


            return new GraphPartitionSolution(partitions, graph);
        };
        
        public PartitionType PartitionTypeOf(Node node) => PartitionTypeOf(node, Partitions);

        public static PartitionType PartitionTypeOf(Node node, ImmutableDictionary<PartitionType, ImmutableHashSet<Node>> partitions)
        {
            foreach (var partitionType in partitions.Keys)
                if (partitions[partitionType].Contains(node))
                    return partitionType;
            throw new ArgumentException("Node " + node + " is not in any partition");
        }

        public void WriteToFile(string path)
        {
            using (var file = File.CreateText(path))
            {
                var partitions = Partitions.Keys.OrderByDescending(k => Partitions[k].Count).ToArray();
                foreach (var partitionType in partitions)
                    file.Write(partitionType.AsString().PadLeft(20).PadRight(30));
                file.WriteLine();

                var maxSize = Partitions.Values.Max(p => p.Count);
                for (int i = 0; i < maxSize; i++)
                {
                    foreach (var partition in partitions)
                    {
                        if (i < Partitions[partition].Count)
                          file.Write(Partitions[partition].ElementAt(i).Value.ToString().PadLeft(10).PadRight(30));
                    }
                    file.WriteLine();
                }
            }
        }
    }
}
