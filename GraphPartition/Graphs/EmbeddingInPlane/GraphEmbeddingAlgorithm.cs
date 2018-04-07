using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Graphs.GraphProperties;
using Optimizations.LocalSearchAlgorithm;
using Utils.DataStructures;

namespace Graphs.EmbeddingInPlane
{
    public sealed partial class GraphEmbedding : ILocalSearch<GraphEmbedding>
    {
        public double NegativePrice => Clashes();

        public GraphEmbedding EmbeddFor(TimeSpan runningTime)
        {
            var localSearch = new LocalSearch<GraphEmbedding>();
            var killProcess = new ConcurrentSignal(false);
            var processKilled = new ConcurrentSignal(false);
            Task.Run(() =>
            {
                Thread.Sleep(runningTime);
                killProcess.Signal();
                processKilled.WaitForSignalBlocking();
            });
            return localSearch.Run(_ => this, LocalSearchSettings.Default, new object(), killProcess, processKilled, new Random()).Last();
        }

        public IEnumerable<GraphEmbedding> Neighbors()
        {
            foreach (var node1 in this.Graph.Nodes)
            foreach (var node2 in this.Graph.Nodes)
                if (node1.Value < node2.Value)
                    yield return this.Replace(node1, node2);
        }

        private GraphEmbedding Replace(Node node1, Node node2)
        {
            var embedding = this.Clone();
            var pt1 = embedding.Embedding[node1];
            var pt2 = embedding.Embedding[node2];
            embedding.Embedding[node1] = pt2;
            embedding.Embedding[node2] = pt1;
            return embedding;
        }


        private int Clashes()
        {
            int sumOfClashes = 0;
            foreach (var edge1 in this.Graph.Edges)
            foreach (var edge2 in Graph.Edges)
                if (edge1.GetHashCode() < edge2.GetHashCode())
                    if (edge1.IsClashingWith(edge2, this.Embedding))
                        sumOfClashes++;
            return sumOfClashes;
        }
    }
}
