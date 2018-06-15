using System;
using System.Drawing;
using System.Linq;
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
            Colorful.Console.WriteAscii("Graph Partiton", Color.CornflowerBlue);
            
            //Repl.Create().Run();

            var amountOfNodes = 24;
            var graph = GraphBuilder.CreateSumGraph(amountOfNodes);
            var bAndB =
                new BranchAndBound<PartialGraphPartition, GraphPartitionSolution, UpperBoundScheme>(
                    PartialGraphPartition.CreateEmpty(graph, rnd));
            bAndB.Run(GraphPartitionSolution.GenerateRandom(graph), BranchAndBoundSettings<UpperBoundScheme>.Default, DistributedInt.Init(), _ => {}, rnd);
            Console.WriteLine("Amount of solutions: " + bAndB.AmountOfSolutions);
            Console.WriteLine("Expensions: ");
            Console.WriteLine(bAndB.LevelExpensions.AsString());
            Console.WriteLine("Cutoffs: ");
            Console.WriteLine(bAndB.LevelCutoffs.AsString());
        }
    }
}
