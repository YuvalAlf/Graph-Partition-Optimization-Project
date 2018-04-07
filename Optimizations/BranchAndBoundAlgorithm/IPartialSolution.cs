using System;
using System.Collections.Generic;

namespace Optimizations.BranchAndBoundAlgorithm
{
    public interface IPartialSolution<PartialSolution, Solution> : IComparable<PartialSolution>
    {
        Solution ConstructSolution(Random rnd);
        IEnumerable<PartialSolution> Children();
        double MinBound { get; }
    }
}