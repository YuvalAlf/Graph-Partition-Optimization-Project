using System.Collections.Generic;

namespace Utils.DataStructures
{
    public static class EphemeralExtensions
    {
        public static IEnumerable<T> MultiPop<T>(this Stack<T> @this, int amount)
        {
            for (int i = 0; i < amount; i++)
                yield return @this.Pop();
        }
    }
}
