using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Graphs.GraphProperties;

namespace Graphs.EmbeddingInPlane
{
    public sealed partial class GraphEmbedding
    {
        public void WriteTo(string filePath) => File.WriteAllLines(filePath, this.ToText());

        public IEnumerable<string> ToText()
        {
            foreach (var node in Embedding.Keys)
                yield return $"Node {node.Value}\t left: {Embedding[node].X}\t top: {Embedding[node].Y}";
            foreach (var edge in Graph.Edges)
                yield return $"Edge ({edge.Node1.Value},{edge.Node2.Value})\t weight {edge.Weight}";
        }

        public static GraphEmbedding Parse(string path) => FromText(File.ReadLines(path));

        public static bool CanParse(string path)
        {
            try
            {
                FromText(File.ReadLines(path));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static GraphEmbedding FromText(IEnumerable<string> lines)
        {
            var nodes = new Dictionary<Node, Point>();
            var edges = new List<Edge>();
            foreach (var line in lines)
            {
                if (line.StartsWith("Node"))
                    ParseNodeAndAdd(line, nodes);
                else if (line.StartsWith("Edge"))
                    ParseEdgeAndAdd(line, edges);
                else
                    throw new ArgumentException("Line malformed: " + line);
            }
            return new GraphEmbedding(Graph.Create(nodes.Keys.ToArray(), edges.ToArray()), nodes);
        }

        private static void ParseEdgeAndAdd(string line, List<Edge> edges)
        {
            var entries = line.Split(new[] { '\t', ' ', ',', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            var node1 = new Node(int.Parse(entries[1]));
            var node2 = new Node(int.Parse(entries[2]));
            var weight = double.Parse(entries[4]);
            edges.Add(Edge.Create(node1, node2, weight));
        }

        private static void ParseNodeAndAdd(string line, Dictionary<Node, Point> nodes)
        {
            var entries = line.Split(new[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var nodeValue = int.Parse(entries[1]);
            var left = double.Parse(entries[3]);
            var top = double.Parse(entries[5]);
            nodes[new Node(nodeValue)] = new Point(left, top);
        }
    }
}
