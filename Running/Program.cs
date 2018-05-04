using System;
using System.Drawing;
using Graphs.Algorithms;
using Graphs.Algorithms.BranchAndBound;
using Graphs.Algorithms.LocalSearch;
using Graphs.GraphProperties;
using Optimizations.BranchAndBoundAlgorithm;
using Optimizations.LocalSearchAlgorithm;
using Utils.DataStructures;
using static Graphs.Algorithms.LocalSearch.GraphPartitionNeighborhoodOption;

namespace Running
{
    class Program
    {
        static Random rnd = new Random();

        static void Main(string[] args)
        {
            Colorful.Console.WriteAscii("Graph Partiton", Color.CornflowerBlue);

            // Repl.Create().Run();

            RunLocalSearch();
            //RunBranchAndBound();
        }

        static void RunLocalSearch()
        {
            var numOfNodes = 100;
            var graph = GraphBuilder.Create4CliqueGraph(numOfNodes);

            void Run(GraphPartitionNeighborhoodOption neighborhoodOption)
            {
                var start = DateTime.Now;
                Console.WriteLine(neighborhoodOption.AsString() + ":");
                var localSearch = new LocalSearch<GraphPartitionSolution, GraphPartitionNeighborhoodOption>();
                var ending = DateTime.Now;
                localSearch.SingleRun(neighborhoodOption, GraphPartitionSolution.GenerateRandom(graph), DistributedInt.Init(), s =>
                {
                    Console.WriteLine(s.NegativePrice);
                    ending = DateTime.Now;
                }, rnd);
                Console.WriteLine("Running time: " + (ending - start).TotalMinutes + " minutes");
            }
            
            //Run(OneSwap);
            Run(CircularSwap);
            Run(TwoSwaps);
            Run(AllTogether);
        }
        static void RunBranchAndBound()
        {
            for (int numOfNodes = 4; numOfNodes <= 16; numOfNodes += 4)
            {
                var graph = GraphBuilder.CreateMaxGraph(numOfNodes);

                var bAndBGreedy = new BranchAndBoundSettings<UpperBoundScheme>(UpperBoundScheme.GreedySolution);
                var bAndBRandomized = new BranchAndBoundSettings<UpperBoundScheme>(UpperBoundScheme.RandomizedSolution);
                var greedy = new BranchAndBound<PartialGraphPartition, GraphPartitionSolution, UpperBoundScheme>(PartialGraphPartition.CreateEmpty(graph, rnd));
                var randomized = new BranchAndBound<PartialGraphPartition, GraphPartitionSolution, UpperBoundScheme>(PartialGraphPartition.CreateEmpty(graph, rnd));

                greedy.Run(GraphPartitionSolution.GenerateRandom(graph), bAndBGreedy, DistributedInt.Init(), _ => { }, rnd);
                Console.WriteLine(numOfNodes + "\tGreedy\t" + greedy.AmountOfSolutions + "\titems");

                randomized.Run(GraphPartitionSolution.GenerateRandom(graph), bAndBRandomized, DistributedInt.Init(), _ => { }, rnd);
                Console.WriteLine(numOfNodes + "\tRandom\t" + randomized.AmountOfSolutions + "\titems");
            }
        }
    }
}
