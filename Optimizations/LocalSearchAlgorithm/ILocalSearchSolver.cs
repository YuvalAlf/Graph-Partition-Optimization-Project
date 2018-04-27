using System;
using System.Collections.Generic;

namespace Optimizations.LocalSearchAlgorithm
{
    public interface ILocalSearchSolver<out Solution, in NeighborhoodOptions> : INegativePrice
    {
        IEnumerable<Solution> Neighbors(Random rnd, NeighborhoodOptions options);
    }
}
