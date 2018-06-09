using System;
using System.Collections.Generic;
using DIBRIS.Hippie;
using MoreLinq;
using Utils.DataStructures;

namespace Optimizations.BranchAndBoundAlgorithm
{
    public sealed class BranchAndBound<PartialSolution, Solution, UpperBound> : OptimizationSolver<Solution, BranchAndBoundSettings<UpperBound>>
        where Solution : INegativePrice
        where PartialSolution : IPartialSolution<PartialSolution, UpperBound, Solution>
    {
        private PartialSolution EmptyPartialSolution { get; }
        public int AmountOfSolutions = 0;
        public BranchAndBound(PartialSolution emptyPartialSolution) => EmptyPartialSolution = emptyPartialSolution;


        public override void Run(Func<Random, Solution> genRandom, BranchAndBoundSettings<UpperBound> settings, DistributedInt killTask, Action<Solution> reportSolution, Random rnd)
        {
            AmountOfSolutions = 0;
            var priorityQueue = new Stack<PartialSolution>();
            priorityQueue.Push(EmptyPartialSolution);

            var bestSolutionNegativePrice = double.PositiveInfinity;

            while (priorityQueue.Count > 0 && killTask.Num == 0)
            {
                var nextItem = priorityQueue.Pop();
                var itemMinBound = nextItem.MinBound;
                AmountOfSolutions++;
                if (itemMinBound >= bestSolutionNegativePrice)
                    continue;
                nextItem.Children().ForEach(priorityQueue.Push);
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
