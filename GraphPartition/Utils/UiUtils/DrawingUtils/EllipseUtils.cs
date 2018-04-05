using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Utils.ExtensionMethods;

namespace Utils.UiUtils.DrawingUtils
{
    public static class EllipseUtils
    {
        public static Ellipse CreateEllipse(double width, double height, int zIndex, Brush fillBrush, Point canvasTopLeft)
        {
            var ellipse = new Ellipse();
            ellipse.Fill = fillBrush;
            ellipse.Width = width;
            ellipse.Height = height;
            ellipse.SetValue(Canvas.ZIndexProperty, zIndex);
            ellipse.SetCanvasTopLeft(canvasTopLeft);
            return ellipse;
        }

        public static Point GetCanvasCenter(this Ellipse @this)
        {
            var point = @this.GetCanvasPosition();
            return point.AddX(@this.Width / 2).AddY(@this.Height / 2);
        }
        public static void SetCanvasCenter(this Ellipse @this, Point point)
        {
            @this.SetCanvasTopLeft(point.X - @this.Width / 2, point.Y - @this.Height / 2);
        }
    }
}
