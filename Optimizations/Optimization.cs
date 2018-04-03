using System.Collections.Generic;

namespace Optimizations
{
    public abstract class Optimization<OptimizationSettings, Result>
    {
        public abstract IEnumerable<Result> Run(OptimizationSettings optimizationSettings);
    }
}
