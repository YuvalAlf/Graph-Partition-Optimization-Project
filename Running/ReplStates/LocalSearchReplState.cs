using System;
using System.IO;
using Graphs.Algorithms;
using Graphs.Algorithms.LocalSearch;
using Graphs.GraphProperties;
using Optimizations.LocalSearchAlgorithm;
using Utils.DataStructures;
using Utils.ExtensionMethods;
using Utils.IO;
using static Graphs.Algorithms.LocalSearch.GraphPartitionNeighborhoodOption;

namespace Running.ReplStates
{
    public sealed class LocalSearchReplState : ReplState
    {
        public Graph Graph { get; }
        public bool DefaultSettings { get; }

        public LocalSearchReplState(Graph graph, bool defaultSettings)
        {
            Graph = graph;
            DefaultSettings = defaultSettings;
        }

        public override ReplState Oparate()
        {
            var solutionPath = ResultsDirPath.CombinePathWith("LocalSearch_" + DateTime.Now.Ticks);
            Directory.CreateDirectory(solutionPath);
            Graph.WriteToFile(solutionPath.CombinePathWith("Graph.txt"));

            var localSearchSettings = GetSettings(); 
            var localSearch = new LocalSearch<GraphPartitionSolution, GraphPartitionNeighborhoodOption>();
            var killTask = DistributedInt.Init();
            localSearch.RunAsync(GraphPartitionSolution.GenerateRandom(Graph), localSearchSettings, killTask, ReportSolution(solutionPath), Random);
            return new PendEndingReplState(killTask);
        }

        private LocalSearchSettings<GraphPartitionNeighborhoodOption> GetSettings()
        {
            var amountInParrallel = 1;
            var neighbosOption = OneSwap;

            if (!DefaultSettings)
            {
                ColorWriter.PrintCyan("Enter amount of runnings of #parallel Local Searches#:");
                amountInParrallel = Parsing.ParseInt(1, 10000, _ => true, "");

                neighbosOption = Choose("Enter #neiborhood# option",
                    ("one-swap", '1', () => OneSwap),
                    ("two-swap", '2', () => TwoSwaps),
                    ("circular-swap", 'C', () => CircularSwap));

            }
            return new LocalSearchSettings<GraphPartitionNeighborhoodOption>(amountInParrallel, neighbosOption);
        }
    }
}