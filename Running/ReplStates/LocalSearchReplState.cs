using System;
using System.IO;
using Graphs.Algorithms;
using Graphs.Algorithms.LocalSearch;
using Graphs.GraphProperties;
using Optimizations.LocalSearchAlgorithm;
using Utils.DataStructures;
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

            var localSearchSettings = DefaultSettings ? LocalSearchSettings<GraphPartitionNeighborhoodOption>.Default : GetSettings(); 
            var localSearch = new LocalSearch<GraphPartitionSolution, GraphPartitionNeighborhoodOption>();
            var killTask = DistributedInt.Init();
            localSearch.RunAsync(GraphPartitionSolution.GenerateRandom(Graph), localSearchSettings, killTask, ReportSolution(solutionPath), Random);
            return new PendEndingReplState(killTask);
        }

        private LocalSearchSettings<GraphPartitionNeighborhoodOption> GetSettings()
        {
            var @default = LocalSearchSettings<GraphPartitionNeighborhoodOption>.Default;
            ColorWriter.PrintCyan("Enter amount of runnings of #parallel Local Searches#:");
            var amountInParrallel = Parsing.ParseInt(1, 10000, _ => true, "", @default.AmountInParralel);

            var neighbosOption = Choose("Enter #neiborhood# option", () => @default.NeighborsOption,
                ("one-swap", '1', () => OneSwap),
                ("two-swap", '2', () => TwoSwaps),
                ("circular-swap", 'C', () => CircularSwap));

            return new LocalSearchSettings<GraphPartitionNeighborhoodOption>(amountInParrallel, neighbosOption);
        }
    }
}