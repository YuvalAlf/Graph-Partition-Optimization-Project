using System;

namespace Optimizations.GeneticAlgorithm
{
    public interface IGeneticSolver<Solution, Mating, Mutation> : INegativePrice
    {
        Solution Mate(Mating mating, Solution otherSolution, Random rnd);
        Solution Mutate(Mutation mutation, Random rnd);
    }
}
