using System;

namespace Graphs.GraphProperties
{
    public struct Node : IEquatable<Node>, IComparable<Node>
    {
        public int Value { get; }
        public Node(int value) => Value = value;
        public static implicit operator Node(int num) => new Node(num);


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Node && Equals((Node)obj);
        }
        public bool Equals(Node other) => Value == other.Value;
        public static bool Equals(Node n1, Node n2) => n1.Equals(n2);
        public override int GetHashCode() => Value;
        public int CompareTo(Node other) => this.Value.CompareTo(other.Value);
        public override string ToString() => $"{Value}";

    }
}
