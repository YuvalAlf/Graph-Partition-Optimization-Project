using System;

namespace Optimizations.GeneticAlgorithm
{
    public interface IGeneticSolution<Solution>
    {
        double NegativePrice { get; }
        Solution MateMutate(Solution otherSolution, double mutationRate, Random rnd);
    }
}
