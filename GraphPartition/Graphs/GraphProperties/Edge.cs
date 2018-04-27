using System;
using System.Collections.Generic;
using Utils;

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
            return new Edge(minNode, maxNode, weight);
        }

        public IEnumerable<Node> Nodes
        {
            get
            {
                yield return Node1;
                yield return Node2;
            }
        }


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


        public static Edge Parse(string str)
        {
            var items = str.Split("(,) ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var node1 = (Node) int.Parse(items[0]);
            var node2 = (Node) int.Parse(items[1]);
            var weight = double.Parse(items[2]);
            return Edge.Create(node1, node2, weight);
        }

        public override string ToString() => $"({Node1},{Node2})".PadRight(15) + Weight;
    }
}
