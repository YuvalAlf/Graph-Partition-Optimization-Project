using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utils.DataStructures;

namespace Optimizations.LocalSearchAlgorithm
{
    public sealed class LocalSearch<Solution> : OptimizationSolver<Solution, LocalSearchSettings>
        where Solution : ILocalSearch<Solution>
    {
        public override IEnumerable<Solution> Run(Func<Random, Solution> genRandom, LocalSearchSettings settings, object runPauseLock,
            ConcurrentSignal killTaskSignal, ConcurrentSignal taskKilledSignal, Random rnd)
        {
            Solution bestSolution = genRandom(rnd);
            double bestPrice = bestSolution.NegativePrice;
            yield return bestSolution;

            while (!killTaskSignal.TryProcessSignal())
                foreach (var neighbor in bestSolution.Neighbors())
                    lock (runPauseLock)
                    {
                        var neighborPrice = neighbor.NegativePrice;
                        if (neighborPrice < bestPrice)
                        {
                            bestSolution = neighbor;
                            bestPrice = neighborPrice;
                            yield return bestSolution;
                        }
                    }
            taskKilledSignal.Signal();
        }
    }
}
