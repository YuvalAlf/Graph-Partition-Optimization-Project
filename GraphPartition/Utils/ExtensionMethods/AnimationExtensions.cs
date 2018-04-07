using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Utils.ExtensionMethods
{
    public static class AnimationExtensions
    {
        public static void MoveOnCanvasTo(this FrameworkElement @this, Point point, TimeSpan time)
        {
            var storyBoard = new Storyboard();
            var doubleAnimationX = new DoubleAnimation(toValue: point.X, duration: time);
            var doubleAnimationY = new DoubleAnimation(toValue: point.Y, duration: time);
            Storyboard.SetTargetProperty(doubleAnimationX, new PropertyPath("(Canvas.Left)"));
            Storyboard.SetTargetProperty(doubleAnimationY, new PropertyPath("(Canvas.Top)"));
            storyBoard.Children.Add(doubleAnimationX);
            storyBoard.Children.Add(doubleAnimationY);
            storyBoard.FillBehavior = FillBehavior.Stop;
            storyBoard.Completed += (sender, args) =>
            {
                storyBoard.Remove();
                @this.SetValue(Canvas.LeftProperty, point.X);
                @this.SetValue(Canvas.TopProperty, point.Y);
            };
            @this.BeginStoryboard(storyBoard);
        }
        public static void MoveOnCanvasTo(this Line @this, Point point1, Point point2, TimeSpan time)
        {
            var storyBoard = new Storyboard();
            var doubleAnimationX1 = new DoubleAnimation(point1.X, time);
            var doubleAnimationY1 = new DoubleAnimation(point1.Y, time);
            var doubleAnimationX2 = new DoubleAnimation(point2.X, time);
            var doubleAnimationY2 = new DoubleAnimation(point2.Y, time);
            Storyboard.SetTargetProperty(doubleAnimationX1, new PropertyPath("(Line.X1)"));
            Storyboard.SetTargetProperty(doubleAnimationY1, new PropertyPath("(Line.Y1)"));
            Storyboard.SetTargetProperty(doubleAnimationX2, new PropertyPath("(Line.X2)"));
            Storyboard.SetTargetProperty(doubleAnimationY2, new PropertyPath("(Line.Y2)"));
            storyBoard.Children.Add(doubleAnimationX1);
            storyBoard.Children.Add(doubleAnimationY1);
            storyBoard.Children.Add(doubleAnimationX2);
            storyBoard.Children.Add(doubleAnimationY2);
            storyBoard.FillBehavior = FillBehavior.Stop;
            storyBoard.Completed += (sender, args) =>
            {
                storyBoard.Remove();
                @this.X1 = point1.X;
                @this.Y1 = point1.Y;
                @this.X2 = point2.X;
                @this.Y2 = point2.Y;
            };
            @this.BeginStoryboard(storyBoard);
        }

    }
}
