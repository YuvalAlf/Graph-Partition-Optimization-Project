using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Xml;
using Graphs.EmbeddingInPlane;
using Graphs.GraphProperties;
using Utils.ExtensionMethods;
using Utils.UiUtils.DrawingUtils;

namespace Graphs.Visualizing
{
    public sealed partial class GraphVisual
    {
        public GraphEmbedding DerivedEmbedding()
        {
            var embedding = new Dictionary<Node, Point>();
            foreach (var node in Nodes.Keys)
                embedding[node] = Nodes[node].Ellipse.GetCanvasCenter();
            return new GraphEmbedding(Graph, embedding);
        }

        public void SetNewEmbedding(GraphEmbedding embedding)
        {
            foreach (var node in Nodes.Keys.ToArray())
                this.RemoveNode(node);
            foreach (var node in embedding.Graph.Nodes)
                this.AddNode(node, embedding.Embedding[node]);
            foreach (var edge in embedding.Graph.Edges)
                this.AddEdge(edge);
        }

        public Task EmbeddFor(TimeSpan annimationTime, TimeSpan runningTime, Action callback, Dispatcher dispatcher)
        {
            return Task.Run(() =>
            {
                GraphEmbedding embedding = dispatcher.Get(this.DerivedEmbedding);
                var bestEmbedding = embedding.EmbeddFor(runningTime);
                dispatcher.InvokeAsync(() => UpdateEmbedding(bestEmbedding, annimationTime));
                dispatcher.InvokeAsync(callback);
            });
        }

        public void UpdateEmbedding(GraphEmbedding embedding, TimeSpan animationTime)
        {
            foreach (var node in Nodes.Keys)
            {
                var ellipse = Nodes[node];
                var ellipseCenter = embedding.Embedding[node];
                var pointToMove = ellipseCenter.AddX(-ellipse.Ellipse.Width / 2).AddY(-ellipse.Ellipse.Height / 2);
                ellipse.Ellipse.MoveOnCanvasTo(pointToMove, animationTime);
            }
            foreach (var edge in Edges.Keys)
            {
                var line = Edges[edge];
                var point1 = embedding.Embedding[edge.Node1];
                var point2 = embedding.Embedding[edge.Node2];
                line.MoveOnCanvasTo(point1, point2, animationTime);
            }
        }
    }
}
