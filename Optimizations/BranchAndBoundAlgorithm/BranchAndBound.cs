using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DIBRIS.Hippie;
using MoreLinq;
using Utils.DataStructures;

namespace Optimizations.BranchAndBoundAlgorithm
{
    public sealed class BranchAndBound<PartialSolution, Solution> : OptimizationSolver<Solution, BranchAndBoundSettings>
        where Solution : INegativePrice
        where PartialSolution : IPartialSolution<PartialSolution, Solution>
    {
        public PartialSolution EmptyPartialSolution { get; }

        public BranchAndBound(PartialSolution emptyPartialSolution) => EmptyPartialSolution = emptyPartialSolution;



        public override IEnumerable<Solution> Run(Func<Random, Solution> genRandom, BranchAndBoundSettings settings, object runPauseLock
            , ConcurrentSignal killTaskSignal, ConcurrentSignal taskKilledSignal, StrongBox<bool> finishedExecution, Random rnd)
        {
            var priorityQueue = HeapFactory.NewArrayHeap<PartialSolution>(2);
            priorityQueue.Add(EmptyPartialSolution);

            var globalMinBound = double.PositiveInfinity;
            var bestSolutionNegativePrice = double.PositiveInfinity;

            while (priorityQueue.Count > 0 && !killTaskSignal.TryProcessSignal())
            {
                var nextItem = priorityQueue.RemoveMin();
                var itemMinBound = nextItem.MinBound;
                globalMinBound = Math.Min(itemMinBound, globalMinBound);
                if (itemMinBound >= bestSolutionNegativePrice)
                    continue;
                nextItem.Children().ForEach(priorityQueue.Add);
                var solution = nextItem.ConstructSolution(rnd);
                var solutionPrice = solution.NegativePrice;
                if (solutionPrice < bestSolutionNegativePrice)
                {
                    bestSolutionNegativePrice = solutionPrice;
                    yield return solution;
                }
            }

            finishedExecution.Value = priorityQueue.Count == 0;

            taskKilledSignal.Signal();
        }
    }
}
