using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Graphs.GraphProperties;
using Utils.MathUtils;

namespace GraphPartition.Gui.GraphCreator.GraphCreatorState
{
    public sealed partial class CreatorState
    {
        public static double InitEdgeWeight = 1.0;
        public Brush LineBrush => Brushes.DarkBlue;
        public Brush NodeBrush => Brushes.DarkTurquoise;
        public double AvarageWeight => (MinWeight + MaxWeight) / 2.0;

        public double EllipseWidth { get; }
        public double MinStrokeThickness { get; }
        public double MaxStrokeThickness { get; }

        public Action<int, int> GraphUpdated { get; }
        public EdgesHandler EdgesHandler { get; }
        public Canvas GraphCanvas { get; }
        public Dictionary<Ellipse, Node> Nodes { get; }
        public Dictionary<Ellipse, Point> EllipsesCenters { get; }

        public Dictionary<Edge, Line> Edges { get; }
        public Dictionary<Line, MathLine> LinesMath { get; }

        private MathLine strokeThickness;
        private MathLine StrokeThickness
        {
            get => strokeThickness;
            set
            {
                strokeThickness = value;
                foreach (var edge in Edges.Keys)
                    Edges[edge].StrokeThickness = strokeThickness.Compute(edge.Weight);
            }
        }

        private double minWeight;
        private double MinWeight
        {
            get => minWeight;
            set
            {
                if (value != minWeight)
                {
                    minWeight = value;
                    StrokeThickness = CreateLineThicknessProportions(MinWeight, MaxWeight, MinStrokeThickness, MaxStrokeThickness);
                }
            }
        }

        private double maxWeight;
        private double MaxWeight
        {
            get => maxWeight;
            set
            {
                if (value != maxWeight)
                {
                    maxWeight = value;
                    StrokeThickness = CreateLineThicknessProportions(MinWeight, MaxWeight, MinStrokeThickness, MaxStrokeThickness);
                }
            }
        }

        public CreatorState(Action<int, int> graphUpdated, EdgesHandler edgesHandler, MathLine strokeThickness, double minWeight, double maxWeight, double ellipseWidth, double minStrokeThickness, double maxStrokeThickness, Canvas graphCanvas, Dictionary<Ellipse, Node> nodes, Dictionary<Ellipse, Point> ellipsesCenters, Dictionary<Edge, Line> edges, Dictionary<Line, MathLine> linesMath)
        {
            GraphUpdated = graphUpdated;
            EdgesHandler = edgesHandler;
            this.strokeThickness = strokeThickness;
            this.minWeight = minWeight;
            this.maxWeight = maxWeight;
            EllipseWidth = ellipseWidth;
            MinStrokeThickness = minStrokeThickness;
            MaxStrokeThickness = maxStrokeThickness;
            GraphCanvas = graphCanvas;
            Nodes = nodes;
            EllipsesCenters = ellipsesCenters;
            Edges = edges;
            LinesMath = linesMath;
        }


        public static CreatorState Create(Canvas graphCanvas, EdgesHandler edgesHandler, Action<int, int> graphUpdated)
        {
            var minWeight = InitEdgeWeight;
            var maxWeight = InitEdgeWeight;
            var ellipseWidth = graphCanvas.Width / 15.0;
            var minStroke = ellipseWidth / 20.0;
            var maxStroke = ellipseWidth / 3.0;
            var strokeThickness = CreateLineThicknessProportions(minWeight, maxWeight, minStroke, maxStroke);
            var nodes = new Dictionary<Ellipse, Node>();
            var ellipsesCenters = new Dictionary<Ellipse, Point>();
            var edges = new Dictionary<Edge, Line>();
            var linesMath = new Dictionary<Line, MathLine>();
            return new CreatorState(graphUpdated, edgesHandler, strokeThickness, minWeight, maxWeight, ellipseWidth, minStroke, maxStroke, graphCanvas, nodes, ellipsesCenters, edges, linesMath);
        }

        private static MathLine CreateLineThicknessProportions(double minWeight, double maxWeight, double minThickness,
            double maxThickness)
        {
            return MathLine.Create(minWeight, maxWeight, minThickness, maxThickness);
        }
    }
}
