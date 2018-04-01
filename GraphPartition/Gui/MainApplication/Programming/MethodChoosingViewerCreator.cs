using System;
using System.Windows.Controls;
using System.Windows.Threading;
using GraphPartition.Gui.ProgrammedGui;
using Optimizations;

namespace GraphPartition.Gui.MainApplication.Programming
{
    public static class MethodChoosingViewerCreator
    {
        public static StackPanel Create(Dispatcher d, Action<OptimizationType> onOptimizationChosen)
        {
            var stackPanel = new StackPanel();
            stackPanel.Children.Add(TextBlockCreator.CreateTitle("Method"));
            stackPanel.Children.Add(RadioButtonChooser.Create(d, OptimizationTypeUtils.All,
                OptimizationTypeUtils.AsString, onOptimizationChosen));

            return stackPanel;
        }
    }
}