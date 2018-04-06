using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace Utils.DataStructures
{
    public sealed class PagesNavigator<Page>
        where Page : class
    {
        private Dispatcher Dispatcher { get; }
        private Action<Page> ShowPage { get; }
        private Stack<Page> LeftPages { get; }
        private Stack<Page> RightPages { get; }
        private Page currentPage = null;
        private Page CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                Dispatcher.Invoke(() => ShowPage(value));
            }
        }

        public bool CanGoLeft => LeftPages.Count > 0;
        public bool CanGoRight => RightPages.Count > 0;
        public event EventHandler<PagesNavigatorStatusEventArgs> StatusEvent; 

        public PagesNavigator(Action<Page> showPage, Stack<Page> leftPages, Stack<Page> rightPages, Dispatcher dispatcher)
        {
            ShowPage = showPage;
            LeftPages = leftPages;
            RightPages = rightPages;
            Dispatcher = dispatcher;
        }

        public static PagesNavigator<Page> Create(Action<Page> showPage, Dispatcher dispatcher) 
            => new PagesNavigator<Page>(showPage, new Stack<Page>(), new Stack<Page>(), dispatcher);

        public void AddNext(Page page)
        {
            if (CurrentPage != null)
            {
                LeftPages.Push(CurrentPage);
                while (RightPages.Count > 0)
                    LeftPages.Push(RightPages.Pop());
            }
                
            CurrentPage = page;
            StatusEvent?.Invoke(this, new PagesNavigatorStatusEventArgs(CanGoLeft, CanGoRight));
        }

        public void MoveLeft()
        {
            RightPages.Push(CurrentPage);
            CurrentPage = LeftPages.Pop();
            StatusEvent?.Invoke(this, new PagesNavigatorStatusEventArgs(CanGoLeft, CanGoRight));
        }
        public void MoveRight()
        {
            LeftPages.Push(CurrentPage);
            CurrentPage = RightPages.Pop();
            ShowPage(CurrentPage);
            StatusEvent?.Invoke(this, new PagesNavigatorStatusEventArgs(CanGoLeft, CanGoRight));
        }
    }
}
