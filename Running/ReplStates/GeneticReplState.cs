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

            var geneticSettings = DefaultSettings ? GeneticSettings<MatingScheme, GraphPartitionNeighborhoodOption, SelectionScheme>.Default : GetSettings();
            var genetic = new Genetic<GraphPartitionSolution, MatingScheme, GraphPartitionNeighborhoodOption, SelectionScheme>();
            var killTask = DistributedInt.Init();
            genetic.RunAsync(GraphPartitionSolution.GenerateRandom(Graph), geneticSettings, killTask, ReportSolution(solutionPath), Random);
            return new PendEndingReplState(killTask);
        }

        private static GeneticSettings<MatingScheme, GraphPartitionNeighborhoodOption, SelectionScheme> GetSettings()
        {
            ColorWriter.PrintCyan("Enter #population#:");
            var population = Parsing.ParseInt(4, 2000, _ => true, "");
            ColorWriter.PrintCyan("Enter #mutation rate#:");
            var mutationRate = Parsing.ParseDouble(0.0, 1.0);
            ColorWriter.PrintCyan("Enter #elitism rate#:");
            var elitismRate = Parsing.ParseDouble(0.0, 1.0);
            ColorWriter.PrintCyan("Enter #new genes rate#:");
            var newGenesRate = Parsing.ParseDouble(0.0, 1.0);


            var matingScheme = Choose("Choose #mating# scheme",
                ("intersection-mating", 'I', () => IntersectionMating),
                ("copy-one-partition", 'C',  () => CopyOnePartition));

            var neighborsScheme = Choose("Choose #mutation# scheme",
                ("one-swap", '1',      () => OneSwap),
                ("two-swap", '2',      () => TwoSwaps),
                ("circular-swap", 'C', () => CircularSwap));

            var selectionScheme = Choose("Choose #selection# scheme",
                ("equal", '=',                 () => SelectionScheme.Equal),
                ("inversed", '1',              () => SelectionScheme.Inversed),
                ("inversed squarred", '2',     () => SelectionScheme.InversedSquarred),
                ("inversed cubed", '3',        () => SelectionScheme.InversedCubed),
                ("inversed log", '7',          () => SelectionScheme.InversedLog),
                ("inversed log squarred", '8', () => SelectionScheme.InversedLogSquarred),
                ("inversed log cubed", '9',    () => SelectionScheme.InversedLogCubed));

            return new GeneticSettings<MatingScheme, GraphPartitionNeighborhoodOption, SelectionScheme>(population, mutationRate,
                elitismRate, newGenesRate, matingScheme, neighborsScheme, selectionScheme);
        }
    }
}