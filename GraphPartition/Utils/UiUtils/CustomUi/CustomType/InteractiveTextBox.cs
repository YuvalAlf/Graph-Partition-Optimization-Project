using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Utils.UiUtils.CustomUi.CustomType
{
    public static class InteractiveTextBox
    {
        public static TextBox Create(string initText, Predicate<string> validityCheck, Action<string> onStringChanged, Dispatcher dispatcher)
        {
            var textBox = new TextBox();
            textBox.TextChanged += (sender, args) => OnTextChanged(textBox, validityCheck, onStringChanged, dispatcher);
            textBox.Style = Application.Current.Resources["TextBoxStyle"] as Style;
            
            textBox.Text = initText;
            return textBox;
        }

        private static void OnTextChanged(TextBox textBox, Predicate<string> validityCheck, Action<string> onStringChanged, Dispatcher dispatcher)
        {
            if (!validityCheck(textBox.Text))
                textBox.Foreground = Brushes.DarkRed;
            else
            {
                textBox.Foreground = Brushes.DarkGreen;
                dispatcher.InvokeAsync(() => onStringChanged(textBox.Text));
            }
        }
    }
}
