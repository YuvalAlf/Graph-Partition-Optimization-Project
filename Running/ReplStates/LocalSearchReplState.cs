using System;
using System.IO;
using Graphs.Algorithms;
using Graphs.GraphProperties;
using Optimizations.LocalSearchAlgorithm;
using Utils.DataStructures;
using Utils.ExtensionMethods;

namespace Running.ReplStates
{
    public sealed class LocalSearchReplState : ReplState
    {
        public Graph Graph { get; }

        public LocalSearchReplState(Graph graph)
        {
            Graph = graph;
        }

        public override ReplState Oparate()
        {
            var solutionPath = ResultsDirPath.CombinePathWith("LocalSearch_" + DateTime.Now.Ticks);
            Directory.CreateDirectory(solutionPath);
            Graph.WriteToFile(solutionPath.CombinePathWith("Graph.txt"));
            Console.WriteLine("Enter amount of runnings of parallel Local Searches:");
            var amountInParrallel = Parsing.ParseInt(1, 10000, _ => true, "");
            var localSearchSettings = new LocalSearchSettings(amountInParrallel);
            var localSearch = new LocalSearch<GraphPartitionSolution>();
            var killTask = DistributedInt.Init();
            localSearch.RunAsync(GraphPartitionSolution.GenerateRandom(Graph), localSearchSettings, killTask, ReportSolution(solutionPath), Random);
            return new PendEndingReplState(killTask);
        }

        private Action<GraphPartitionSolution> ReportSolution(string solutionPath) => solution =>
            {
                solution.WriteToFile(solutionPath.CombinePathWith(solution.NegativePrice + ".txt"));
            };
    }

    public sealed class PendEndingReplState : ReplState
    {
        public DistributedInt KillTask { get; }

        public PendEndingReplState(DistributedInt killTask)
        {
            KillTask = killTask;
        }

        public override ReplState Oparate()
        {
            Console.Write("To End Running, Press Any Key: ");
            Console.ReadKey(false);
            KillTask.AddOne();
            KillTask.WaitForValue(0);
            Console.WriteLine();
            Console.WriteLine("-----------------------------------------");
            return new InitReplState();
        }
    }
}