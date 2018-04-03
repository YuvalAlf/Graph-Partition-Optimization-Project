using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Optimizations
{
    public sealed class SolverRunner<Solution>
    {
        public Task<IEnumerable<Solution>> RunningTask { get; }
        public Dispatcher Dispatcher { get; }
        public Action<Solution> FoundSolution { get; }

        public static SolverRunner<Solution> CreateAndRun(Func<Func<bool>, IEnumerable<Solution>> solver, Dispatcher dispatcher, Action<Solution> foundSolution, Func<bool> shouldStop)
        {
           // var solutions = solver(shouldStop);
            throw new NotImplementedException();
        }



        public void Run()
        {
        }

        public void Pause()
        {

        }


    }
}
