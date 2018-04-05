using System;
using System.Windows;
using System.Windows.Controls;

namespace Utils.UiUtils.CustomUi.Creator
{
    public static class ButtonCreator
    {
        public static Button Create(string text, Action onClick)
        {
            var button = new Button();
            button.Style = Application.Current.Resources["BoundingMarginStyle"] as Style;
            button.Content = TextBlockCreator.RegularTextBlock(text);
            button.Click += (sender, args) => onClick(); 
            return button;
        }
    }
}
