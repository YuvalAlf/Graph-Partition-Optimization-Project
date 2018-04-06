using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
