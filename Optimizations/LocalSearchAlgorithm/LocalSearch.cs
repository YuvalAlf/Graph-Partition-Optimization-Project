using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Utils.DataStructures;

namespace Optimizations.LocalSearchAlgorithm
{
    public sealed class LocalSearch<Solution> : OptimizationSolver<Solution, LocalSearchSettings>
        where Solution : ILocalSearchSolver<Solution>
    {
        public override IEnumerable<Solution> Run(Func<Random, Solution> genRandom, LocalSearchSettings settings, object runPauseLock,
            ConcurrentSignal killTaskSignal, ConcurrentSignal taskKilledSignal, StrongBox<bool> finished, Random rnd)
        {
            Solution bestSolution = genRandom(rnd);
            double bestPrice = bestSolution.NegativePrice;
            yield return bestSolution;
            
            foreach (var neighbor in bestSolution.Neighbors())
            {
                var proccessedSingnal = killTaskSignal.TryProcessSignal();
                finished.Value = !proccessedSingnal;
                if (proccessedSingnal)
                    break;
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
            }
                
            taskKilledSignal.Signal();
        }
    }
}
