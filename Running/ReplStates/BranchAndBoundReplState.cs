using System;
using System.IO;
using Graphs.Algorithms;
using Graphs.Algorithms.BranchAndBound;
using Graphs.GraphProperties;
using Optimizations.BranchAndBoundAlgorithm;
using Utils.DataStructures;
using Utils.IO;

namespace Running.ReplStates
{
    public sealed class BranchAndBoundReplState : ReplState
    {
        public Graph Graph { get; }
        public bool DefaultSettings { get; }

        public BranchAndBoundReplState(Graph graph, bool defaultSettings)
        {
            Graph = graph;
            DefaultSettings = defaultSettings;
        }

        public override ReplState Oparate()
        {
            var solutionPath = ResultsDirPath.CombinePathWith("BranchAndBound_" + DateTime.Now.Ticks);
            Directory.CreateDirectory(solutionPath);
            Graph.WriteToFile(solutionPath.CombinePathWith("Graph.txt"));
            
            var branchAndBoundSettings = GetSettings();
            var branchAndBound = new BranchAndBound<PartialGraphPartition, GraphPartitionSolution, UpperBoundScheme>(PartialGraphPartition.CreateEmpty(Graph, Random));
            var killTask = DistributedInt.Init();
            branchAndBound.RunAsync(GraphPartitionSolution.GenerateRandom(Graph), branchAndBoundSettings, killTask, ReportSolution(solutionPath), Random);
            return new PendEndingReplState(killTask);
        }

        private BranchAndBoundSettings<UpperBoundScheme> GetSettings()
        {
            var @default = BranchAndBoundSettings<UpperBoundScheme>.Default;

            var upperBoundScheme = Choose("Choose #upper bound# scheme", () => @default.UpperBoundScheme,
                ("Greedy", 'G', () => UpperBoundScheme.GreedySolution),
                ("R", 'R', () => UpperBoundScheme.RandomizedSolution));

            return new BranchAndBoundSettings<UpperBoundScheme>(upperBoundScheme);
        }
    }
}