using System.Collections.Generic;

namespace Optimizations.LocalSearchAlgorithm
{
    public interface ILocalSearch<Solution> : INegativePrice
    {
        IEnumerable<Solution> Neighbors();
    }
}
