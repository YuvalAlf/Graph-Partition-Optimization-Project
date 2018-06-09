using System;
using System.Drawing;
using Graphs.Algorithms;
using Graphs.Algorithms.BranchAndBound;
using Graphs.GraphProperties;
using Optimizations.BranchAndBoundAlgorithm;
using Utils.DataStructures;
using Utils.IO;

namespace Running
{
    class Program
    {
        static Random rnd = new Random();

        static void Main(string[] args)
        {
           // Colorful.Console.WriteAscii("Graph Partiton", Color.CornflowerBlue);
            
            // Repl.Create().Run();

            var rnd = new Random(12323);
            Console.WriteLine("Type,Graph Nodes,Tree Nodes,Sec Time,Result Price");
            for (int size = 36; size <= 44; size += 4)
            {
                var graph = GraphBuilder.Create4CliqueGraph(size);
                var bAndB = new BranchAndBound<PartialGraphPartition, GraphPartitionSolution, UpperBoundScheme>(PartialGraphPartition.CreateEmpty(graph, rnd));
                var settings = new BranchAndBoundSettings<UpperBoundScheme>(UpperBoundScheme.GreedySolution);
                var start = DateTime.Now;
                GraphPartitionSolution result = null;
                bAndB.Run(GraphPartitionSolution.GenerateRandom(graph), settings, DistributedInt.Init(), s => result = s, rnd);
                var ending = DateTime.Now;
                var time = ending - start;
                Console.WriteLine($"Greedy,{size},{bAndB.AmountOfSolutions},{time.TotalSeconds},{result.NegativePrice}");
            }

            Console.ReadKey(false);
        }
    }
}
