using Running.ReplStates;

namespace Running
{
    class Program
    {
        static void Main(string[] args)
        {
            var repl = new Repl(new InitReplState());
            repl.Run();
        }
    }
}
