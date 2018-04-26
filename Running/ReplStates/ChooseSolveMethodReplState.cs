using Graphs.GraphProperties;
using Utils;

namespace Running.ReplStates
{
    public sealed class ChooseSolveMethodReplState : ReplState
    {
        public Graph Graph { get; }

        public ChooseSolveMethodReplState(Graph graph)
        {
            Graph = graph;
        }

        public override ReplState Oparate()
        {
            var defaultSettings = Choose("Settings", ("Custom settings", 'C', () => false), ("Default settings", 'D', () => true));

            return Choose("Choose",
                ("Genetic", 'G', () => new GeneticReplState(Graph, defaultSettings).TypeCast<ReplState>()),
                ("Local Search", 'L', () => new LocalSearchReplState(Graph, defaultSettings).TypeCast<ReplState>()),
                ("Branch & Bound", 'B', () => new BranchAndBoundReplState(Graph, defaultSettings).TypeCast<ReplState>()));
        }
    }
}
