using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Graphs.EmbeddingInPlane;
using Graphs.GraphProperties;
using Utils.ExtensionMethods;

namespace GraphCreator
{
    class Program
    {
        static GraphEmbedding CreateEmbedding(int numOfNodes, Func<int, int, double> weights)
        {
            var nodes = Enumerable.Range(1, numOfNodes).ToArray();
            var edges = new List<Edge>();
            for(int i = 1; i < numOfNodes; i++)
                for (int j = i + 1; j <= numOfNodes; j++)
                    edges.Add(Edge.Create(i, j, weights(i, j)));
            var graph = Graph.Create(nodes.Map(n => (Node)n), edges.ToArray());
            var embedding = new Dictionary<Node,Point>();
            foreach (var node in graph.Nodes)
            {
                var angle = (node.Value / (double)numOfNodes) * 2 * Math.PI;
                var left = Math.Sin(angle) * 400;
                var top = Math.Cos(angle) * 400;
                embedding[node] = new Point(500 + left, 500 + top);
            }
            return new GraphEmbedding(graph, embedding);
        }
        static GraphEmbedding BiDirectional(int numOfNodes, double weight)
        {
            var nodes = Enumerable.Range(1, numOfNodes).ToArray();
            var edges = new List<Edge>();
            for(int i = 1; i <= numOfNodes / 2; i++)
                for (int j = 1 + numOfNodes / 2; j <= numOfNodes; j++)
                    edges.Add(Edge.Create(i, j, 1));
            var graph = Graph.Create(nodes.Map(n => (Node)n), edges.ToArray());
            var embedding = new Dictionary<Node,Point>();
            foreach (var node in graph.Nodes)
            {
                var left = node.Value <= numOfNodes / 2 ? 100 : 900;
                var top = 100 + 800 * (node.Value % (numOfNodes / 2)) / (numOfNodes / 2);
                embedding[node] = new Point(left, top);
            }
            return new GraphEmbedding(graph, embedding);
        }

        static Func<int, int, double> InCliqueFunction(int numOfNodes, int numOfCliques, double insideCliqueWeight, double outsideCliqueWeight) => (n1, n2) =>
        {
            var devideBy = numOfNodes / numOfCliques;
            if (((n1 - 1) / devideBy) == (n2 - 1) / devideBy)
                return insideCliqueWeight;
            return outsideCliqueWeight;
        };


        static void Main(string[] args)
        {
      //      CreateEmbedding(20, (i, j) => Math.Min(i, j)).WriteTo(@"C:\Users\Yuval\Desktop\MinGraph20.txt");
      //      CreateEmbedding(40, (i, j) => Math.Min(i, j)).WriteTo(@"C:\Users\Yuval\Desktop\MinGraph40.txt");
      //      CreateEmbedding(60, (i, j) => Math.Min(i, j)).WriteTo(@"C:\Users\Yuval\Desktop\MinGraph60.txt");
      //      CreateEmbedding(80, (i, j) => Math.Min(i, j)).WriteTo(@"C:\Users\Yuval\Desktop\MinGraph80.txt");

      //      CreateEmbedding(16, (i, j) => Math.Max(i, j)).WriteTo(@"C:\Users\Yuval\Desktop\MaxGraph16.txt");
      //      CreateEmbedding(32, (i, j) => Math.Max(i, j)).WriteTo(@"C:\Users\Yuval\Desktop\MaxGraph32.txt");
      //      CreateEmbedding(48, (i, j) => Math.Max(i, j)).WriteTo(@"C:\Users\Yuval\Desktop\MaxGraph48.txt");

      //      CreateEmbedding(20, InCliqueFunction(20, 4, 2, 5)).WriteTo(@"C:\Users\Yuval\Desktop\Clique20Nodes4Cliques2Inside5Outside.txt");
       //     CreateEmbedding(36, InCliqueFunction(36, 4, 1, 8)).WriteTo(@"C:\Users\Yuval\Desktop\Clique36Nodes4Cliques1Inside8Outside.txt");
      //      CreateEmbedding(20, InCliqueFunction(20, 5, 1, 6)).WriteTo(@"C:\Users\Yuval\Desktop\Clique20Nodes5Cliques1Inside6Outside.txt");


            BiDirectional(40, 1).WriteTo(@"C:\Users\Yuval\Desktop\BiDirectional40.txt");
            BiDirectional(32, 1).WriteTo(@"C:\Users\Yuval\Desktop\BiDirectional32.txt");
        }
    }
}
