using System;
using System.Threading.Tasks;
using MoreLinq;
using Utils.DataStructures;
using Utils.ExtensionMethods;

namespace Optimizations.LocalSearchAlgorithm
{
    public sealed class LocalSearch<Solution, NeighborhoodOptions> : OptimizationSolver<Solution, LocalSearchSettings<NeighborhoodOptions>>
        where Solution : ILocalSearchSolver<Solution, NeighborhoodOptions>
    { 
        public override void Run(Func<Random, Solution> genRandom, LocalSearchSettings<NeighborhoodOptions> settings, DistributedInt killTask, Action<Solution> reportSolution, Random rnd)
        {
            var kills = ArrayExtensions.InitArray(settings.AmountInParralel, _ => DistributedInt.Init());
            var bestPrice = double.PositiveInfinity;
            Solution globalMinimum = default(Solution);

            void reportSolutionGlobally(Solution s)
            {
                if (s.NegativePrice < bestPrice)
                {
                    bestPrice = s.NegativePrice;
                    globalMinimum = s;
                    reportSolution(s);
                }
            }


            Task LocalSearchRun(int index) => Task.Run(() =>
            {
                SingleRun(settings.NeighborsOption, genRandom, kills[index], reportSolutionGlobally, rnd);
            });

            var tasks = ArrayExtensions.InitArray(settings.AmountInParralel, LocalSearchRun);

            killTask.WaitForValue(1);
            killTask.MinusOne();
            kills.ForEach(d => d.AddOne());
            kills.ForEach(d => d.WaitForValue(0));
        }


        private void SingleRun(NeighborhoodOptions neighbors, Func<Random, Solution> genRandom, DistributedInt killTask, Action<Solution> reportSolution, Random rnd)
        {
            (Solution newBestSolution, bool shouldFinish) Loop(Solution bestSolution)
            {
                foreach (var neighbor in bestSolution.Neighbors(rnd, neighbors))
                {
                    if (killTask.Num == 1)
                        return (bestSolution, true);
                    if (neighbor.NegativePrice < bestSolution.NegativePrice)
                        return (neighbor, false);
                }
                return (bestSolution, true);
            }

            Solution globalBestSolution = genRandom(rnd);
            reportSolution(globalBestSolution);
            while (true)
            {
                (Solution newBestSolution, bool shouldFinish) = Loop(globalBestSolution);
                if (shouldFinish)
                {
                    killTask.MinusOne();
                    return;
                }

                globalBestSolution = newBestSolution;
                reportSolution(globalBestSolution);
            }
        }
    }
}
