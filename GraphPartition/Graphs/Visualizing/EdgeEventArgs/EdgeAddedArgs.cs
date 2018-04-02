using System;
using Graphs.GraphProperties;

namespace Graphs.Visualizing.EdgeEventArgs
{
    public class EdgeAddedArgs : EventArgs
    {
        public Edge AddedEdge { get; }

        public EdgeAddedArgs(Edge addedEdge)
        {
            AddedEdge = addedEdge;
        }
    }
}
