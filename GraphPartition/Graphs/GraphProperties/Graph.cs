using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using Utils;
using Utils.ExtensionMethods;

namespace Graphs.GraphProperties
{
    public sealed class Graph
    {
        public Node[] Nodes { get; }
        public Edge[] Edges { get; }

        private Graph(Node[] nodes, Edge[] edges)
        {
            Nodes = nodes;
            Edges = edges;
        }

        public static Graph Create(int amountOfNodes, Func<int, int, double> weights)
        {
            var nodes = Enumerable.Range(1, amountOfNodes).ToArray();
            var edges = new List<Edge>(nodes.Length * nodes.Length / 2);
            for(int i = 0; i < nodes.Length - 1; i++)
            for(int j = i + 1; j < nodes.Length; j++)
                edges.Add(Edge.Create(nodes[i], nodes[j], weights(nodes[i], nodes[j])));
            return new Graph(nodes.Select(n => (Node)n).ToArray(), edges.ToArray());
        }

        public static Graph Create(Node[] nodes, Edge[] edges) => new Graph(nodes, edges);

        public void WriteToFile(string filePath)
        {
            File.WriteAllLines(filePath, Edges.Select(e => e.ToString()));
        }

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
