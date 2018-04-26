using System;
using DIBRIS.Hippie;
using MoreLinq;
using Utils.DataStructures;

namespace Optimizations.BranchAndBoundAlgorithm
{
    public sealed class BranchAndBound<PartialSolution, Solution, UpperBound> : OptimizationSolver<Solution, BranchAndBoundSettings<UpperBound>>
        where Solution : INegativePrice
        where PartialSolution : IPartialSolution<PartialSolution, UpperBound, Solution>
    {
        public PartialSolution EmptyPartialSolution { get; }

        public BranchAndBound(PartialSolution emptyPartialSolution) => EmptyPartialSolution = emptyPartialSolution;


        public override void Run(Func<Random, Solution> genRandom, BranchAndBoundSettings<UpperBound> settings, DistributedInt killTask, Action<Solution> reportSolution, Random rnd)
        {
            var priorityQueue = HeapFactory.NewArrayHeap<PartialSolution>(2);
            priorityQueue.Add(EmptyPartialSolution);

            var globalMinBound = double.PositiveInfinity;
            var bestSolutionNegativePrice = double.PositiveInfinity;

            while (priorityQueue.Count > 0 && killTask.Num == 0)
            {
                var nextItem = priorityQueue.RemoveMin();
                var itemMinBound = nextItem.MinBound;
                globalMinBound = Math.Min(itemMinBound, globalMinBound);
                if (itemMinBound >= bestSolutionNegativePrice)
                    continue;
                nextItem.Children().ForEach(priorityQueue.Add);
                var solution = nextItem.ConstructSolution(settings.UpperBoundScheme, rnd);
                var solutionPrice = solution.NegativePrice;
                if (solutionPrice < bestSolutionNegativePrice)
                {
                    bestSolutionNegativePrice = solutionPrice;
                    reportSolution(solution);
                }
            }

            killTask.MinusOne();
        }
    }
}
