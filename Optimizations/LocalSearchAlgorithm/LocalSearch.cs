using System;
using Utils.DataStructures;

namespace Optimizations.LocalSearchAlgorithm
{
    public sealed class LocalSearch<Solution> : OptimizationSolver<Solution, LocalSearchSettings>
        where Solution : ILocalSearchSolver<Solution>
    {
        public override void Run(Func<Random, Solution> genRandom, LocalSearchSettings settings, DistributedInt killTask, Action<Solution> reportSolution, Random rnd)
        {
            (Solution newBestSolution, double newBestPrice, bool shouldFinish) Loop(Solution bestSolution, double bestPrice)
            {
                foreach (var neighbor in bestSolution.Neighbors())
                {
                    if (killTask.Num == 1)
                        return (bestSolution, bestPrice, true);
                        var neighborPrice = neighbor.NegativePrice;
                        if (neighborPrice < bestPrice)
                            return (neighbor, neighborPrice, false);
                }
                return (bestSolution, bestPrice, true);
            }

            Solution globalBestSolution = genRandom(rnd);
            double globalBestPrice = globalBestSolution.NegativePrice;
            reportSolution(globalBestSolution);
            while (true)
            {
                (Solution newBestSolution, double newBestPrice, bool shouldFinish) = Loop(globalBestSolution, globalBestPrice);
                if (shouldFinish)
                {
                    killTask.MinusOne();
                    return;
                }

                if (newBestPrice < globalBestPrice)
                {
                    globalBestPrice = newBestPrice;
                    globalBestSolution = newBestSolution;
                    reportSolution(globalBestSolution);
                }
            }
        }
    }
}
