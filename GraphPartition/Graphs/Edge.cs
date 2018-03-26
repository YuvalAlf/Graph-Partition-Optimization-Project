using System;
using Utils;

namespace Graphs
{
    public struct Edge : IEquatable<Edge>
    {
        public int Node1 { get; }
        public int Node2 { get; }
        public double Weight { get; }

        private Edge(int node1, int node2, double weight)
        {
            Node1 = node1;
            Node2 = node2;
            Weight = weight;
        }

        public static Edge Create(int node1, int node2, double weight)
        {
            var (minNode, maxNode) = GeneralUtils.MinMax(node1, node2);
            return new Edge(minNode, maxNode, weight);
        }

        public Edge WithWeight(double newWeight)
        {
            return Create(Node1, Node2, newWeight);
        }

        public bool Equals(Edge other)
        {
            return Node1 == other.Node1 && Node2 == other.Node2;
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
                return (Node1 * 397) ^ Node2;
            }
        }

        public override string ToString()
        {
            return $"({Node1}, {Node2}) {Weight}";
        }
    }
}
