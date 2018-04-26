using System;
using System.IO;
using Graphs.Algorithms;
using Graphs.Algorithms.BranchAndBound;
using Graphs.GraphProperties;
using Optimizations.BranchAndBoundAlgorithm;
using Utils.DataStructures;
using Utils.ExtensionMethods;

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

            var branchAndBoundSettings = BranchAndBoundSettings<UpperBound>.Default;
            var branchAndBound = new BranchAndBound<PartialGraphPartition, GraphPartitionSolution, UpperBound>(PartialGraphPartition.CreateEmpty(Graph));
            var killTask = DistributedInt.Init();
            branchAndBound.RunAsync(GraphPartitionSolution.GenerateRandom(Graph), branchAndBoundSettings, killTask, ReportSolution(solutionPath), Random);
            return new PendEndingReplState(killTask);
        }
    }
}