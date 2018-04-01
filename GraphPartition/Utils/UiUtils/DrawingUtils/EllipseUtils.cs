using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Utils.ExtensionMethods;

namespace Utils.UiUtils.DrawingUtils
{
    public static class EllipseUtils
    {
        public static Ellipse CreateEllipse(double width, double height, double zIndex, Brush fillBrush, Point canvasTopLeft)
        {
            var ellipse = new Ellipse();
            ellipse.Fill = fillBrush;
            ellipse.Width = width;
            ellipse.Height = height;
            ellipse.SetValue(Panel.ZIndexProperty, zIndex);
            ellipse.SetCanvasTopLeft(canvasTopLeft);
            return ellipse;
        }

        public static Point GetCanvasCenter(this Ellipse @this)
        {
            var point = @this.GetCanvasPosition();
            return point.AddX(@this.Width / 2).AddY(@this.Height / 2);
        }
    }
}
