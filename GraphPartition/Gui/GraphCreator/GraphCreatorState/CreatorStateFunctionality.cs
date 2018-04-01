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
using Utils;
using Utils.ExtensionMethods;
using Utils.MathUtils;

namespace GraphPartition.Gui.GraphCreator.GraphCreatorState
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
            ellipse.SetValue(Panel.ZIndexProperty, 500);

            GraphCanvas.Children.Add(ellipse);

            Nodes[ellipse] = node;
            EllipsesCenters[ellipse] = curretPoint;
            GraphUpdated(Nodes.Count, Edges.Count);
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
            line.StrokeDashCap = PenLineCap.Round;
            line.StrokeEndLineCap = PenLineCap.Round;
            return line;
        }

        public void FinalLine(Line line, double weight, Ellipse startingEllipse, Ellipse endingEllipse)
        {
            var edge = Edge.Create(Nodes[startingEllipse], Nodes[endingEllipse], weight);
            if (Edges.ContainsKey(edge))
            {
                GraphCanvas.Children.Remove(line);
                return;
            }

            if (Nodes[startingEllipse].Value > Nodes[endingEllipse].Value)
                GeneralUtils.SwapValues(ref startingEllipse, ref endingEllipse);

            line.X1 = EllipsesCenters[startingEllipse].X;
            line.Y1 = EllipsesCenters[startingEllipse].Y;
            line.X2 = EllipsesCenters[endingEllipse].X;
            line.Y2 = EllipsesCenters[endingEllipse].Y;
            var mathLine = MathLine.Create(line.X1, line.X2, line.Y1, line.Y2);
            
            Edges[edge] = line;
            LinesMath[line] = mathLine;
            EdgesHandler.AddEdge(edge);
            GraphUpdated(Nodes.Count, Edges.Count);
        }

        public void RemoveEllipse(Ellipse ellipse)
        {
            var node = Nodes[ellipse];
            Nodes.Remove(ellipse);
            EllipsesCenters.Remove(ellipse);
            GraphCanvas.Children.Remove(ellipse);
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
                EdgesHandler.RemoveEdge(edge);
            }
            GraphUpdated(Nodes.Count, Edges.Count);
        }

        public void UpdateWeight(Edge edge)
        {
            var line = Edges[edge];
            Edges.Remove(edge);
            Edges[edge] = line;
            MinWeight = Enumerable.Min<Edge>(Edges.Keys, e => e.Weight);
            MaxWeight = Enumerable.Max<Edge>(Edges.Keys, e => e.Weight);
            line.StrokeThickness = StrokeThickness.Compute(edge.Weight);
        }

        public GraphEmbedding DerivedEmbedding()
        {
            var nodes = Enumerable.ToArray<Node>(Nodes.Values);
            var edges = Enumerable.ToArray<Edge>(Edges.Keys);
            var embedding = new Dictionary<Node, Point>();
            foreach (var ellipse in Nodes.Keys)
                embedding[Nodes[ellipse]] = EllipsesCenters[ellipse];
            return new GraphEmbedding(Graph.Create(nodes, edges), embedding);
        }

        public Task EmbeddFor(TimeSpan runningTime, Action callback, Dispatcher dispatcher)
        {
            void Embedd()
            {
                var embedding = DerivedEmbedding();
                var bestEmbedding = embedding.EmbeddFor(runningTime);
                dispatcher.Invoke(() => UpdateEmbedding(bestEmbedding));
                dispatcher.Invoke(callback);
            }

            return Task.Run(() => Embedd());
        }

        public void UpdateEmbedding(GraphEmbedding embedding)
        {
            foreach (var ellipse in Nodes.Keys)
            {
                var node = Nodes[ellipse];
                var ellipseCenter = embedding.Embedding[node];
                var pointToMove = ellipseCenter.AddX(-EllipseWidth / 2).AddY(-EllipseWidth / 2);
                AnimationExtensions.MoveOnCanvasTo(ellipse, pointToMove, TimeSpan.FromSeconds(2.0));
                EllipsesCenters[ellipse] = ellipseCenter;
            }

            foreach (var edge in Edges.Keys)
            {
                var line = Edges[edge];
                var point1 = EllipsesCenters[DictionaryExtensions.GetKeyOf<Ellipse, Node>(Nodes, edge.Node1, Node.Equals)];
                var point2 = EllipsesCenters[DictionaryExtensions.GetKeyOf<Ellipse, Node>(Nodes, edge.Node2, Node.Equals)];
                AnimationExtensions.MoveOnCanvasTo(line, point1, point2, TimeSpan.FromSeconds(2.0));
                var mathLine = MathLine.Create(point1.X, point2.X, point1.Y, point2.Y);
                LinesMath[line] = mathLine;
            }
        }
        public void SetNewEmbedding(GraphEmbedding embedding)
        {
            foreach (var ellipse in Enumerable.ToArray<Ellipse>(EllipsesCenters.Keys))
                RemoveEllipse(ellipse);
            foreach (var node in embedding.Graph.Nodes)
                AddEllipseNode(embedding.Embedding[node]);
            foreach (var edge in embedding.Graph.Edges)
            {
                var startingEllipse = DictionaryExtensions.GetKeyOf<Ellipse, Node>(Nodes, edge.Node1, Node.Equals);
                var endingEllipse = DictionaryExtensions.GetKeyOf<Ellipse, Node>(Nodes, edge.Node2, Node.Equals);
                var line = AddLine(EllipsesCenters[startingEllipse], EllipsesCenters[endingEllipse], edge.Weight);
                FinalLine(line, edge.Weight, startingEllipse, endingEllipse);}
                
        }
    }
}
