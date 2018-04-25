using System;

namespace Running.ReplStates
{
    public abstract class ReplState
    {
        public Random Random = new Random();

        public abstract ReplState Oparate();

        public string ResultsDirPath => @"C:\Users\Yuval\Desktop\GraphPartitionResults";

    }
}