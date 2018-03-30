using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace GraphPartition.Gui.Programming
{
    public sealed class InteractiveTextBox : TextBox
    {
        public Predicate<string> ValidityCheck { get; }
        public Action<string> OnStringChanged { get; }
        public Dispatcher AppDispatcher { get; }

        public InteractiveTextBox(string initText, Predicate<string> validityCheck, Action<string> onStringChanged, Dispatcher dispatcher)
        {
            ValidityCheck = validityCheck;
            OnStringChanged = onStringChanged;
            AppDispatcher = dispatcher;
            this.TextChanged += (sender, args) => OnTextChanged();
            this.FontFamily = new FontFamily("Calibri");
            this.VerticalAlignment = VerticalAlignment.Center;
            this.Style = Application.Current.Resources["BoundingMarginStyle"] as Style;
            this.Padding = new Thickness(3, 1, 3, 1);
            this.FontSize = 22;
            this.Text = initText;
        }

        private void OnTextChanged()
        {
            var text = this.Text;
            if (!ValidityCheck(text))
                this.Foreground = Brushes.DarkRed;
            else
            {
                this.Foreground = Brushes.DarkGreen;
                AppDispatcher.InvokeAsync(() => OnStringChanged(text));
            }
        }
    }
}
