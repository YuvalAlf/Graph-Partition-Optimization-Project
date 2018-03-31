using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Utils.ExtensionMethods;

namespace Graphs
{
    public sealed class GraphEmbedding
    {
        public Graph Graph { get; }
        public Dictionary<Node, Point> Embedding { get; }

        public GraphEmbedding(Graph graph, Dictionary<Node, Point> embedding)
        {
            Graph = graph;
            Embedding = embedding;
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

        public IEnumerable<string> ToText()
        {
            foreach (var node in Embedding.Keys)
                yield return $"Node {node.Value}\t left: {Embedding[node].X}\t top: {Embedding[node].Y}";
            foreach (var edge in Graph.Edges)
                yield return $"Edge ({edge.Node1.Value},{edge.Node2.Value})\t weight {edge.Weight}";
        }

        public GraphEmbedding EmbeddFor(TimeSpan runningTime)
        {
            var endingTime = DateTime.Now + runningTime;
            var bestEmbedding = this;
            var bestClashes = this.Clashes();
            while (true)
                foreach (var neighbor in bestEmbedding.Neighbors())
                {
                    if (DateTime.Now > endingTime)
                        return bestEmbedding;
                    var neighborClashes = neighbor.Clashes();
                    if (neighborClashes < bestClashes)
                    {
                        bestClashes = neighborClashes;
                        bestEmbedding = neighbor;
                    }
                }
        }

        private IEnumerable<GraphEmbedding> Neighbors()
        {
            foreach (var node1 in this.Graph.Nodes)
            foreach (var node2 in this.Graph.Nodes)
                if (node1.Value < node2.Value)
                    yield return this.Replace(node1, node2);

        }

        private GraphEmbedding Replace(Node node1, Node node2)
        {
            var embedding = this.Clone();
            var pt1 = embedding.Embedding[node1];
            var pt2 = embedding.Embedding[node2];
            embedding.Embedding[node1] = pt2;
            embedding.Embedding[node2] = pt1;
            return embedding;
        }

        private GraphEmbedding Clone()
        {
            return new GraphEmbedding(Graph, Embedding.Clone());
        }

        private double Clashes()
        {
            int sumOfClashes = 0;
            foreach (var edge1 in this.Graph.Edges)
            foreach (var edge2 in Graph.Edges)
                if (edge1.GetHashCode() < edge2.GetHashCode())
                    if (edge1.IsClashingWith(edge2, this.Embedding))
                        sumOfClashes++;
            return sumOfClashes;
        }
    }
}
