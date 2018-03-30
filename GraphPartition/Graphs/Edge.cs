using System;
using Utils;

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
            return Node1.Equals(other.Node1) && Node2.Equals(other.Node2) && Weight.Equals(other.Weight);
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
                hashCode = (hashCode * 397) ^ Weight.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"({Node1}, {Node2}) {Weight}";
        }
    }
}
