using System;

namespace Graphs.GraphProperties
{
    public static class GraphBuilder
    {
        public static Graph CreateSumGraph(int amountOfNodes) => Graph.Create(amountOfNodes, (i, j) => i + j);

        public static Graph CreateMinGraph(int amountOfNodes) => Graph.Create(amountOfNodes, (i, j) => Math.Min(i, j));

        public static Graph CreateMaxGraph(int amountOfNodes) => Graph.Create(amountOfNodes, (i, j) => Math.Max(i, j));

        public static Graph Create4CliqueGraph(int amountOfNodes)
            => Graph.Create(amountOfNodes, (i, j) => Convert.ToInt32((i - 1) / (amountOfNodes / 4) != (j - 1) / (amountOfNodes / 4)));

        public static Graph CreateEvenGraph(int amountOfNodes)
            => Graph.Create(amountOfNodes, (i, j) => Convert.ToInt32(i % 2 == 0 || j % 2 == 0));

        public static Graph CreateRandomizedGraph(int amountOfNodes)
        {
            var rnd = new Random();
            return Graph.Create(amountOfNodes, (i, j) => rnd.NextDouble());
        }

    }
}
