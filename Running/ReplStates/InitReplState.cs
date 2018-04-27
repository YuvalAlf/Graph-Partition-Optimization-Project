using Graphs.GraphProperties;
using Utils;
using Utils.IO;

namespace Running.ReplStates
{
    public sealed class InitReplState : ReplState
    {
        private static int GetAmountOfNodes()
        {
            ColorWriter.PrintCyan("Choose #amount# of nodes");
            return Parsing.ParseInt(4, 100000, x => x % 4 == 0, "Input has to be divdable by 4");
        }

        private static bool GetWhetherDefaultSettings()
            => Choose("Settings", ("Custom settings", 'C', () => false), ("Default settings", 'D', () => true));

        private static Graph ParseGraphFromFile()
        {
            ColorWriter.PrintCyan("Enter graph #path#:");
            return Graph.ParseFromPath(Parsing.ParseString(Graph.CanParseFromPath, "Path is invalid"));
        }

        public override ReplState Oparate()
        {
            var graph = Choose("Choose graph type",
                ("min", 'N',          () => GraphBuilder.CreateMinGraph(GetAmountOfNodes())),
                ("max", 'X',          () => GraphBuilder.CreateMaxGraph(GetAmountOfNodes())),
                ("randomized", 'R',   () => GraphBuilder.CreateRandomizedGraph(GetAmountOfNodes())),
                ("four cliques", '4', () => GraphBuilder.Create4CliqueGraph(GetAmountOfNodes())),
                ("path", 'P',         ParseGraphFromFile));
            
            return Choose("Choose",
                ("Genetic", 'G', () => new GeneticReplState(graph, GetWhetherDefaultSettings()).TypeCast<ReplState>()),
                ("Local Search", 'L', () => new LocalSearchReplState(graph, GetWhetherDefaultSettings()).TypeCast<ReplState>()),
                ("Branch & Bound", 'B', () => new BranchAndBoundReplState(graph, GetWhetherDefaultSettings()).TypeCast<ReplState>()));
        }

    }
}