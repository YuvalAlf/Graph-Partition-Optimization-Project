using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Utils.ExtensionMethods;
using Utils.UiUtils.CustomUi.Creator;

namespace Utils.UiUtils.CustomUi.CustomType
{
    public static class RadioButtonChooser
    {
        public static StackPanel Create<T>(Dispatcher dispatcher, T[] options, Func<T, string> optionsText, Action<T> onChosenChanged)
        {
            RadioButton CreateRadioButton(T data)
            {
                var radioButton = new RadioButton();
                radioButton.Content = TextBlockCreator.RegularTextBlock(optionsText(data));
                radioButton.Style = Application.Current.Resources["RadioButtonStyle"] as Style;
                radioButton.Checked += (sender, args) => dispatcher.InvokeAsync(() => onChosenChanged(data));
                return radioButton;
            }
            return GuiExtensions.CreateStackPanel(options.Select(CreateRadioButton).Cast<UIElement>().ToArray());
        }
    }
}
