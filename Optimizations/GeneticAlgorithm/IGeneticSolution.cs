using System;

namespace Optimizations.GeneticAlgorithm
{
    public interface IGeneticSolver<Solution> : INegativePrice
    {
        Solution MateMutate(Solution otherSolution, double mutationRate, Random rnd);
    }
}
