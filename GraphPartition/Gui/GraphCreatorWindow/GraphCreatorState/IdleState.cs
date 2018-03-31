using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using Utils.MathUtils;

namespace GraphPartition.Gui.GraphCreatorWindow.GraphCreatorState
{
    public sealed class IdleState : StateController
    {
        public IdleState(CreatorState creatorState) : base(creatorState)
        {
        }

        public override StateController LeftMouseDownAt(Point point, int times)
        {
            var (closestEllipse, distanceEllipse) = this.CreatorState.ClosestEllipseFrom(point);
            var (closestLine, distanceLine) = this.CreatorState.ClosestLineFrom(point);

            var minDistance = Math.Min(distanceLine, distanceEllipse);

            var valueRange = new Range(base.CreatorState.EllipseWidth / 2.0, base.CreatorState.GraphCanvas.Width - base.CreatorState.EllipseWidth / 2.0);
            if (valueRange.Contains(point.X) && valueRange.Contains(point.Y))
                if (times == 2 && minDistance > base.CreatorState.EllipseWidth)
                {
                    base.CreatorState.AddEllipseNode(point);
                    return this;
                }

            if (times == 1  && distanceEllipse < base.CreatorState.EllipseWidth)
            {
                return StartedLineState.Create(base.CreatorState, closestEllipse, point);
            }

            return this;
        }

        public override StateController RightMouseDownAt(Point point, int times)
        {
            if (times == 2)
            {
                var (closestEllipse, distanceEllipse) = this.CreatorState.ClosestEllipseFrom(point);
                if (distanceEllipse < base.CreatorState.EllipseWidth / 2)
                    base.CreatorState.RemoveEllipse(closestEllipse);
            }

            return this;
        }

        public override StateController LeftMouseUpAt(Point point)
        {
            return this;
        }

        public override StateController MouseMoveTo(Point point)
        {
            return this;
        }
    }
}
