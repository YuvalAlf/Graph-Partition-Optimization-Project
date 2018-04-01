using System;
using System.Collections.Generic;
using Graphs.GraphProperties;

namespace Graphs.EmbeddingInPlane
{
    public sealed partial class GraphEmbedding
    {
        public GraphEmbedding EmbeddFor(TimeSpan runningTime)
        {
            var endingTime = DateTime.Now + runningTime;
            var bestEmbedding = this;
            var bestClashes = this.Clashes();
            while (true)
                foreach (var neighbor in bestEmbedding.Neighbors())
                {
                    if (DateTime.Now > endingTime || bestClashes == 0)
                        return bestEmbedding;
                    var neighborClashes = neighbor.Clashes();
                    if (neighborClashes < bestClashes)
                    {
                        bestClashes = neighborClashes;
                        bestEmbedding = neighbor;
                    }
                }
        }

        private IEnumerable<GraphEmbedding> Neighbors()
        {
            foreach (var node1 in this.Graph.Nodes)
            foreach (var node2 in this.Graph.Nodes)
                if (node1.Value < node2.Value)
                    yield return this.Replace(node1, node2);

        }

        private GraphEmbedding Replace(Node node1, Node node2)
        {
            var embedding = this.Clone();
            var pt1 = embedding.Embedding[node1];
            var pt2 = embedding.Embedding[node2];
            embedding.Embedding[node1] = pt2;
            embedding.Embedding[node2] = pt1;
            return embedding;
        }


        private int Clashes()
        {
            int sumOfClashes = 0;
            foreach (var edge1 in this.Graph.Edges)
            foreach (var edge2 in Graph.Edges)
                if (edge1.GetHashCode() < edge2.GetHashCode())
                    if (edge1.IsClashingWith(edge2, this.Embedding))
                        sumOfClashes++;
            return sumOfClashes;
        }
    }
}
