using System.Windows;
using System.Windows.Controls;
using Optimizations.BranchAndBound;
using Utils.UiUtils;
using Utils.UiUtils.CustomUi.Creator;

// ReSharper disable once CheckNamespace
namespace GraphPartition.Gui.MainApplication
{
    public partial class MainWindow : Window
    {
        private BranchAndBoundSettings BranchAndBoundSettings { get; set; } = BranchAndBoundSettings.Default;

        private StackPanel InitBranchAndBoundSettings()
        {
            var title = TextBlockCreator.TitleTextBlock("Branch & Bound Settings");

            return GuiExtensions.CreateStackPanel(title);
        }
    }
}
