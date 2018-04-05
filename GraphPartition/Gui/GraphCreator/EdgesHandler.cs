using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Graphs.GraphProperties;
using MoreLinq;
using Utils.ExtensionMethods;
using Utils.UiUtils;
using Utils.UiUtils.CustomUi.Creator;
using Utils.UiUtils.CustomUi.CustomType;

namespace GraphPartition.Gui.GraphCreator
{
    public sealed class EdgesHandler
    {
        private Dictionary<Edge, DockPanel> Edges { get; }
        private StackPanel StackPanel { get; }
        private Action<Edge>[] UpdateWeight { get; }

        public EdgesHandler(StackPanel stackPanel, Dictionary<Edge, DockPanel> edges, params Action<Edge>[] updateWeight)
        {
            StackPanel = stackPanel;
            UpdateWeight = updateWeight;
            Edges = edges;
        }

        public static EdgesHandler Create(ScrollViewer scrollViewer, params Action<Edge>[] updateWeight)
        {
            var stackPanel = new StackPanel();
            scrollViewer.Content = stackPanel;
            stackPanel.Children.Add(TextBlockCreator.TitleTextBlock("Edges"));
            return new EdgesHandler(stackPanel, new Dictionary<Edge, DockPanel>(), updateWeight);
        }

        public void RemoveEdge(Edge e)
        {
            var dockPanel = Edges[e];
            Edges.Remove(e);
            StackPanel.Children.Remove(dockPanel);
        }

        public void AddEdge(Edge edge, bool active = true)
        {
            void WeightChanged(string newWeight) => UpdateWeight.ForEach(a => a.Invoke(edge.WithWeight(double.Parse(newWeight))));

            UIElement textPart =
                !active
                    ? TextBlockCreator.RegularTextBlock(edge.Weight.ToString()).WithVerticalAlignment(VerticalAlignment.Center)
                    : (UIElement) InteractiveTextBox.Create(edge.Weight.ToString(), StringExtensions.IsDouble(), WeightChanged,
                        Dispatcher.CurrentDispatcher);


            var dockPanel = HorizontalContainerStrecherd.Create()
                .AddLeft(TextBlockCreator.RegularTextBlock(edge + ": ").WithBullet())
                .AsDock(textPart);
            this.StackPanel.Children.Add(dockPanel);
            this.Edges[edge] = dockPanel;
        }

    }
}
