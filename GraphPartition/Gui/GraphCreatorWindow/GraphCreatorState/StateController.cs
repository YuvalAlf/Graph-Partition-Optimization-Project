using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GraphPartition.Gui.GraphCreatorWindow.GraphCreatorState
{
    public abstract class StateController
    {
        public CreatorState CreatorState { get; }

        protected StateController(CreatorState creatorState)
        {
            CreatorState = creatorState;
        }

        public abstract StateController LeftMouseDownAt(Point point, int times);
        public abstract StateController RightMouseDownAt(Point point, int times);
        public abstract StateController LeftMouseUpAt(Point point);
        public abstract StateController MouseMoveTo(Point point);
    }
}
