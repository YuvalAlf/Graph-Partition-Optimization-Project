using System.Windows;
using Graphs.Visualizing;

namespace GraphPartition.Gui.GraphCreator.GraphCreatorState
{
    public abstract class StateController
    {
        public GraphVisual GraphVisual { get; }
        
        protected StateController(GraphVisual graphVisual)
        {
            GraphVisual = graphVisual;
        }

        public abstract StateController LeftMouseDownAt(Point point, int times);
        public abstract StateController RightMouseDownAt(Point point, int times);
        public abstract StateController LeftMouseUpAt(Point point);
        public abstract StateController MouseMoveTo(Point point);
    }
}
