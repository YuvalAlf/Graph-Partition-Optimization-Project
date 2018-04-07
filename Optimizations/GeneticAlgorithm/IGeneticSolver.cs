using System;

namespace Optimizations.GeneticAlgorithm
{
    public interface IGeneticSolver<Solution> : INegativePrice
    {
        Solution Mate(Solution otherSolution, Random rnd);
        Solution Mutate(Random rnd);
    }
}
