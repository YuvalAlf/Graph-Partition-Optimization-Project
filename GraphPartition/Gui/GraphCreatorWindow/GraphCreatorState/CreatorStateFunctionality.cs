using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Graphs;
using Utils.ExtensionMethods;
using Utils.MathUtils;

namespace GraphPartition.Gui.GraphCreatorWindow.GraphCreatorState
{
    public sealed partial class CreatorState
    {
        public static Point GetEllipseCenter(Ellipse e)
        {
            var x = (double)e.GetValue(Canvas.LeftProperty) + e.Width / 2;
            var y = (double)e.GetValue(Canvas.TopProperty) + e.Height / 2;
            return new Point(x, y);
        }

        public (Ellipse, double) ClosestEllipseFrom(Point point)
        {
            Ellipse closestEllipse = null;
            double closestDistance = double.PositiveInfinity;

            foreach (var ellipse in EllipsesCenters.Keys)
            {
                var distance = point.DistanceTo(EllipsesCenters[ellipse]);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEllipse = ellipse;
                }
            }

            return (closestEllipse, closestDistance);
        }
        public (Line, double) ClosestLineFrom(Point point)
        {
            Line closestLine = null;
            double closestDistance = double.PositiveInfinity;

            foreach (var line in LinesMath.Keys)
            {
                var mathLine = LinesMath[line];
                var distance = mathLine.DistanceFrom(point);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestLine = line;
                }
            }

            return (closestLine, closestDistance);
        }


        public void AddEllipseNode(Point curretPoint)
        {
            var nodeNum = Enumerable.Range(1, int.MaxValue).First(num => !Nodes.ContainsValue(new Node(num)));
            var node = new Node(nodeNum);
            var ellipse = new Ellipse();
            ellipse.Width = ellipse.Height = EllipseWidth;
            var grid = new Grid();
            grid.Children.Add(new Viewbox
            {
                Stretch = Stretch.Fill,
                Child = new Rectangle {Width = 1, Height = 1, Fill = Brushes.Black}
            });
            grid.Children.Add(new Label {Content = nodeNum.CountDigits() == 1 ? " " + nodeNum + " " : nodeNum.ToString(), Foreground = Brushes.White});
            ellipse.Fill = new VisualBrush(grid);
            ellipse.SetValue(Canvas.LeftProperty, curretPoint.X - EllipseWidth / 2);
            ellipse.SetValue(Canvas.TopProperty, curretPoint.Y - EllipseWidth / 2);
            ellipse.SetValue(Canvas.ZIndexProperty, 500);

            GraphCanvas.Children.Add(ellipse);

            this.Nodes[ellipse] = node;
            this.EllipsesCenters[ellipse] = curretPoint;
        }

        public Line AddLine(Point startingPoint, Point endingPoint, double weight)
        {
            var line = new Line();
            line.X1 = startingPoint.X;
            line.Y1 = startingPoint.Y;
            line.X2 = endingPoint.X;
            line.Y2 = endingPoint.Y;
            line.StrokeThickness = StrokeThickness.Compute(weight);
            line.Stroke = LineBrush;
            GraphCanvas.Children.Add(line);
            return line;
        }

        public void FinalLine(Line line, double weight, Ellipse startingEllipse, Ellipse endingEllipse)
        {
            var edge = Edge.Create(Nodes[startingEllipse], Nodes[endingEllipse], weight);
            if (Edges.ContainsKey(edge))
                return;

            line.X1 = EllipsesCenters[startingEllipse].X;
            line.Y1 = EllipsesCenters[startingEllipse].Y;
            line.X2 = EllipsesCenters[endingEllipse].X;
            line.Y2 = EllipsesCenters[endingEllipse].Y;
            var mathLine = MathLine.Create(line.X1, line.X2, line.Y1, line.Y2);

            this.Edges[edge] = line;
            this.LinesMath[line] = mathLine;
            this.EdgesHandler.AddEdge(edge);
        }

        public void RemoveEllipse(Ellipse ellipse)
        {
            var node = Nodes[ellipse];
            this.Nodes.Remove(ellipse);
            this.EllipsesCenters.Remove(ellipse);
            this.GraphCanvas.Children.Remove(ellipse);
            var edgesToRemove = new List<Edge>();
            foreach (var edge in Edges.Keys)
                if (edge.Node1.Equals(node) || edge.Node2.Equals(node))
                    edgesToRemove.Add(edge);

            foreach (var edge in edgesToRemove)
            {
                var line = Edges[edge];
                Edges.Remove(edge);
                LinesMath.Remove(line);
                GraphCanvas.Children.Remove(line);
                this.EdgesHandler.RemoveEdge(edge);
            }
        }

        public void UpdateWeight(Edge edge)
        {
            var line = this.Edges[edge];
            this.Edges.Remove(edge);
            this.Edges[edge] = line;
            this.MinWeight = Edges.Keys.Min(e => e.Weight);
            this.MaxWeight = Edges.Keys.Max(e => e.Weight);
        }
    }
}
