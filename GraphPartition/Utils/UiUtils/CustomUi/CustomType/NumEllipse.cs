using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Utils.UiUtils.CustomUi.Creator;
using Utils.UiUtils.DrawingUtils;

namespace Utils.UiUtils.CustomUi.CustomType
{
    public sealed class NumEllipse
    {
        public Ellipse Ellipse { get; }
        public Rectangle Background { get; }

        public double Width
        {
            get => Ellipse.Width;
            set => Ellipse.Width = value;
        }
        public double Height
        {
            get => Ellipse.Height;
            set => Ellipse.Height = value;
        }

        public Point Center
        {
            get => Ellipse.GetCanvasCenter();
            set => Ellipse.SetCanvasCenter(value);
        } 


        private NumEllipse(Ellipse ellipse, Rectangle background)
        {
            Ellipse = ellipse;
            Background = background;
        }

        public static NumEllipse Create(Point topLeft, double width, double height, int zIndex, string text, Brush backgroundBrush, Brush textBrush)
        {
            var ellipse = EllipseUtils.CreateEllipse(width, height, zIndex, backgroundBrush, topLeft);
            var label = TextBlockCreator.RegularTextBlock(text).WithForeground(textBrush).WithBold().WithFont(new FontFamily("Calibri")).ToLabel();
            var rectangleBackground = new Rectangle {Width = 1.0, Height = 1.0, Fill = backgroundBrush};
            var grid = GuiExtensions.CreateGrid(rectangleBackground.SurroundViewBox(Stretch.Fill), label);
            ellipse.Fill = new VisualBrush(grid);
            return new NumEllipse(ellipse, rectangleBackground);
        }
    }
}
