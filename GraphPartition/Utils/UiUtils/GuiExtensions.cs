using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using MoreLinq;

namespace Utils.UiUtils
{
    public static class GuiExtensions
    { 
        public static TextBlock WithFont(this TextBlock @this, FontFamily fontFamily)
        {
            @this.FontFamily = fontFamily;
            return @this;
        }
        public static TextBlock WithForeground(this TextBlock @this, Brush brush)
        {
            @this.Foreground = brush;
            return @this;
        }
        public static TextBlock WithBold(this TextBlock @this)
        {
            @this.FontWeight = FontWeights.Bold;
            return @this;
        }
        public static TextBlock PlusFontSize(this TextBlock @this, double plusAmount)
        {
            @this.FontSize += plusAmount;
            return @this;
        }
        public static T WithVerticalAlignment<T>(this T @this, VerticalAlignment v)
            where T : FrameworkElement
        {
            @this.VerticalAlignment = v;
            return @this;
        }
        public static T WithHorizonalAlignment<T>(this T @this, HorizontalAlignment h)
            where T : FrameworkElement
        {
            @this.HorizontalAlignment = h;
            return @this;
        }
        public static T WithMargin<T>(this T @this, double margin)
            where T : FrameworkElement
        {
            @this.Margin = new Thickness(margin);
            return @this;
        }
        public static Label ToLabel(this TextBlock @this)
        {
            var label = new Label();
            label.Content = @this;
            return label;
        }
        public static Grid CreateGrid(params UIElement[] children)
        {
            var grid = new Grid();
            children.ForEach(ch => grid.Children.Add(ch));
            return grid;
        }
        public static StackPanel CreateStackPanel(params UIElement[] children)
        {
            var stackPanel = new StackPanel();
            children.ForEach(ch => stackPanel.Children.Add(ch));
            return stackPanel;
        }
        public static Viewbox SurroundViewBox(this UIElement @this, Stretch stretch)
        {
            var viewBox = new Viewbox();
            viewBox.Stretch = stretch;
            viewBox.Child = @this;
            return viewBox;
        }
    }
}
