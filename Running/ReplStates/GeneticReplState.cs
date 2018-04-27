using System;
using System.IO;
using Graphs.Algorithms;
using Graphs.Algorithms.Genetic;
using Graphs.Algorithms.LocalSearch;
using Graphs.GraphProperties;
using Optimizations.GeneticAlgorithm;
using Utils.DataStructures;
using Utils.IO;
using static Graphs.Algorithms.Genetic.MatingScheme;
using static Graphs.Algorithms.LocalSearch.GraphPartitionNeighborhoodOption;

namespace Running.ReplStates
{
    public sealed class GeneticReplState : ReplState
    {
        public Graph Graph { get; }
        public bool DefaultSettings { get; }

        public GeneticReplState(Graph graph, bool defaultSettings)
        {
            Graph = graph;
            DefaultSettings = defaultSettings;
        }

        public override ReplState Oparate()
        {
            var solutionPath = ResultsDirPath.CombinePathWith("Genetic_" + DateTime.Now.Ticks);
            Directory.CreateDirectory(solutionPath);
            Graph.WriteToFile(solutionPath.CombinePathWith("Graph.txt"));

            var geneticSettings = DefaultSettings ? GeneticSettings<MatingScheme, GraphPartitionNeighborhoodOption>.Default : GetSettings();
            var genetic = new Genetic<GraphPartitionSolution, MatingScheme, GraphPartitionNeighborhoodOption>();
            var killTask = DistributedInt.Init();
            genetic.RunAsync(GraphPartitionSolution.GenerateRandom(Graph), geneticSettings, killTask, ReportSolution(solutionPath), Random);
            return new PendEndingReplState(killTask);
        }

        private static GeneticSettings<MatingScheme, GraphPartitionNeighborhoodOption> GetSettings()
        {
            ColorWriter.PrintCyan("Enter #population#:");
            var population = Parsing.ParseInt(4, 2000, _ => true, "");
            ColorWriter.PrintCyan("Enter #mutation rate#:");
            var mutationRate = Parsing.ParseDouble(0.0, 1.0);
            ColorWriter.PrintCyan("Enter #elitism rate#:");
            var elitismRate = Parsing.ParseDouble(0.0, 1.0);
            ColorWriter.PrintCyan("Enter #new genes rate#:");
            var newGenesRate = Parsing.ParseDouble(0.0, 1.0);


            var matingScheme = Choose("Enter #mating# scheme",
                ("intersection-mating", 'I', () => IntersectionMating),
                ("copy-one-partition", 'C', () => CopyOnePartition));

            var neighborsScheme = Choose("Enter #mutation# scheme",
                ("one-swap", '1', () => OneSwap),
                ("two-swap", '2', () => TwoSwaps),
                ("circular-swap", 'C', () => CircularSwap));

            return new GeneticSettings<MatingScheme, GraphPartitionNeighborhoodOption>(population, mutationRate,
                elitismRate, newGenesRate, matingScheme, neighborsScheme);
        }
    }
}