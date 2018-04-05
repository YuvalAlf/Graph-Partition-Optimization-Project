using System;
using System.Windows.Controls;
using System.Windows.Threading;
using Optimizations;
using Utils.UiUtils;
using Utils.UiUtils.CustomUi.Creator;
using Utils.UiUtils.CustomUi.CustomType;

namespace GraphPartition.Gui.MainApplication.Programming
{
    public static class MethodChoosingViewerCreator
    {
        public static StackPanel Create(Dispatcher d, Action<OptimizationType> onOptimizationChosen)
        {
            var title = TextBlockCreator.TitleTextBlock("Method");
            var chooseOptimizationRadioButtonChooser = RadioButtonChooser.Create(d, OptimizationTypeUtils.All,
                OptimizationTypeUtils.AsString, onOptimizationChosen);

            return GuiExtensions.CreateStackPanel(title, chooseOptimizationRadioButtonChooser);
        }
    }
}