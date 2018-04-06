using System.Windows;
using System.Windows.Controls;
using Optimizations.LocalSearchAlgorithm;
using Utils.UiUtils;
using Utils.UiUtils.CustomUi.Creator;

// ReSharper disable once CheckNamespace
namespace GraphPartition.Gui.MainApplication
{
    public partial class MainWindow : Window
    {
        private LocalSearchSettings LocalSearchSettings { get; set; } = LocalSearchSettings.Default;

        private StackPanel InitLocalSearchSettings()
        {
            var title = TextBlockCreator.TitleTextBlock("Local Search Settings");

            return GuiExtensions.CreateStackPanel(title);
        }
    }
}
