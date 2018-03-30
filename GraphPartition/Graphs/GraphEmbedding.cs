using System.Collections.Generic;

namespace Graphs
{
    public sealed class GraphEmbedding
    {
        public Graph Graph { get; }
        public Dictionary<Node, Coordinate> Embedding { get; }
        public int MaxTop { get; }
        public int MaxLeft { get; }

        public GraphEmbedding(Graph graph, Dictionary<Node, Coordinate> embedding, int maxTop, int maxLeft)
        {
            Graph = graph;
            Embedding = embedding;
            MaxTop = maxTop;
            MaxLeft = maxLeft;
        }
    }
}
