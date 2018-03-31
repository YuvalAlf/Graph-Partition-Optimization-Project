using System.Linq;

namespace Graphs
{
    public sealed class Graph
    {
        public Node[] Nodes { get; }
        public Edge[] Edges { get; }

        public Graph(Node[] nodes, Edge[] edges)
        {
            Nodes = nodes;
            Edges = edges;
        }

        public static Graph Create(Node[] nodes, Edge[] edges)
        {
            return new Graph(nodes, edges);
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
