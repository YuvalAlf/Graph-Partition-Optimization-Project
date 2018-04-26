using System;
using System.Drawing;
using System.Linq;
using Utils.DataStructures;
using Utils.IO;

namespace Running.ReplStates
{
    public sealed class PendEndingReplState : ReplState
    {
        public DistributedInt KillTask { get; }

        public PendEndingReplState(DistributedInt killTask) => KillTask = killTask;

        public override ReplState Oparate()
        {
            ColorWriter.PrintCyan("To #End Running#, Press Any Key: ");
            Console.ReadKey(false);
            KillTask.AddOne();
            KillTask.WaitForValue(0);
            Console.WriteLine();
            Colorful.Console.Write("---------------------------------------------", Color.Blue);
            Console.WriteLine();
            return new InitReplState();
        }
    }
}