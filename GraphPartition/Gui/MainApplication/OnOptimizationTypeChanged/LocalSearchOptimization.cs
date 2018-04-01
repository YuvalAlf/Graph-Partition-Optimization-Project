using System.Windows;
using System.Windows.Controls;
using GraphPartition.Gui.ProgrammedGui;
using Optimizations.LocalSearch;

// ReSharper disable once CheckNamespace
namespace GraphPartition.Gui.MainApplication
{
    public partial class MainWindow : Window
    {
        private LocalSearchSettings LocalSearchSettings { get; set; } = LocalSearchSettings.Default;

        private StackPanel InitLocalSearchSettings()
        {
            var stackPanel = new StackPanel();

            stackPanel.Children.Add(TextBlockCreator.CreateTitle("Local Search Settings"));

            return stackPanel;
        }
    }
}
