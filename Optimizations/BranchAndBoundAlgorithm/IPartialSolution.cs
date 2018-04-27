using System;
using System.Collections.Generic;

namespace Optimizations.BranchAndBoundAlgorithm
{
    public interface IPartialSolution<PartialSolution, in UpperBoundScheme, out Solution> : IComparable<PartialSolution>
    {
        Solution ConstructSolution(UpperBoundScheme upperBoundScheme, Random rnd);
        IEnumerable<PartialSolution> Children();
        double MinBound { get; }
    }
}