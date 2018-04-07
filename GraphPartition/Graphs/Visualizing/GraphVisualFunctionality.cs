using System;
using System.Linq;
using System.Windows;
using System.Windows.Shapes;
using Graphs.GraphProperties;
using Graphs.Visualizing.EdgeEventArgs;
using Utils.ExtensionMethods;
using Utils.UiUtils.CustomUi.CustomType;
using Utils.UiUtils.DrawingUtils;

namespace Graphs.Visualizing
{
    public sealed partial class GraphVisual
    {
        public event EventHandler<EdgeAddedArgs> EdgeAddedEvent;
        public event EventHandler<EdgeRemovedArgs> EdgeRemovedEvent;
        public event EventHandler<NodesAmountChangedArgs> NodeAmountChangedEvent;

        public void AddNode(Node node, Point point)
        {
            var canvasTopLeft = point.AddX(-NodeWidth / 2).AddY(-NodeWidth / 2);
            var ellipse = NumEllipse.Create(canvasTopLeft, NodeWidth, NodeWidth, 1, node.Value.AsPaddedString(), NodeBrush, NumBrush);
            Canvas.Children.Add(ellipse.Ellipse);
            Nodes[node] = ellipse;
            UpdateCalcNodeWidth();
            NodeAmountChangedEvent?.Invoke(this, new NodesAmountChangedArgs());
        }

        public void RemoveNode(Node node)
        {
            this.Canvas.Children.Remove(Nodes[node].Ellipse);
            Nodes.Remove(node);
            foreach (var edge in Edges.Keys.ToArray())
                if (edge.HasNode(node))
                    RemoveEdge(edge);
            UpdateCalcNodeWidth();
            NodeAmountChangedEvent?.Invoke(this, new NodesAmountChangedArgs());
        }

        public void AddEdge(Edge edge)
        {
            var point1 = Nodes[edge.Node1].Center;
            var point2 = Nodes[edge.Node2].Center;
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
            this.UpdateThicknesses();
            EdgeRemovedEvent?.Invoke(this, new EdgeRemovedArgs(edge));
        }


        public void UpdateWeight(Edge e)
        {
            var line = this.Edges[e];
            this.Edges.Remove(e);
            this.Edges[e] = line;
            UpdateThicknesses();
        }


        public (NumEllipse, double) ClosestEllipseFrom(Point point) 
            => Nodes.Values.MinBy(ellipse => ellipse.Center.DistanceTo(point));

        public (Line, double) ClosestLineFrom(Point point) 
            => Edges.Values.MinBy(line => line.ToMathLine().DistanceFrom(point));
    }
}
