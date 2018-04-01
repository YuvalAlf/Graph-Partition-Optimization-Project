using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Utils.UiUtils.DrawingUtils
{
    public static class LineUtils
    {
        public static void SetLinePosition(this Line line, Point p1, Point p2)
        {
            line.X1 = p1.X;
            line.Y1 = p1.Y;
            line.X2 = p2.X;
            line.Y2 = p2.Y;
        }

        public static Line CreateLine(Point point1, Point point2, Brush lineBrush, double thickness, PenLineCap strokeDashCap)
        {
            var line = new Line();
            line.SetLinePosition(point1, point2);
            line.Stroke = lineBrush;
            line.StrokeThickness = thickness;
            line.StrokeDashCap = strokeDashCap;
            return line;
        }
    }
}
