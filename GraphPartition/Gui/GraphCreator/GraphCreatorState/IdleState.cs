using System;
using System.Linq;
using System.Windows;
using Graphs.GraphProperties;
using Graphs.Visualizing;
using Utils.ExtensionMethods;
using Utils.MathUtils;

namespace GraphPartition.Gui.GraphCreator.GraphCreatorState
{
    public sealed class IdleState : StateController
    {
        public IdleState(GraphVisual graphVisual) : base(graphVisual) { }

        public override StateController LeftMouseUpAt(Point point) => this;

        public override StateController MouseMoveTo(Point point) => this;

        public override StateController LeftMouseDownAt(Point point, int times)
        {
            var (closestEllipse, distanceEllipse) = GraphVisual.ClosestEllipseFrom(point);
            var (closestLine, distanceLine) = GraphVisual.ClosestLineFrom(point);

            var minDistance = Math.Min(distanceLine, distanceEllipse);

            var valueRange = new Range(GraphVisual.NodeWidth / 2.0, GraphVisual.CanvasWidth - GraphVisual.NodeWidth / 2.0);
            if (valueRange.Contains(point.X) && valueRange.Contains(point.Y))
                if (times == 2 && minDistance > GraphVisual.NodeWidth)
                {
                    var nodeValue = Enumerable.Range(1, int.MaxValue).First(x => !GraphVisual.Nodes.ContainsKey(x));
                    GraphVisual.AddNode(nodeValue , point);
                    return this;
                }

            if (times == 1  && distanceEllipse < GraphVisual.NodeWidth)
                return StartedLineState.Create(GraphVisual, closestEllipse, point);

            return this;
        }

        public override StateController RightMouseDownAt(Point point, int times)
        {
            if (times == 2)
            {
                var (closestEllipse, distanceEllipse) = GraphVisual.ClosestEllipseFrom(point);
                if (distanceEllipse < GraphVisual.NodeWidth / 2)
                    GraphVisual.RemoveNode(GraphVisual.Nodes.GetKeyOf(closestEllipse, ReferenceEquals));
            }

            return this;
        }
    }
}
