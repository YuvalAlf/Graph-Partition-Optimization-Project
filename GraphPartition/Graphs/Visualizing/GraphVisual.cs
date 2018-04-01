using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Graphs.GraphProperties;
using Utils.ExtensionMethods;
using Utils.MathUtils;
using Utils.UiUtils.DrawingUtils;

namespace Graphs.Visualizing
{
    public sealed class GraphVisual
    {
        public double CanvasWidth => Canvas.Width;
        public double NodeWidth => Canvas.Width / 15.0;
        public double MaxLineThickness => NodeWidth / 3.0;
        public double MinLineThickness => NodeWidth / 15.0;
        public Brush NodeBrush { get; }
        public Brush NodeNumBrush { get; }
        public Brush LineBrush { get; }
        public Canvas Canvas { get; }
        public Dictionary<Node, Ellipse> Nodes { get; }
        public Dictionary<Edge, Line> Edges { get; }

        public GraphVisual(Brush nodeBrush, Brush nodeNumBrush, Brush lineBrush, Canvas canvas, Dictionary<Node, Ellipse> nodes, Dictionary<Edge, Line> edges)
        {
            NodeBrush = nodeBrush;
            NodeNumBrush = nodeNumBrush;
            LineBrush = lineBrush;
            Canvas = canvas;
            Nodes = nodes;
            Edges = edges;
        }

        public GraphVisual Create(Canvas canvas, Brush nodeBrush, Brush numBrush, Brush lineBrush)
        {
            return new GraphVisual(nodeBrush, numBrush, lineBrush, canvas, new Dictionary<Node, Ellipse>(), new Dictionary<Edge, Line>());
        }

        public void AddNode(Node node, Point point)
        {
            var ellipseFillGrid = new Grid();
            ellipseFillGrid.Children.Add(new Rectangle { Width = 1.0, Height = 1.0, Fill = NodeBrush }.SurroundViewBox(Stretch.Fill));
            ellipseFillGrid.Children.Add(new Label { Content = node.Value.AsPaddedString(), Foreground = NodeNumBrush });
            var fillBrush = new VisualBrush(ellipseFillGrid);
            var canvasTopLeft = point.AddX(-NodeWidth / 2).AddY(-NodeWidth / 2);
            var ellipse = EllipseUtils.CreateEllipse(NodeWidth, NodeWidth, 500, fillBrush, canvasTopLeft);
            Canvas.Children.Add(ellipse);
            Nodes[node] = ellipse;
        }

        public void AddEdge(Edge edge)
        {
            var point1 = Nodes[edge.Node1].GetCanvasCenter();
            var point2 = Nodes[edge.Node2].GetCanvasCenter();
            var line = LineUtils.CreateLine(point1, point2, LineBrush, MinLineThickness, PenLineCap.Round);
            Canvas.Children.Add(line);
            Edges[edge] = line;
            this.UpdateThicknesses();
        }

        public void RemoveEdge(Edge edge)
        {
            this.Canvas.Children.Remove(Edges[edge]);
            this.Edges.Remove(edge);
            UpdateThicknesses();
        }

        public void RemoveNode(Node node)
        {
            this.Canvas.Children.Remove(Nodes[node]);
            Nodes.Remove(node);
            foreach (var edge in Edges.Keys.ToArray())
                if (edge.HasNode(node))
                    RemoveEdge(edge);
            this.UpdateThicknesses();
        }

        public void UpdateWeight(Edge e)
        {
            RemoveEdge(e);
            AddEdge(e);
        }

        private void UpdateThicknesses()
        {
            if (Edges.Count == 0)
                return;
            var minWeight = Edges.Keys.Min(e => e.Weight);
            var maxWeight = Edges.Keys.Max(e => e.Weight);
            var proportion = MathLine.Create(minWeight, maxWeight, MinLineThickness, MaxLineThickness);
            foreach (var edge in Edges.Keys)
                Edges[edge].StrokeThickness = proportion.Compute(edge.Weight);
        }
    }
}
