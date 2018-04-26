using System;
using System.IO;
using Graphs.Algorithms;
using Graphs.Algorithms.Genetic;
using Graphs.Algorithms.LocalSearch;
using Graphs.GraphProperties;
using Optimizations.GeneticAlgorithm;
using Utils.DataStructures;
using Utils.ExtensionMethods;
using Utils.IO;
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

            var geneticSettings = GetSettings();
            var genetic = new Genetic<GraphPartitionSolution, Mating, GraphPartitionNeighborhoodOption>();
            var killTask = DistributedInt.Init();
            genetic.RunAsync(GraphPartitionSolution.GenerateRandom(Graph), geneticSettings, killTask, ReportSolution(solutionPath), Random);
            return new PendEndingReplState(killTask);
        }

        private GeneticSettings<Mating, GraphPartitionNeighborhoodOption> GetSettings()
        {
            var population = 20;
            var mutationRate = 0.6;
            var elitismRate = 0.35;
            var newGenesRate = 0.1;
            var mating = default(Mating);
            var neighborsOption = OneSwap;

            if (!DefaultSettings)
            {
                ColorWriter.PrintCyan("Enter population:");
                population = Parsing.ParseInt(4, 2000, _ => true, "");
                ColorWriter.PrintCyan("Enter #mutation rate#:");
                mutationRate = Parsing.ParseDouble(0.0, 1.0);
                ColorWriter.PrintCyan("Enter #elitism rate#:");
                elitismRate = Parsing.ParseDouble(0.0, 1.0);
                ColorWriter.PrintCyan("Enter #new genes rate#:");
                newGenesRate = Parsing.ParseDouble(0.0, 1.0);


                neighborsOption = Choose("Enter neiborhood option",
                    ("one-swap", '1', () => OneSwap),
                    ("two-swap", '2', () => TwoSwaps),
                    ("circular-swap", 'C', () => CircularSwap));

            }

            return new GeneticSettings<Mating, GraphPartitionNeighborhoodOption>(population, mutationRate, elitismRate, newGenesRate, mating, neighborsOption);
        }
    }
}