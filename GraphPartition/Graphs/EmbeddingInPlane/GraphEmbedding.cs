using System.Collections.Generic;
using System.Windows;
using Graphs.GraphProperties;
using Utils.ExtensionMethods;

namespace Graphs.EmbeddingInPlane
{
    public sealed partial class GraphEmbedding
    {
        public Graph Graph { get; }
        public Dictionary<Node, Point> Embedding { get; }

        public GraphEmbedding(Graph graph, Dictionary<Node, Point> embedding)
        {
            Graph = graph;
            Embedding = embedding;
        }

        private GraphEmbedding Clone() => new GraphEmbedding(Graph, Embedding.Clone());
    }
}
