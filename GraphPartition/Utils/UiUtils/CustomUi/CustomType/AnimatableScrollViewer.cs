using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Utils.UiUtils.CustomUi.CustomType
{
    public sealed class AnimatableScrollViewer : ScrollViewer
    {
        public static readonly DependencyProperty AnimatableVerticalOffsetProperty 
            = DependencyProperty.Register("AnimatableVerticalOffset", typeof(double), typeof(AnimatableScrollViewer),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, OnVerticalOffsetChanged));

        private static void OnVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var scrollViewer = (ScrollViewer) d;
            var offset = (double) e.NewValue;
            scrollViewer.ScrollToVerticalOffset(offset);
        }

        public double AnimatableVerticalOffset
        {
            get => base.VerticalOffset;
            set => SetValue(AnimatableVerticalOffsetProperty, value);
        }


        public void ScrollToEnd(TimeSpan time)
        {
            var storyBoard = new Storyboard();
            var doubleAnimation = new DoubleAnimation(base.VerticalOffset, base.ExtentHeight, time);
            doubleAnimation.EasingFunction = new QuadraticEase {EasingMode = EasingMode.EaseInOut};
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(AnimatableVerticalOffsetProperty));
            storyBoard.Children.Add(doubleAnimation);
            storyBoard.Completed += (sender, args) =>
            {
                this.AnimatableVerticalOffset = base.ExtentHeight;
                storyBoard.Remove();
            };

            this.BeginStoryboard(storyBoard);
        }
    }
}
