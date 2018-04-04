using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GraphPartition.Gui.GraphCreator;
using GraphPartition.Gui.ProgrammedGui;
using Graphs.Algorithms;
using Graphs.EmbeddingInPlane;
using Graphs.GraphProperties;
using Graphs.Visualizing;
using Optimizations;
using Optimizations.BranchAndBound;
using Optimizations.GeneticAlgorithm;
using Optimizations.LocalSearch;
using Utils.ExtensionMethods;
using Utils.MathUtils;
using Utils.UiUtils.DrawingUtils;

// ReSharper disable once CheckNamespace
namespace GraphPartition.Gui.MainApplication
{
    public partial class MainWindow : Window
    {
        public string InputGraphPath { get; set; } = @"C:\Users\Yuval\Desktop\yuval.graph";
        public string OutputResultPath { get; set; } = @"C:\Users\Yuval\Desktop";
        public GraphVisual GraphVisual { get; set; }

        public static Brush NodeBrush => Brushes.Black;
        public static Brush NumBrush => Brushes.White;
        public static Brush LineBrush => Brushes.CornflowerBlue;
        public static PenLineCap PenLineCap => PenLineCap.Round;

        private void SetGraph(GraphEmbedding graphEmbedding)
        {
            this.RunAlgorithmButton.IsEnabled = true;
            this.StaticGraphCanvas.Children.Clear();
            this.GraphVisual = GraphVisual.Create(StaticGraphCanvas, NodeBrush, NumBrush, LineBrush, PenLineCap);
            foreach (var node in graphEmbedding.Graph.Nodes)
                GraphVisual.AddNode(node, graphEmbedding.Embedding[node]);
            foreach (var edge in graphEmbedding.Graph.Edges)
                GraphVisual.AddEdge(edge);
            var scaleViewStackPanel = new StackPanel();
            this.ScaleViewViewer.Content = scaleViewStackPanel;
            WeightsHandler.Create(scaleViewStackPanel, NodeBrush, LineBrush, GraphVisual.NodeWidth,
                GraphVisual.MinLineThickness,
                GraphVisual.MaxLineThickness, GraphVisual.MinWeight, GraphVisual.MaxWeight);
            var edges = EdgesHandler.Create(this.WeightsViewer);
            foreach (var edge in graphEmbedding.Graph.Edges)
                edges.AddEdge(edge, active: false);
        }

        private TextBlock ResultTextBlock { get; set; }


        private void PrepareResultWindow()
        {
            var stackPanel = new StackPanel();
            ResultTextBlock = TextBlockCreator.CreateTitle("");
            this.StatusViewer.Content = ResultTextBlock;
        }

        private void RunAlgorithmButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.PrepareResultWindow();
            var solutions = Run(OptimizationType, GraphVisual.DerivedEmbedding().Graph);
            Task.Run(() =>
            {
                foreach (var solution in solutions)
                {
                    Dispatcher.Invoke(() => SetSolution(solution));
                }

            });
        }

        private void SetSolution(GraphPartitionSolution solution)
        {
            BestSolutionDockPanel.Children.Clear();
            ResultTextBlock.Text = solution.NegativePrice.ToString();
            
            var partitionCanvas1 = PartitionCanvas(PartitionType.SmallPartition1, solution, GraphVisual, Brushes.Blue, Brushes.DarkBlue, 0.7);
            partitionCanvas1.SetValue(DockPanel.DockProperty, Dock.Left);
            var partitionCanvas2 = PartitionCanvas(PartitionType.SmallPartition2, solution, GraphVisual, Brushes.Blue, Brushes.Navy, 0.7);
            partitionCanvas2.SetValue(DockPanel.DockProperty, Dock.Right);
            var partitionCanvas3 = PartitionCanvas(PartitionType.BigPartition, solution, GraphVisual, Brushes.Blue, Brushes.DarkSlateBlue, 0.7);
            this.BestSolutionDockPanel.Children.Add(partitionCanvas1);
            this.BestSolutionDockPanel.Children.Add(partitionCanvas2);
            this.BestSolutionDockPanel.Children.Add(partitionCanvas3);
        }

        private UIElement PartitionCanvas(PartitionType partitionType, GraphPartitionSolution solution, GraphVisual originalGraphVisual, Brush nodeBrush, Brush lineBrush, double opacity)
        {
            var canvas = new Canvas{Width = 1, Height = 1};
            canvas.Background = Brushes.AliceBlue;
            var graph = GraphVisual.Create(canvas, NodeBrush, NumBrush, LineBrush, PenLineCap);
            originalGraphVisual.CopyTo(graph);
            foreach (var node in graph.Nodes.Keys)
            {
                var ellipse = graph.Nodes[node];
                if (solution.Partitions[partitionType].Contains(node))
                {
                    var visualBrush = ellipse.Fill as VisualBrush;
                    var grid = (visualBrush.Visual as Grid);
                    var rect = (grid.Children[0] as Viewbox).Child as Rectangle;
                    rect.Fill = nodeBrush;
                }
                    
            }

            foreach (var edge in graph.Edges.Keys)
            {
                var line = graph.Edges[edge];
                if (solution.PartitionTypeOf(edge.Node1) == solution.PartitionTypeOf(edge.Node2) &&
                    solution.PartitionTypeOf(edge.Node1) == partitionType)
                    line.Stroke = lineBrush;
                else
                    line.Stroke = new SolidColorBrush((LineBrush as SolidColorBrush).Color){Opacity = opacity};
            }
            return canvas.SurroundViewBox(Stretch.Uniform);
        }


        private IEnumerable<GraphPartitionSolution> Run(OptimizationType optimizationType, Graph graph)
        {
            var random = new Random();
            switch (optimizationType)
            {
                case OptimizationType.Genetic:
                    return GraphPartitionSolution.RunGenetic(GeneticSettings, random, graph, () => this.isClosing, runningMonitor);
                case OptimizationType.BranchAndBound:
                    return GraphPartitionSolution.RunBranchAndBound(BranchAndBoundSettings, graph);
                case OptimizationType.LocalSearch:
                    return GraphPartitionSolution.RunLocalSearch(LocalSearchSettings, graph);
                default:
                    throw new ArgumentOutOfRangeException(nameof(optimizationType), optimizationType, null);
            }
        }
    }
}

