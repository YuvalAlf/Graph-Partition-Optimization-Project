using System.Collections.Generic;

namespace Optimizations.LocalSearchAlgorithm
{
    public interface ILocalSearchSolver<Solution> : INegativePrice
    {
        IEnumerable<Solution> Neighbors();
    }
}
