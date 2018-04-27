using System;
using System.Collections.Generic;
using System.Text;
using Graphs.Algorithms;
using Utils.IO;

namespace Running.ReplStates
{
    public abstract class ReplState
    {
        public Random Random = new Random();

        public abstract ReplState Oparate();

        public string ResultsDirPath => @"C:\Users\Yuval\Desktop\GraphPartitionResults";

        protected Action<GraphPartitionSolution> ReportSolution(string solutionPath) => solution =>
        {
            solution.WriteToFile(solutionPath.CombinePathWith(solution.NegativePrice + ".txt"));
        };

        protected static T Choose<T>(string message, params (string, char, Func<T>)[] optionalInputs)
        {
            var str = new StringBuilder(message + " :");
            var chToFunc = new Dictionary<char, Func<T>>();
            foreach (var (m, ch, func) in optionalInputs)
            {
                chToFunc[ch] = func;
                str.Append(" #" + ch + "# for " + m + " /");
            }
            str.Remove(str.Length - 1, 1);
            ColorWriter.PrintCyan(str.ToString());
            var chosenChar = Parsing.ParseChar(chToFunc.Keys);
            return chToFunc[chosenChar].Invoke();
        }

    }
}