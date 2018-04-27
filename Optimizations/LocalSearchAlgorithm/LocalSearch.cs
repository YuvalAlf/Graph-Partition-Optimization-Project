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

            void ReportSolutionGlobally(Solution s)
            {
                if (s.NegativePrice < bestPrice)
                {
                    bestPrice = s.NegativePrice;
                    reportSolution(s);
                }
            }


            Task LocalSearchRun(int index) => Task.Run(() =>
            {
                SingleRun(settings.NeighborsOption, genRandom, kills[index], ReportSolutionGlobally, rnd);
            });

            var tasks = ArrayExtensions.InitArray(settings.AmountInParralel, LocalSearchRun);

            killTask.WaitForValue(1);
            kills.ForEach(d => d.AddOne());
            kills.ForEach(d => d.WaitForValue(0));
            killTask.MinusOne();
        }


        private void SingleRun(NeighborhoodOptions neighbors, Func<Random, Solution> genRandom, DistributedInt killTask, Action<Solution> reportSolution, Random rnd)
        {
            (Solution nextSolution, bool shouldFinish) Loop(Solution currentSolution)
            {
                foreach (var neighbor in currentSolution.Neighbors(rnd, neighbors))
                {
                    if (killTask.Num == 1)
                        return (currentSolution, true);
                    if (neighbor.NegativePrice < currentSolution.NegativePrice)
                        return (neighbor, false);
                }
                return (currentSolution, true);
            }

            Solution initialSolution = genRandom(rnd);
            reportSolution(initialSolution);
            while (true)
            {
                (Solution nextSolution, bool shouldFinish) = Loop(initialSolution);
                if (shouldFinish)
                {
                    killTask.MinusOne();
                    return;
                }

                initialSolution = nextSolution;
                reportSolution(initialSolution);
            }
        }
    }
}
