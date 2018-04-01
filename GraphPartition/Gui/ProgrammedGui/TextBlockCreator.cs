using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphPartition.Gui.ProgrammedGui
{
    public static class TextBlockCreator
    {
        public static TextBlock CreateNormal(string text)
        {
            var textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.Style = Application.Current.Resources["NormalTextBlockStyle"] as Style;
            return textBlock;
        }

        public static TextBlock CreateTitle(string text)
        {
            var textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.Style = Application.Current.Resources["TitleTextBlockStyle"] as Style;
            return textBlock;
        }

        public static BulletDecorator WithBullet(this TextBlock @this)
        {
            var bulletDecorator = new BulletDecorator();
            bulletDecorator.Bullet = new Ellipse {Height = 8, Width = 8, Fill = Brushes.Black};
            bulletDecorator.Style = Application.Current.Resources["BulletStyle"] as Style;
            bulletDecorator.VerticalAlignment = VerticalAlignment.Center;
            @this.Margin = new Thickness(10, 0, 5, 0);
            bulletDecorator.Child = @this;
            return bulletDecorator;
        }
    }
}
