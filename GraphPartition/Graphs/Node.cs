using System;

namespace Graphs
{
    public struct Node : IEquatable<Node>
    {
        public int Value { get; }

        public Node(int value)
        {
            Value = value;
        }

        public bool Equals(Node other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Node && Equals((Node) obj);
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public override string ToString()
        {
            return $"{Value}";
        }

        public static bool Equals(Node n1, Node n2)
        {
            return n1.Equals(n2);
        }
    }
}
