using System;

namespace Utils.DataStructures
{
    public sealed class PagesNavigatorStatusEventArgs : EventArgs
    {
        public bool CanGoLeft { get; }
        public bool CanGoRight { get; }

        public PagesNavigatorStatusEventArgs(bool canGoLeft, bool canGoRight)
        {
            CanGoLeft = canGoLeft;
            CanGoRight = canGoRight;
        }
    }
}
