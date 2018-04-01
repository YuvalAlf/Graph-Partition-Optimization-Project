using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Threading;
using GraphPartition.Gui.ProgrammedGui;
using Graphs.GraphProperties;
using Utils.ExtensionMethods;

namespace GraphPartition.Gui.GraphCreator
{
    public sealed class EdgesHandler
    {
        private Dictionary<Edge, DockPanel> Edges { get; }
        private StackPanel StackPanel { get; }
        private Action<Edge> UpdateWeight { get; }

        public EdgesHandler(StackPanel stackPanel, Action<Edge> updateWeight, Dictionary<Edge, DockPanel> edges)
        {
            StackPanel = stackPanel;
            UpdateWeight = updateWeight;
            Edges = edges;
        }

        public static EdgesHandler Create(ScrollViewer scrollViewer, Action<Edge> updateWeight)
        {
            var stackPanel = new StackPanel();
            scrollViewer.Content = stackPanel;
            stackPanel.Children.Add(TextBlockCreator.CreateTitle("Edges"));
            return new EdgesHandler(stackPanel, updateWeight, new Dictionary<Edge, DockPanel>());
        }

        public void RemoveEdge(Edge e)
        {
            var dockPanel = Edges[e];
            Edges.Remove(e);
            StackPanel.Children.Remove(dockPanel);
        }

        public void AddEdge(Edge edge)
        {
            void WeightChanged(string newWeight) => UpdateWeight(edge.WithWeight(double.Parse(newWeight)));

            var dockPanel = HorizontalContainerStrecherd.Create()
                .AddLeft(TextBlockCreator.CreateNormal(edge + ": ").WithBullet())
                .AsDock(InteractiveTextBox.Create(edge.Weight.ToString(), StringExtensions.IsDouble(), WeightChanged,
                    Dispatcher.CurrentDispatcher));
            this.StackPanel.Children.Add(dockPanel);
            this.Edges[edge] = dockPanel;
        }

    }
}
