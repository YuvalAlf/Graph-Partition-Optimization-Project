using System.Linq;

namespace Graphs
{
    public sealed class Graph
    {
        public int[] Nodes { get; }
        public Edge[] Edges { get; }

        public Graph(int[] nodes, Edge[] edges)
        {
            Nodes = nodes;
            Edges = edges;
        }

        public Graph WithNode(int node)
        {
            var newNodes = Nodes.Append(node).ToArray();
            return new Graph(newNodes, Edges);
        }

        public Graph WithEdge(int node)
        {
            var newNodes = Nodes.Append(node).ToArray();
            return new Graph(newNodes, Edges);
        }
        public Graph WithEdgeWeight(Edge edge, double newWeight)
        {
            var newEdges = Edges.Select(e => e.Equals(edge) ? e.WithWeight(newWeight) : e).ToArray();
            return new Graph(Nodes, newEdges);
        }

        public Graph WithoutEdge(Edge edge)
        {
            var newEdges = Edges.Where(e => !e.Equals(edge)).ToArray();
            return new Graph(Nodes, newEdges);
        }

    }
}
