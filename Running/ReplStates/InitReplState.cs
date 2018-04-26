using GraphCreator;
using Graphs.GraphProperties;
using Utils.IO;

namespace Running.ReplStates
{
    public sealed class InitReplState : ReplState
    {
        public override ReplState Oparate()
        {
            ColorWriter.PrintCyan("Choose #amount# of nodes");

            var amountOfNodes = Parsing.ParseInt(4, 100000, x => x%4 == 0, "Input has to be divdable by 4");

            var graph = Choose("Choose graph type",
                ("min", 'N', () => GraphBuilder.CreateMinGraph(amountOfNodes)),
                ("max", 'X', () => GraphBuilder.CreateMaxGraph(amountOfNodes)),
                ("randomized", 'R', () => GraphBuilder.CreateRandomizedGraph(amountOfNodes)),
                ("path", 'P', ParseGraphFromFile));

            return new ChooseSolveMethodReplState(graph);
        }

        private Graph ParseGraphFromFile()
        {
            ColorWriter.PrintCyan("Enter graph #path#:");
            return Graph.ParseFromPath(Parsing.ParseString(Graph.CanParseFromPath, "Path is invalid"));
        }
    }
}