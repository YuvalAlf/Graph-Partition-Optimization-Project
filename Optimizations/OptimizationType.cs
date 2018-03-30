using System;

namespace Optimizations
{
    public enum OptimizationType
    {
        Genetic,
        BranchAndBound,
        LocalSearch
    }

    public static class OptimizationTypeUtils
    {
        public static OptimizationType[] All = {OptimizationType.Genetic, OptimizationType.BranchAndBound, OptimizationType.LocalSearch};

        public static string AsString(this OptimizationType @this)
        {
            switch (@this)
            {
                case OptimizationType.Genetic:
                    return "Genetic Algorithm";
                case OptimizationType.BranchAndBound:
                    return "Branch & Bound";
                case OptimizationType.LocalSearch:
                    return "Local Search";
                default:
                    throw new ArgumentOutOfRangeException(nameof(@this), @this, null);
            }
        }
    }
}
