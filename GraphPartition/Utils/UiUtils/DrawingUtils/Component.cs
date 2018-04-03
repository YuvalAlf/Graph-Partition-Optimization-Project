using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Utils.UiUtils.DrawingUtils
{
    public static class Component
    {

        public static UIElement WithVerticalAlignment(this FrameworkElement @this, VerticalAlignment v)
        {
            @this.VerticalAlignment = v;
            return @this;
        }
        public static UIElement WithMargin(this FrameworkElement @this, double margin)
        {
            @this.Margin = new Thickness(margin);
            return @this;
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
