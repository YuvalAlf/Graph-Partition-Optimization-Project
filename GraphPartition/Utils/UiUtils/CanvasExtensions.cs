using System.Windows;
using System.Windows.Controls;

namespace Utils.UiUtils
{
    public static class CanvasExtensions
    {
        public static void SetCanvasTopLeft(this DependencyObject @this, double left, double top)
        {
            @this.SetValue(Canvas.LeftProperty, left);
            @this.SetValue(Canvas.TopProperty, top);
        }
        public static void SetCanvasTopLeft(this DependencyObject @this, Point point) 
            => @this.SetCanvasTopLeft(point.X, point.Y);

        public static Point GetCanvasPosition(this DependencyObject @this) 
            => new Point((double)@this.GetValue(Canvas.LeftProperty), (double)@this.GetValue(Canvas.TopProperty));
    }
}
