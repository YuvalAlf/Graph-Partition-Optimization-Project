using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GraphPartition.Gui.GraphCreator;
using Graphs.Algorithms;
using Graphs.EmbeddingInPlane;
using Graphs.GraphProperties;
using Graphs.Visualizing;
using Optimizations;
using Optimizations.BranchAndBoundAlgorithm;
using Optimizations.GeneticAlgorithm;
using Optimizations.LocalSearchAlgorithm;
using Utils;
using Utils.UiUtils;
using Utils.UiUtils.CustomUi.Creator;

// ReSharper disable once CheckNamespace
namespace GraphPartition.Gui.MainApplication
{
    public partial class MainWindow : Window
    {
        public string InputGraphPath { get; set; } = @"C:\Users\Yuval\Desktop\yuval.graph";
        public string OutputResultPath { get; set; } = @"C:\Users\Yuval\Desktop\GraphPartitionResults";
        public GraphVisual GraphVisual { get; set; }

        public static Brush NodeBrush => Brushes.Black;
        public static Brush NumBrush => Brushes.White;
        public static Brush LineBrush => Brushes.CornflowerBlue;
        public static PenLineCap PenLineCap => PenLineCap.Round;

        private void SetGraph(GraphEmbedding graphEmbedding)
        {
            PauseButton_Click(null, null);
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
            var title = TextBlockCreator.TitleTextBlock("Result Status");
            var statusDescreption = TextBlockCreator.RegularTextBlock("Summation of edges:");
            ResultTextBlock = TextBlockCreator.RegularTextBlock("").WithHorizonalAlignment(HorizontalAlignment.Center).PlusFontSize(10);
            this.StatusViewer.Content = GuiExtensions.CreateStackPanel(title, statusDescreption, ResultTextBlock);
        }


        private void SetSolution(GraphPartitionSolution solution)
        {
            ResultTextBlock.Text = solution.NegativePrice.ToString();
            var canvasBackground = SolutionBackground.Fill;
            var defaultLine = new SolidColorBrush(Colors.DarkGray) { Opacity = 0.3};
            var brushesDictionary = new Dictionary<PartitionType, (Brush, Brush)>();
            brushesDictionary[PartitionType.BigPartition] = (Brushes.DarkBlue, Brushes.Blue);
            brushesDictionary[PartitionType.SmallPartition1] = (Brushes.DarkViolet, Brushes.Plum);
            brushesDictionary[PartitionType.SmallPartition2] = (Brushes.Turquoise, Brushes.MediumTurquoise);
            BestSolutionViewBox.Child = PartitionCanvas(solution, GraphVisual, canvasBackground, brushesDictionary, defaultLine);
        }

        private UIElement PartitionCanvas(GraphPartitionSolution solution, GraphVisual originalGraphVisual, Brush canvasBackground, Dictionary<PartitionType, (Brush, Brush)> lineEdgeBrush, Brush defaultLineBrush)
        {
            var canvas = new Canvas{Width = 1000, Height = 1000};
            canvas.Background = canvasBackground;
            var partitionGraphVisual = originalGraphVisual.Clone(canvas);
            foreach (var node in partitionGraphVisual.Nodes.Keys)
            {
                var ellipse = partitionGraphVisual.Nodes[node];
                var partitionType = solution.PartitionTypeOf(node);
                ellipse.Background.Fill = lineEdgeBrush[partitionType].Item1;
            }

            foreach (var edge in partitionGraphVisual.Edges.Keys)
            {
                var line = partitionGraphVisual.Edges[edge];
                var partitionType = solution.PartitionTypeOf(edge.Node1);
                if (partitionType == solution.PartitionTypeOf(edge.Node2))
                {
                    line.Stroke = lineEdgeBrush[partitionType].Item2;
                    line.StrokeDashArray?.Clear();
                }
                else
                {
                    line.Stroke = defaultLineBrush;
                    line.StrokeDashArray = new DoubleCollection(new [] {1.0, 0.8});
                    line.SetValue(Canvas.ZIndexProperty, -10);
                }
                    
            }
            return canvas;
        }


        private IEnumerable<GraphPartitionSolution> Run(OptimizationType optimizationType, Graph graph)
        {
            var random = new Random();

            IEnumerable<GraphPartitionSolution> RunAlg<Settings>(OptimizationSolver<GraphPartitionSolution, Settings> solver, Settings settings)
            {
                var finishedExecution = new StrongBox<bool>(false);
                var results = solver.Run(GraphPartitionSolution.GenerateRandom(graph), settings, runPauseLock, killTaskSignal, taskKilledSignal, finishedExecution, random);
                foreach (var result in results)
                    yield return result;
                if (finishedExecution.Value == true)
                    Dispatcher.InvokeAsync(() =>
                        StatusViewer.Content.TypeCast<StackPanel>().Children
                            .Add(TextBlockCreator.RegularTextBlock("Finished Running").PlusFontSize(10.0)));
            }

            switch (optimizationType)
            {
                case OptimizationType.Genetic:
                    return RunAlg(new Genetic<GraphPartitionSolution>(), GeneticSettings);
                case OptimizationType.BranchAndBound:
                    return Enumerable.Empty<GraphPartitionSolution>();
                   // var emptyGraphPartition = PartialGraphPartition.CreateEmpty(GraphVisual.Graph);
                   // return RunAlg(new BranchAndBound<PartialGraphPartition, GraphPartitionSolution>(emptyGraphPartition), BranchAndBoundSettings);
                case OptimizationType.LocalSearch:
                    return RunAlg(new LocalSearch<GraphPartitionSolution>(), LocalSearchSettings);
                default:
                    throw new ArgumentOutOfRangeException(nameof(optimizationType), optimizationType, null);
            }
        }
    }
}

