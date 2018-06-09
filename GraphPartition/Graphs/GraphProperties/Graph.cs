using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils.ExtensionMethods;
using Utils.IO;

namespace Graphs.GraphProperties
{
    public sealed class Graph
    {
        public Node[] Nodes { get; }
        public Edge[] Edges { get; }
        public Dictionary<Node, Dictionary<Node, double>> Weights { get; }

        private Graph(Node[] nodes, Edge[] edges, Dictionary<Node, Dictionary<Node, double>> weights)
        {
            Nodes = nodes;
            Edges = edges;
            Weights = weights;
        }

        public static Graph Create(Node[] nodes, Edge[] edges)
        {
            var weights = new Dictionary<Node, Dictionary<Node, double>>(nodes.Length);
            foreach (var node in nodes)
                weights[node] = new Dictionary<Node, double>(nodes.Length);
            foreach (var edge in edges)
            {
                weights[edge.Node1][edge.Node2] = edge.Weight;
                weights[edge.Node2][edge.Node1] = edge.Weight;
            }
            return new Graph(nodes, edges, weights);
        }

        public static Graph Create(int amountOfNodes, Func<int, int, double> weights)
        {
            var nodes = ArrayExtensions.InitArray(amountOfNodes, i => (Node) (i + 1));
            var edges = new Edge[nodes.Length * (nodes.Length - 1) / 2];
            int index = 0;
            for(int i = 0; i < nodes.Length - 1; i++)
                for(int j = i + 1; j < nodes.Length; j++)
                    edges[index++] = Edge.Create(nodes[i], nodes[j], weights(nodes[i].Value, nodes[j].Value));
            return Create(nodes, edges);
        }


        public void WriteToFile(string filePath) => File.WriteAllLines(filePath, Edges.Select(e => e.ToString()));

        public static Graph ParseFromPath(string path)
        {
            path = path.EndsWith("Graph.txt") ? path : path.CombinePathWith("Graph.txt");
            var edges = File.ReadLines(path).Select(Edge.Parse).ToArray();
            var nodes = edges.SelectMany(e => new[] {e.Node1, e.Node2}).Distinct().ToArray();
            return Graph.Create(nodes, edges);
        }
        public static bool CanParseFromPath(string path)
        {
            try
            {
                ParseFromPath(path);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
