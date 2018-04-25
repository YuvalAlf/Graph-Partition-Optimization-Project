using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Utils.DataStructures;

namespace Optimizations
{
    public abstract class OptimizationSolver<SolutionType, Settings> : IComparer<SolutionType>
        where SolutionType : INegativePrice
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Compare(SolutionType s1, SolutionType s2) => s1.NegativePrice.CompareTo(s2.NegativePrice);

        public void RunAsync(Func<Random, SolutionType> genRandom, Settings settings, DistributedInt killTask, Action<SolutionType> reportSolution, Random rnd)
        {
            Task.Run(() => Run(genRandom, settings, killTask, reportSolution, rnd));
        }

        public abstract void Run(Func<Random, SolutionType> genRandom, Settings settings, DistributedInt killTask, Action<SolutionType> reportSolution, Random rnd);
    }
}
