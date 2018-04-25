using System;
using System.Xml;
using GraphCreator;
using Graphs.GraphProperties;

namespace Running.ReplStates
{
    public sealed class InitReplState : ReplState
    {
        public override ReplState Oparate()
        {
            ColorWriter.PrintCyan("Choose #amount# of nodes");
            var amountOfNodes = Parsing.ParseInt(4, 100000, x => x%4 == 0, "Input has to be divdable by 4");
            ColorWriter.PrintCyan("Choose graph type: #N# for min / #X# for max / #R# for randomized");
            var type = Parsing.ParseChar("NXR");
            var graph = GetGraph(amountOfNodes, type);
            return new ChooseSolveMethodReplState(graph);
        }

        private Graph GetGraph(int amountOfNodes, char type)
        {
            switch (type)
            {
                case 'N':
                    return GraphBuilder.CreateMinGraph(amountOfNodes);
                case 'X':
                    return GraphBuilder.CreateMaxGraph(amountOfNodes);
                case 'R':
                    return GraphBuilder.CreateRandomizedGraph(amountOfNodes);
                default:
                    throw new ArgumentException();
            }
        }
    }
}