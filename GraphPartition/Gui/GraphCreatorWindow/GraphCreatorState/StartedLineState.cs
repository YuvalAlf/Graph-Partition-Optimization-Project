using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace GraphPartition.Gui.GraphCreatorWindow.GraphCreatorState
{
    public sealed class StartedLineState : StateController
    {
        private Ellipse StartingEllipse { get; }
        private Line WorkingLine { get; }
        private double Weight { get; }

        public StartedLineState(CreatorState creatorState, Ellipse startingEllipse, Line workingLine, double weight) 
            : base(creatorState)
        {
            StartingEllipse = startingEllipse;
            WorkingLine = workingLine;
            Weight = weight;
        }

        public static StartedLineState Create(CreatorState creatorState, Ellipse startingEllipse, Point currentPoint)
        {
            var ellipseCenter = creatorState.EllipsesCenters[startingEllipse];
            var weight = creatorState.AvarageWeight;
            var line = creatorState.AddLine(ellipseCenter, currentPoint, weight);
            return new StartedLineState(creatorState, startingEllipse, line, weight);
        }

        public override StateController LeftMouseDownAt(Point point, int times)
        {
            throw new InvalidOperationException();
        }

        public override StateController LeftMouseUpAt(Point point)
        {
            var (closestEllipse, distance) = base.CreatorState.ClosestEllipseFrom(point);
            if (distance < base.CreatorState.EllipseWidth && closestEllipse != StartingEllipse)
                base.CreatorState.FinalLine(WorkingLine, Weight, StartingEllipse, closestEllipse);
            else
                base.CreatorState.GraphCanvas.Children.Remove(WorkingLine);
            return new IdleState(base.CreatorState);
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
