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
        public static Dictionary<K,V> Clone<K,V>(this Dictionary<K, V> @this)
        {
            var newDict = new Dictionary<K, V>();
            foreach (var key in @this.Keys)
                newDict[key] = @this[key];
            return newDict;
        }
        public static Dictionary<K,V> RemoveKeys<K,V>(this Dictionary<K, V> @this, IEnumerable<K> excludedKeys)
        {
            var newDict = @this.Clone();
            foreach (var excludedKey in excludedKeys)
                newDict.Remove(excludedKey);

            return newDict;
        }
    }
}
