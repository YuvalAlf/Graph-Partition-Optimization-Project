using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GraphPartition.Gui.Programming;
using Graphs;
using Utils.ExtensionMethods;
using Utils.MathUtils;

namespace GraphPartition.Gui
{
    public sealed class GraphEmbeddingToCanvas : Canvas
    {
        /*public Dictionary<Node, Border> NodesCircles { get; }
        public Dictionary<Edge, Line> EdgesLines { get; }
        public MathLine WeightThicknessProportion { get; }


        public GraphEmbeddingToCanvas(Dictionary<Node, Border> nodesCircles, Dictionary<Edge, Line> edgesLines, MathLine weightThicknessProportion)
        {
            NodesCircles = nodesCircles;
            EdgesLines = edgesLines;
            WeightThicknessProportion = weightThicknessProportion;

            foreach (var line in edgesLines.Values)
                base.Children.Add(line);
            foreach (var circle in nodesCircles.Values)
                base.Children.Add(circle);

        }

        public static GraphEmbeddingToCanvas Create(GraphEmbedding embedding)
        {
            var proportion = MathLine.Create(embedding.Graph.MinWeight, embedding.Graph.MaxWeight, 0.003, 0.03);
            var nodesCircles = new Dictionary<Node, Border>();
            var edgesLines = new Dictionary<Edge, Line>();
            int maxDigits = embedding.Graph.MaxNodeValue.CountDigits();
            foreach (var node in embedding.Graph.Nodes)
                nodesCircles[node] = CreateNodeBorder(node.Value, maxDigits, embedding.Embedding[node]);
            foreach (var edge in embedding.Graph.Edges)
                edgesLines[edge] = CreateEdgeLine(embedding.Embedding[edge.Node1], embedding.Embedding[edge.Node2], proportion.Compute(edge.Weight));
            return new GraphEmbeddingToCanvas(nodesCircles, edgesLines, proportion);
        }

        private static Border CreateNodeBorder(int nodeValue, int digits, Coordinate coordinate)
        {
            var border = new Border();
            border.CornerRadius = new CornerRadius(0.25);
            border.BorderThickness = new Thickness(0.01);
            border.BorderBrush = Brushes.Black;
            border.Background = Brushes.Bisque;

            var textBlock = TextBlockCreator.CreateNormal(nodeValue.AsString(digits: digits));

            border.Child = textBlock;
            coordinate.SetToValueCanvas(border);

            return border;
        }

        private static Line CreateEdgeLine(Coordinate c1, Coordinate c2, double thickness)
        {
            var line = new Line();
            line.X1 = c1.Left;
            line.Y1 = c1.Top;
            line.X2 = c2.Left;
            line.Y2 = c2.Top;
            line.Stroke = Brushes.Coral;
            line.StrokeThickness = thickness;
            new Coordinate().SetToValueCanvas(line);
            return line;
        }*/
    }
}
