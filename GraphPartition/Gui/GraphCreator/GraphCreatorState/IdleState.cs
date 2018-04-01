using System;
using System.Windows;
using Utils.MathUtils;

namespace GraphPartition.Gui.GraphCreator.GraphCreatorState
{
    public sealed class IdleState : StateController
    {
        public IdleState(CreatorState creatorState) : base(creatorState) { }

        public override StateController LeftMouseUpAt(Point point) => this;

        public override StateController MouseMoveTo(Point point) => this;

        public override StateController LeftMouseDownAt(Point point, int times)
        {
            var (closestEllipse, distanceEllipse) = CreatorState.ClosestEllipseFrom(point);
            var (closestLine, distanceLine) = CreatorState.ClosestLineFrom(point);

            var minDistance = Math.Min(distanceLine, distanceEllipse);

            var valueRange = new Range(CreatorState.EllipseWidth / 2.0, CreatorState.GraphCanvas.Width - CreatorState.EllipseWidth / 2.0);
            if (valueRange.Contains(point.X) && valueRange.Contains(point.Y))
                if (times == 2 && minDistance > CreatorState.EllipseWidth)
                {
                    CreatorState.AddEllipseNode(point);
                    return this;
                }

            if (times == 1  && distanceEllipse < CreatorState.EllipseWidth)
            {
                return StartedLineState.Create(CreatorState, closestEllipse, point);
            }

            return this;
        }

        public override StateController RightMouseDownAt(Point point, int times)
        {
            if (times == 2)
            {
                var (closestEllipse, distanceEllipse) = CreatorState.ClosestEllipseFrom(point);
                if (distanceEllipse < CreatorState.EllipseWidth / 2)
                    CreatorState.RemoveEllipse(closestEllipse);
            }

            return this;
        }
    }
}
