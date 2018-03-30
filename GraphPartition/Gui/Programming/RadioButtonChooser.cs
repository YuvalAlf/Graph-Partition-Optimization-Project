using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace GraphPartition.Gui.Programming
{
    public static class RadioButtonChooser
    {
        public static StackPanel Create<T>(Dispatcher dispatcher, T[] options, Func<T, string> optionsText, Action<T> onChosenChanged)
        {
            RadioButton CreateRadioButton(T data)
            {
                var radioButton = new RadioButton();
                radioButton.Content = TextBlockCreator.CreateNormal(optionsText(data));
                radioButton.Style = Application.Current.Resources["RadioButtonStyle"] as Style;
                radioButton.Checked += (sender, args) => dispatcher.InvokeAsync(() => onChosenChanged(data));
                return radioButton;
            }


            var radioButtonGroup = new StackPanel();

            foreach (var option in options)
                radioButtonGroup.Children.Add(CreateRadioButton(option));

            return radioButtonGroup;
        }
    }
}
