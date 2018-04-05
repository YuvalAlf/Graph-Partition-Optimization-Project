using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Utils.UiUtils
{
    public sealed class AnimatableScrollViewer : ScrollViewer
    {
        public static readonly DependencyProperty AnimatableVerticalOffsetProperty = DependencyProperty.Register(
            "AnimatableVerticalOffset", typeof(double), typeof(AnimatableScrollViewer),
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
            var doubleAnimation = new DoubleAnimation(this.VerticalOffset, this.ExtentHeight, time);
            doubleAnimation.EasingFunction = new QuadraticEase() {EasingMode = EasingMode.EaseInOut};
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(AnimatableVerticalOffsetProperty));
            storyBoard.Children.Add(doubleAnimation);
            storyBoard.Completed += (sender, args) =>
            {
                this.ScrollToBottom();
                storyBoard.Remove();
            };


            this.BeginStoryboard(storyBoard);
        }
    }
}
