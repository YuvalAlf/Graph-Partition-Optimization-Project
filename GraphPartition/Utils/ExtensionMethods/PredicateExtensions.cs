using System;

namespace Utils.ExtensionMethods
{
    public static class PredicateExtensions
    {
        public static Predicate<T> And<T>(this Predicate<T> p1, Predicate<T> p2)
        {
            return t => p1(t) && p2(t);
        }
    }
}
