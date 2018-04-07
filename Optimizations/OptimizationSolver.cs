using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Utils.DataStructures;

namespace Optimizations
{
    public abstract class OptimizationSolver<SolutionType, Settings> : IComparer<SolutionType>
        where SolutionType : INegativePrice
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Compare(SolutionType s1, SolutionType s2) => s1.NegativePrice.CompareTo(s2.NegativePrice);

        public abstract IEnumerable<SolutionType> Run(Func<Random, SolutionType> genRandom, Settings settings,
            object runPauseLock, ConcurrentSignal killTaskSignal, ConcurrentSignal taskKilledSignal, StrongBox<bool> finishedExecution, Random rnd);
    }
}
