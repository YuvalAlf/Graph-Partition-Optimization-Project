using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Utils.UiUtils.DrawingUtils
{
    public static class Component
    {

        public static Viewbox SurroundViewBox(this UIElement @this, Stretch stretch)
        {
            var viewBox = new Viewbox();
            viewBox.Stretch = stretch;
            viewBox.Child = @this;
            return viewBox;
        }
    }
}
