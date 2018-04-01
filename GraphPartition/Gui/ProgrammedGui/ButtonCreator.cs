using System;
using System.Windows;
using System.Windows.Controls;

namespace GraphPartition.Gui.ProgrammedGui
{
    public static class ButtonCreator
    {
        public static Button Create(string text, Action onClick)
        {
            var button = new Button();
            button.Style = Application.Current.Resources["BoundingMarginStyle"] as Style;
            button.Content = TextBlockCreator.CreateNormal(text);
            button.Click += (sender, args) => onClick(); 
            return button;
        }
    }
}
