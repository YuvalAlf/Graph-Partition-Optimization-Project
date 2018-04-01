namespace Graphs.GraphProperties
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
    }
}
