using System;

namespace Graphs.GraphProperties
{
    public static class GraphBuilder
    {
        public static Graph CreateMinGraph(int amountOfNodes) => Graph.Create(amountOfNodes, (i, j) => Math.Min(i, j));

        public static Graph CreateMaxGraph(int amountOfNodes) => Graph.Create(amountOfNodes, (i, j) => Math.Max(i, j));

        public static Graph CreateRandomizedGraph(int amountOfNodes)
        {
            var rnd = new Random();
            return Graph.Create(amountOfNodes, (i, j) => rnd.NextDouble());
        }
    }
}
