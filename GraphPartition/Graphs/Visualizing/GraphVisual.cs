using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Graphs.GraphProperties;
using Utils.ExtensionMethods;
using Utils.MathUtils;
using Utils.UiUtils.CustomUi.CustomType;

namespace Graphs.Visualizing
{
    public sealed partial class GraphVisual
    {
        public double CanvasWidth => Canvas.Width;
        public double NodeWidth { get; private set; }
        public double MaxLineThickness => NodeWidth / 8.0;
        public double MinLineThickness => NodeWidth / 40.0;
        public Brush NodeBrush { get; }
        public Brush NumBrush { get; }
        public Brush LineBrush { get; }
        public PenLineCap PenLineCap { get; }
        public Canvas Canvas { get; }
        public Dictionary<Node, NumEllipse> Nodes { get; }
        public Dictionary<Edge, Line> Edges { get; }
        public bool BigGraph => Edges.Count > 1000;
        public Graph Graph => Graph.Create(Nodes.Keys.ToArray(), Edges.Keys.ToArray());
        public double MinWeight => Edges.Keys.Min(e => e.Weight, 0.0);
        public double MaxWeight => Edges.Keys.Max(e => e.Weight, 2.0);

        private GraphVisual(Brush nodeBrush, Brush numBrush, Brush lineBrush, PenLineCap penLineCap, Canvas canvas, Dictionary<Node, NumEllipse> nodes, Dictionary<Edge, Line> edges)
        {
            NodeBrush = nodeBrush;
            NumBrush = numBrush;
            LineBrush = lineBrush;
            PenLineCap = penLineCap;
            Canvas = canvas;
            Nodes = nodes;
            Edges = edges;
            UpdateCalcNodeWidth();
        }


        public static GraphVisual Create(Canvas canvas, Brush nodeBrush, Brush numBrush, Brush lineBrush, PenLineCap penLineCap)
            => new GraphVisual(nodeBrush, numBrush, lineBrush, penLineCap, canvas, new Dictionary<Node, NumEllipse>(), new Dictionary<Edge, Line>());


        public GraphVisual Clone(Canvas canvas)
        {
            var newGraphVisual = GraphVisual.Create(canvas, NodeBrush, NumBrush, LineBrush, PenLineCap);
            foreach (var node in Nodes.Keys)
                newGraphVisual.AddNode(node, Nodes[node].Center);
            foreach (var edge in Edges.Keys)
                newGraphVisual.AddEdge(edge);
            return newGraphVisual;
        }


        private void UpdateCalcNodeWidth()
        {
            const int updateEach = 5;
            var nodesAmount = this.Nodes.Count;
            var level = nodesAmount / updateEach;
            var width = CanvasWidth / (8 + Math.Pow(Math.Log(level * updateEach + 2), 1.5) * Math.Sqrt(level * updateEach + 2));
            foreach (var ellipse in Nodes.Values)
            {
                var ellipseCenter = ellipse.Center;
                ellipse.Width = ellipse.Height = width;
                ellipse.Center = ellipseCenter;
            }
            NodeWidth = width;
            UpdateThicknesses();
        }

        private void UpdateThicknesses()
        {
            var proportion = MathLine.Create(MinWeight, MaxWeight, MinLineThickness, MaxLineThickness);
            foreach (var edge in Edges.Keys)
                Edges[edge].StrokeThickness = proportion.Compute(edge.Weight);
        }
    }
}
