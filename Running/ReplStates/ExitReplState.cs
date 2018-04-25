using System;

namespace Running.ReplStates
{
    public sealed class ExitReplState : ReplState
    {
        public override ReplState Oparate()
        {
            throw new InvalidOperationException();
        }
    }
}