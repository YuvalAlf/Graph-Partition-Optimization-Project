using System;

namespace Optimizations.GeneticAlgorithm
{
    public interface IGeneticSolver<Solution, in Mating, in Mutation, Selection> : INegativePrice
    {
        Solution Mate(Mating mating, Solution otherSolution, Random rnd);
        Solution Mutate(Mutation mutation, Random rnd);
        Func<Random, (Solution, Solution)> SelectionMethod(Selection selection, Solution[] population);
    }
}
