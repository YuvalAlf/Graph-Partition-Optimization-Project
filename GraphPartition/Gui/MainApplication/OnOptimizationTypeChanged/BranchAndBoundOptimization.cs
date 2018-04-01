using System.Windows;
using System.Windows.Controls;
using GraphPartition.Gui.ProgrammedGui;
using Optimizations.BranchAndBound;

// ReSharper disable once CheckNamespace
namespace GraphPartition.Gui.MainApplication
{
    public partial class MainWindow : Window
    {
        private BranchAndBoundSettings BranchAndBoundSettings { get; set; } = BranchAndBoundSettings.Default;

        private StackPanel InitBranchAndBoundSettings()
        {
            var stackPanel = new StackPanel();

            stackPanel.Children.Add(TextBlockCreator.CreateTitle("Branch & Bound Settings"));

            return stackPanel;
        }
    }
}
