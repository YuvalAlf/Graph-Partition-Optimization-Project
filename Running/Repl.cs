using Running.ReplStates;

namespace Running
{
    public class Repl
    {
        private ReplState ReplState { get; set; }

        public Repl(ReplState replState) => ReplState = replState;

        public static Repl Create() => new Repl(new InitReplState());

        public void Run()
        {
            while(true)
                ReplState = ReplState.Oparate();
        }
    }
}
