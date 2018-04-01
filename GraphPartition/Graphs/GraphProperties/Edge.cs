using System;
using System.Collections.Generic;
using System.Windows;
using MathNet.Numerics;
using Utils;
using Utils.MathUtils;

namespace Graphs.GraphProperties
{
    public struct Edge : IEquatable<Edge>
    {
        public Node Node1 { get; }
        public Node Node2 { get; }
        public double Weight { get; }

        private Edge(Node node1, Node node2, double weight)
        {
            Node1 = node1;
            Node2 = node2;
            Weight = weight;
        }

        public static Edge Create(Node node1, Node node2, double weight)
        {
            var (minNode, maxNode) = GeneralUtils.MinMax(node1.Value, node2.Value);
            return new Edge(new Node(minNode), new Node(maxNode), weight);
        }

        public bool HasNode(Node node) => Node1.Equals(node) || Node2.Equals(node);

        public Edge WithWeight(double newWeight) => Create(Node1, Node2, newWeight);

        public bool Equals(Edge other) => Node1.Equals(other.Node1) && Node2.Equals(other.Node2);
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Edge && Equals((Edge) obj);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Node1.GetHashCode();
                hashCode = (hashCode * 397) ^ Node2.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString() => $"({Node1}, {Node2})";

        public bool IsClashingWith(Edge edge, Dictionary<Node, Point> embedding)
        {
            var edge1Start = embedding[this.Node1];
            var edge1End = embedding[this.Node2];
            var edge2Start = embedding[edge.Node1];
            var edge2End = embedding[edge.Node2];
            var edge1XRange = Range.Create(edge1Start.X, edge1End.X);
            var edge2XRange = Range.Create(edge2Start.X, edge2End.X);
            if (edge1XRange.Max <= edge2XRange.Min)
                return false;
            if (!edge1XRange.IsOverlapping(edge2XRange))
                return false;
            var overlapRange = edge1XRange.GetOverlap(edge2XRange);
            var edge1Line = MathLine.Create(edge1Start.X, edge1End.X, edge1Start.Y, edge1End.Y);
            var edge2Line = MathLine.Create(edge2Start.X, edge2End.X, edge2Start.Y, edge2End.Y);

            var deltaYMin = (edge1Line.Compute(overlapRange.Min) - edge2Line.Compute(overlapRange.Min)).CoerceZero();
            var deltaYMax = (edge1Line.Compute(overlapRange.Max) - edge2Line.Compute(overlapRange.Max)).CoerceZero();
            return deltaYMin * deltaYMax < 0;
        }
    }
}
