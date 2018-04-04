using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Graphs.EmbeddingInPlane;
using Graphs.GraphProperties;
using Utils.ExtensionMethods;
using Utils.MathUtils;
using Utils.UiUtils.DrawingUtils;
using Graphs.Visualizing.EdgeEventArgs;
using Utils.UiUtils;

namespace Graphs.Visualizing
{
    public sealed class GraphVisual
    {
        public double CanvasWidth => Canvas.Width;
        public double NodeWidth { get; private set; }
        public double MaxLineThickness => NodeWidth / 3.0;
        public double MinLineThickness => NodeWidth / 15.0;
        public Brush NodeBrush { get; }
        public Brush NodeNumBrush { get; }
        public Brush LineBrush { get; }
        public PenLineCap PenLineCap { get; }
        public Canvas Canvas { get; }
        public Dictionary<Node, Ellipse> Nodes { get; }
        public Dictionary<Edge, Line> Edges { get; }
        public double MinWeight => Edges.Keys.Min(e => e.Weight, 0.0);
        public double MaxWeight => Edges.Keys.Max(e => e.Weight, 2.0);
        public event EventHandler<EdgeAddedArgs> EdgeAddedEvent;
        public event EventHandler<EdgeRemovedArgs> EdgeRemovedEvent;
        public event EventHandler<NodesAmountChangedArgs> NodeAmountChangedEvent;

        public GraphVisual(Brush nodeBrush, Brush nodeNumBrush, Brush lineBrush, PenLineCap penLineCap, Canvas canvas, Dictionary<Node, Ellipse> nodes, Dictionary<Edge, Line> edges)
        {
            NodeBrush = nodeBrush;
            NodeNumBrush = nodeNumBrush;
            LineBrush = lineBrush;
            PenLineCap = penLineCap;
            Canvas = canvas;
            Nodes = nodes;
            Edges = edges;
            UpdateCalcNodeWidth();
        }

        private void UpdateCalcNodeWidth()
        {
            var nodesAmount = this.Nodes.Count;
            var level = nodesAmount / 10;
            var width = CanvasWidth / (10 + level * Math.Sqrt(nodesAmount + 10));
            foreach (var ellipse in Nodes.Values)
            {
                var ellipseCenter = ellipse.GetCanvasCenter();
                ellipse.Width = ellipse.Height = width;
                ellipse.SetCanvasTopLeft(ellipseCenter.X - width / 2, ellipseCenter.Y - width / 2);
            }
            NodeWidth = width;
            UpdateThicknesses();
        }


        public static GraphVisual Create(Canvas canvas, Brush nodeBrush, Brush numBrush, Brush lineBrush, PenLineCap penLineCap)
        {
            return new GraphVisual(nodeBrush, numBrush, lineBrush, penLineCap, canvas, new Dictionary<Node, Ellipse>(), new Dictionary<Edge, Line>());
        }

        public void AddNode(Node node, Point point)
        {
            var ellipseFillGrid = new Grid();
            ellipseFillGrid.Children.Add(new Rectangle { Width = 1.0, Height = 1.0, Fill = NodeBrush }.SurroundViewBox(Stretch.Fill));
            ellipseFillGrid.Children.Add(new Label { Content = node.Value.AsPaddedString(), Foreground = NodeNumBrush });
            var fillBrush = new VisualBrush(ellipseFillGrid);
            var canvasTopLeft = point.AddX(-NodeWidth / 2).AddY(-NodeWidth / 2);
            var ellipse = EllipseUtils.CreateEllipse(NodeWidth, NodeWidth, 1, fillBrush, canvasTopLeft);
            Canvas.Children.Add(ellipse);
            Nodes[node] = ellipse;
            UpdateCalcNodeWidth();
            NodeAmountChangedEvent?.Invoke(this, new NodesAmountChangedArgs());
        }

