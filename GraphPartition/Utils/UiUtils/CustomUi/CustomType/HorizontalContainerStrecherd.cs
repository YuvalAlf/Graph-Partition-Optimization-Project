using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Utils.UiUtils.CustomUi.CustomType
{
    public sealed class HorizontalContainerStrecherd
    {
        private LinkedList<UIElement> Lefts { get; }
        private LinkedList<UIElement> Rights { get; }

        private HorizontalContainerStrecherd(LinkedList<UIElement> lefts, LinkedList<UIElement> rights)
        {
            Lefts = lefts;
            Rights = rights;
        }

        public static HorizontalContainerStrecherd Create()
        {
            return new HorizontalContainerStrecherd(new LinkedList<UIElement>(), new LinkedList<UIElement>());
        }

        public HorizontalContainerStrecherd AddLeft(UIElement element)
        {
            Lefts.AddFirst(element);
            return this;
        }
        public HorizontalContainerStrecherd AddRight(UIElement element)
        {
            Rights.AddFirst(element);
            return this;
        }

        public DockPanel AsDock(UIElement strechedElement)
        {
            DockPanel d = new DockPanel();
            foreach (var left in Lefts)
            {
                left.SetValue(DockPanel.DockProperty, Dock.Left);
                d.Children.Add(left);
            }
            foreach (var right in Rights)
            {
                right.SetValue(DockPanel.DockProperty, Dock.Right);
                d.Children.Add(right);
            }

            d.Children.Add(strechedElement);
            return d;
        }
    }
}
