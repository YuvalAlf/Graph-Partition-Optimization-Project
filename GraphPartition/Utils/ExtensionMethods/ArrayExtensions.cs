using System;
using System.Linq;

namespace Utils.ExtensionMethods
{
    public static class ArrayExtensions
    {
        public static T[] Copy<T>(this T[] @this) 
            => @this.Clone() as T[];

        public static T ChooseRandomly<T>(this T[] @this, Random rnd) 
            => @this[rnd.Next(@this.Length)];

        public static T[] InitArray<T>(int length, Func<int, T> initilizor)
            => Enumerable.Range(0, length).Select(initilizor).ToArray();
    }
}
