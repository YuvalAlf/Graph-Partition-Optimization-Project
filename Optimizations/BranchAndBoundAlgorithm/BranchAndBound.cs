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
        public Dictionary<int, int> LevelExpensions = new Dictionary<int, int>();
        public Dictionary<int, int> LevelCutoffs = new Dictionary<int, int>();
        public BranchAndBound(PartialSolution emptyPartialSolution) => EmptyPartialSolution = emptyPartialSolution;


        public override void Run(Func<Random, Solution> genRandom, BranchAndBoundSettings<UpperBound> settings, DistributedInt killTask, Action<Solution> reportSolution, Random rnd)
        {
            AmountOfSolutions = 0;
            for (int i = 0; i < 50; i++)
            {
                LevelExpensions[i] = 0;
                LevelCutoffs[i] = 0;
            }

            var priorityQueue = new Stack<(int, PartialSolution)>();
            priorityQueue.Push((0, EmptyPartialSolution));

            var bestSolutionNegativePrice = double.PositiveInfinity;

            int loopTime = 0;
            while (priorityQueue.Count > 0 && killTask.Num == 0)
            {
                loopTime++;
                if (loopTime % 10000 == 0)
                    Console.WriteLine(loopTime);

                var (level, nextItem) = priorityQueue.Pop();
                var itemMinBound = nextItem.MinBound;
                AmountOfSolutions++;
                if (itemMinBound >= bestSolutionNegativePrice)
                {
                    LevelCutoffs[level]++;
                    continue;
                }
                nextItem.Children().ForEach(child => priorityQueue.Push((level + 1, child)));
                LevelExpensions[level]++;
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
