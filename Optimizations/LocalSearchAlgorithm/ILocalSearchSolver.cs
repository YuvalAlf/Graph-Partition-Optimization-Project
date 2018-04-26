using System;
using System.Collections.Generic;

namespace Optimizations.LocalSearchAlgorithm
{
    public interface ILocalSearchSolver<Solution, NeighborhoodOptions> : INegativePrice
    {
        IEnumerable<Solution> Neighbors(Random rnd, NeighborhoodOptions options);
    }
}
