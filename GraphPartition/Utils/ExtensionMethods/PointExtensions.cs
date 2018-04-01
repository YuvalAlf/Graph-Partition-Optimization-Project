using System;
using System.Windows;

namespace Utils.ExtensionMethods
{
    public static class PointExtensions
    {
        public static Point AddX(this Point @this, double amount)
        {
            return new Point(@this.X + amount, @this.Y);
        }
        public static Point AddY(this Point @this, double amount)
        {
            return new Point(@this.X, @this.Y + amount);
        }

        public static double DistanceTo(this Point @this, Point other)
        {
            var dx = @this.X - other.X;
            var dy = @this.Y - other.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
