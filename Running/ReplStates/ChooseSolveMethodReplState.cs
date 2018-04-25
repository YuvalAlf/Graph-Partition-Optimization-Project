using System;
using Graphs.GraphProperties;

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
            ColorWriter.PrintCyan("Choose: #G# for Genetic / #L# for Local Search / #B# for Branch & Bound");
            var ch = Parsing.ParseChar("GLB");
            switch (ch)
            {
                case 'G':
                    return new GeneticReplState();
                case 'L':
                    return new LocalSearchReplState(Graph);
                case 'B':
                    return new BranchAndBoundReplState();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
