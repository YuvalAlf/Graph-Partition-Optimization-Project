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

        private Graph(Node[] nodes, Edge[] edges)
        {
            Nodes = nodes;
            Edges = edges;
        }

        public static Graph Create(Node[] nodes, Edge[] edges) => new Graph(nodes, edges);

        public static Graph Create(int amountOfNodes, Func<int, int, double> weights)
        {
            var nodes = ArrayExtensions.InitArray(amountOfNodes, i => (Node) (i + 1));
            var edges = new Edge[nodes.Length * (nodes.Length - 1) / 2];
            int index = 0;
            for(int i = 0; i < nodes.Length - 1; i++)
                for(int j = i + 1; j < nodes.Length; j++)
                    edges[index++] = Edge.Create(nodes[i], nodes[j], weights(nodes[i].Value, nodes[j].Value));
            return new Graph(nodes, edges);
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
