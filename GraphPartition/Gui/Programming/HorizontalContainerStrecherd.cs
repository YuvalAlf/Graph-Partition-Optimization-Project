using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GraphPartition.Gui.Programming
{
    public sealed class HorizontalContainerStrecherd
    {
        private LinkedList<UIElement> Lefts = new LinkedList<UIElement>();
        private LinkedList<UIElement> Rights = new LinkedList<UIElement>();
        public static HorizontalContainerStrecherd Create()
        {
            return new HorizontalContainerStrecherd();
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
