using System;
using Graphs.GraphProperties;

namespace Graphs.Visualizing.EdgeEventArgs
{
    public sealed class EdgeRemovedArgs : EventArgs
    {
        public Edge RemovedEdge { get; }

        public EdgeRemovedArgs(Edge removedEdge) => RemovedEdge = removedEdge;
    }
}
