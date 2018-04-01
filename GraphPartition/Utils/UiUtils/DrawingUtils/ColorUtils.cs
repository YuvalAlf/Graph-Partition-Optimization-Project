using System.Windows.Media;

namespace Utils.UiUtils.DrawingUtils
{
    public static class ColorUtils
    {
        public static SolidColorBrush ToBrush(this Color @this)
            => new SolidColorBrush(@this);

        public static Color WithTransparancy(this Color @this, float alpha)
            => Color.FromScRgb(alpha, @this.ScR, @this.ScG, @this.ScB);
    }
}
