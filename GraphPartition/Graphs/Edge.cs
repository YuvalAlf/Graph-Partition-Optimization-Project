using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using MathNet.Numerics;
using Utils;
using Utils.MathUtils;

namespace Graphs
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

        public Edge WithWeight(double newWeight)
        {
            return Create(Node1, Node2, newWeight);
        }

        public bool Equals(Edge other)
        {
            return Node1.Equals(other.Node1) && Node2.Equals(other.Node2);
        }

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

        public override string ToString()
        {
            return $"({Node1}, {Node2})";
        }

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

        static Edge()
        {
            var dict = new Dictionary<Node, Point>();
            var node1 = new Node(1);
            var node2 = new Node(2);
            var node3 = new Node(3);
            var node4 = new Node(4);
            var node5 = new Node(5);
            var node6 = new Node(6);
            dict.Add(node1, new Point(0, 0));
            dict.Add(node2, new Point(3, 4));
            dict.Add(node3, new Point(0, 5));
            dict.Add(node4, new Point(2, 2));
            dict.Add(node5, new Point(2, 3));
            dict.Add(node6, new Point(2, 4));

            var edge12 = new Edge(node1, node2, 0.0);
            var edge13 = new Edge(node1, node3, 0.0);
            var edge14 = new Edge(node1, node4, 0.0);
            var edge15 = new Edge(node1, node5, 0.0);
            var edge16 = new Edge(node1, node6, 0.0);


            Debug.Assert(!edge12.IsClashingWith(edge13, dict));
            Debug.Assert(!edge12.IsClashingWith(edge14, dict));
            Debug.Assert(!edge12.IsClashingWith(edge15, dict));
            Debug.Assert(!edge12.IsClashingWith(edge16, dict));

            var edge21 = new Edge(node2, node1, 0.0);
            var edge23 = new Edge(node2, node3, 0.0);
            var edge24 = new Edge(node2, node4, 0.0);
            var edge25 = new Edge(node2, node5, 0.0);
            var edge26 = new Edge(node2, node6, 0.0);

            var edge31 = new Edge(node3, node1, 0.0);
            var edge32 = new Edge(node3, node2, 0.0);
            var edge34 = new Edge(node3, node4, 0.0);
            var edge35 = new Edge(node3, node5, 0.0);
            var edge36 = new Edge(node3, node6, 0.0);

            Debug.Assert(edge12.IsClashingWith(edge34, dict));


        }

    }
}
