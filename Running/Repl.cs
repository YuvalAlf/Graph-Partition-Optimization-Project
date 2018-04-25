using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Running.ReplStates;

namespace Running
{
    public class Repl
    {
        private ReplState ReplState { get; set; }

        public Repl(ReplState replState)
        {
            ReplState = replState;
        }

        public static Repl Create()
        {
            return new Repl(new InitReplState());
        }

        public void Run()
        {
            while(true)
                ReplState = ReplState.Oparate();
        }
    }
}