        public void AddEdge(Edge edge)
        {
            var point1 = Nodes[edge.Node1].GetCanvasCenter();
            var point2 = Nodes[edge.Node2].GetCanvasCenter();
            var line = LineUtils.CreateLine(point1, point2, LineBrush, MinLineThickness, PenLineCap);
            Canvas.Children.Add(line);
            Edges[edge] = line;
            this.UpdateThicknesses();
            EdgeAddedEvent?.Invoke(this, new EdgeAddedArgs(edge));
        }

        public void RemoveEdge(Edge edge)
        {
            this.Canvas.Children.Remove(Edges[edge]);
            this.Edges.Remove(edge);
            UpdateThicknesses();
            EdgeRemovedEvent?.Invoke(this, new EdgeRemovedArgs(edge));
        }

        public void RemoveNode(Node node)
        {
            this.Canvas.Children.Remove(Nodes[node]);
            Nodes.Remove(node);
            foreach (var edge in Edges.Keys.ToArray())
                if (edge.HasNode(node))
                    RemoveEdge(edge);
            this.UpdateThicknesses();
            NodeAmountChangedEvent?.Invoke(this, new NodesAmountChangedArgs());
        }

        public void UpdateWeight(Edge e)
        {
            var line = this.Edges[e];
            this.Edges.Remove(e);
            this.Edges[e] = line;
            UpdateThicknesses();
        }

        private void UpdateThicknesses()
        {
            var proportion = MathLine.Create(MinWeight, MaxWeight, MinLineThickness, MaxLineThickness);
            foreach (var edge in Edges.Keys)
                Edges[edge].StrokeThickness = proportion.Compute(edge.Weight);
        }

        public (Ellipse, double) ClosestEllipseFrom(Point point)
        {
            return Nodes.Values.MinBy(ellipse => ellipse.GetCanvasCenter().DistanceTo(point));
        }
        public (Line, double) ClosestLineFrom(Point point)
        {
            return Edges.Values.MinBy(line => line.ToMathLine().DistanceFrom(point));
        }

        public GraphEmbedding DerivedEmbedding()
        {
            var graph = new Graph(Nodes.Keys.ToArray(), Edges.Keys.ToArray());
            var embedding = new Dictionary<Node, Point>();
            foreach (var node in Nodes.Keys)
                embedding[node] = Nodes[node].GetCanvasCenter();
            return new GraphEmbedding(graph, embedding);
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

        public Task EmbeddFor(TimeSpan runningTime, Action callback, Dispatcher dispatcher)
        {
            void Embedd()
            {
                GraphEmbedding embedding = null;
                dispatcher.Invoke(() => embedding = this.DerivedEmbedding());
                var bestEmbedding = embedding.EmbeddFor(runningTime);
                dispatcher.Invoke(() => UpdateEmbedding(bestEmbedding));
                dispatcher.Invoke(callback);
            }

            return Task.Run(() => Embedd());
        }

        public void UpdateEmbedding(GraphEmbedding embedding)
        {
            foreach (var node in Nodes.Keys)
            {
                var ellipse = Nodes[node];
                var ellipseCenter = embedding.Embedding[node];
                var pointToMove = ellipseCenter.AddX(-ellipse.Width / 2).AddY(-ellipse.Height / 2);
                ellipse.MoveOnCanvasTo(pointToMove, TimeSpan.FromSeconds(2.0));
            }
            foreach (var edge in Edges.Keys)
            {
                var line = Edges[edge];
                var point1 = embedding.Embedding[edge.Node1];
                var point2 = embedding.Embedding[edge.Node2];
                line.MoveOnCanvasTo(point1, point2, TimeSpan.FromSeconds(2.0));
            }
        }

        public void CopyTo(GraphVisual graph)
        {
            foreach (var node in Nodes.Keys)
                graph.AddNode(node, Nodes[node].GetCanvasCenter());
            foreach (var edge in Edges.Keys)
                graph.AddEdge(edge);
        }
    }
}
