using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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


        public static void SaveAsPng(this Canvas @this, string filePath, int scale)
        {
            @this.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            var rect = new Rect(@this.DesiredSize);
            @this.Arrange(rect);
            var bmp = new RenderTargetBitmap((int)(scale * rect.Width),(int)(scale * rect.Height), scale * 96, scale * 96, PixelFormats.Default);
            bmp.Render(@this);
            var enc = new PngBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(bmp));

            using (var stream = File.Create(filePath))
                enc.Save(stream);
        }
    }
}
