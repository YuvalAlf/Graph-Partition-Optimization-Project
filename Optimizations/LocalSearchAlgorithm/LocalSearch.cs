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
            ConcurrentSignal killTaskSignal, ConcurrentSignal taskKilledSignal, StrongBox<bool> finishedExecution, Random rnd)
        {
            (Solution newBestSolution, double newBestPrice, bool finishedBySignal, bool finishedByExecution) Loop(Solution bestSolution, double bestPrice)
            {
                foreach (var neighbor in bestSolution.Neighbors())
                {
                    if (killTaskSignal.TryProcessSignal())
                        return (bestSolution, bestPrice, true, false);
                    lock (runPauseLock)
                    {
                        var neighborPrice = neighbor.NegativePrice;
                        if (neighborPrice < bestPrice)
                            return (neighbor, neighborPrice, false, false);
                    }
                }
                return (bestSolution, bestPrice, false, true);
            }

            Solution globalBestSolution = genRandom(rnd);
            double globalBestPrice = globalBestSolution.NegativePrice;
            yield return globalBestSolution;
            while (true)
            {
                (Solution newBestSolution, double newBestPrice, bool finishedBySignal, bool finishedByExecution) =
                    Loop(globalBestSolution, globalBestPrice);
                if (finishedByExecution)
                    finishedExecution.Value = true;
                if (finishedBySignal || finishedByExecution)
                    break;
                if (newBestPrice < globalBestPrice)
                {
                    globalBestPrice = newBestPrice;
                    globalBestSolution = newBestSolution;
                    yield return globalBestSolution;
                }
            }
            taskKilledSignal.Signal();
        }
    }
}
