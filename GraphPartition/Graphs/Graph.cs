﻿using System.Linq;

namespace Graphs
{
    public sealed class Graph
    {
        public Node[] Nodes { get; }
        public Edge[] Edges { get; }
        public double MinWeight { get; }
        public double MaxWeight { get; }
        public int MaxNodeValue { get; }

        private Graph(Node[] nodes, Edge[] edges, double minWeight, double maxWeight, int maxNodeValue)
        {
            Nodes = nodes;
            Edges = edges;
            MinWeight = minWeight;
            MaxWeight = maxWeight;
            MaxNodeValue = maxNodeValue;
        }

        public static Graph Create(Node[] nodes, Edge[] edges)
        {
            var minWeight = edges.Min(e => e.Weight);
            var maxWeight = edges.Max(e => e.Weight);
            var maxNodeValue = nodes.Max(n => n.Value);
            return new Graph(nodes, edges, minWeight, maxWeight, maxNodeValue);
        }

        public Graph WithNode(Node node)
        {
            var newNodes = Nodes.Append(node).ToArray();
            return Create(newNodes, Edges);
        }

        public Graph WithEdge(Edge edge)
        {
            var newEdges = Edges.Append(edge).ToArray();
            return Create(Nodes, newEdges);
        }
        public Graph WithEdgeWeight(Edge edge, double newWeight)
        {
            var newEdges = Edges.Select(e => e.Equals(edge) ? e.WithWeight(newWeight) : e).ToArray();
            return Create(Nodes, newEdges);
        }

        public Graph WithoutEdge(Edge edge)
        {
            var newEdges = Edges.Where(e => !e.Equals(edge)).ToArray();
            return Create(Nodes, newEdges);
        }

    }
}