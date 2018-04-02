using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Graphs.GraphProperties;
using Graphs.Visualizing;
using Utils.ExtensionMethods;
using Utils.UiUtils.DrawingUtils;

namespace GraphPartition.Gui.GraphCreator.GraphCreatorState
{
    public sealed class StartedLineState : StateController
    {
        private Ellipse StartingEllipse { get; }
        private Line WorkingLine { get; }
        private double Weight { get; }

        private StartedLineState(GraphVisual graphVisual, Ellipse startingEllipse, Line workingLine, double weight) 
            : base(graphVisual)
        {
            StartingEllipse = startingEllipse;
            WorkingLine = workingLine;
            Weight = weight;
        }

        public static StartedLineState Create(GraphVisual graphVisual, Ellipse startingEllipse, Point currentPoint)
        {
            var ellipseCenter = startingEllipse.GetCanvasCenter();
            var minWeight = graphVisual.Edges.Keys.Min(e => e.Weight, 1);
            var maxWeight = graphVisual.Edges.Keys.Max(e => e.Weight, 1);
            var weight = (maxWeight + minWeight) / 2.0;
            var thickness = (graphVisual.MaxLineThickness + graphVisual.MinLineThickness) / 2.0;
            var line = LineUtils.CreateLine(ellipseCenter, currentPoint, graphVisual.LineBrush, thickness, graphVisual.PenLineCap);
            graphVisual.Canvas.Children.Add(line);
            return new StartedLineState(graphVisual, startingEllipse, line, weight);
        }

        public override StateController LeftMouseDownAt(Point point, int times)
        {
            throw new InvalidOperationException();
        }

        public override StateController LeftMouseUpAt(Point point)
        {
            GraphVisual.Canvas.Children.Remove(WorkingLine);
            var (closestEllipse, distance) = GraphVisual.ClosestEllipseFrom(point);
            if (distance < GraphVisual.NodeWidth && closestEllipse != StartingEllipse)
            {
                var startingNode = GraphVisual.Nodes.GetKeyOf(StartingEllipse, ReferenceEquals);
                var endingNode = GraphVisual.Nodes.GetKeyOf(closestEllipse, ReferenceEquals);
                var edge = Edge.Create(startingNode, endingNode, Weight);
                if (!GraphVisual.Edges.ContainsKey(edge))
                    GraphVisual.AddEdge(edge);
            }
            return new IdleState(GraphVisual);
        }

        public override StateController MouseMoveTo(Point point)
        {
            WorkingLine.X2 = point.X;
            WorkingLine.Y2 = point.Y;
            return this;
        }

        public override StateController RightMouseDownAt(Point point, int times)
        {
            return this;
        }
    }
}
