using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GraphPartition.Gui.GraphCreator;
using GraphPartition.Gui.ProgrammedGui;
using Graphs.Algorithms;
using Graphs.EmbeddingInPlane;
using Graphs.GraphProperties;
using Graphs.Visualizing;
using Optimizations;
using Optimizations.GeneticAlgorithm;
using Utils.ExtensionMethods;
using Utils.MathUtils;

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

        private TextBlock ResultTextBlock { get; }


        private void PrepareResultWindow()
        {
            var stackPanel = new StackPanel();
        }

        private void RunAlgorithmButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.PrepareResultWindow();
            var solution = Run(OptimizationType, GraphVisual.DerivedEmbedding().Graph);
        }


        private IEnumerable<GraphPartitionSolution> Run(OptimizationType optimizationType, Graph graph)
        {
            switch (optimizationType)
            {
                case OptimizationType.Genetic:
                    return GraphPartitionSolution.RunGenetic(GeneticSettings, graph);
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

