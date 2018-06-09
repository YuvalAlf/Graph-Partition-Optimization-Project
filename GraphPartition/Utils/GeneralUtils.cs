using System;

namespace Utils
{
    public static class GeneralUtils
    {
        public static S TypeCast<S>(this object @this) => (S)@this;

        public static (int min, int max) MinMax(int a, int b) => (Math.Min(a, b), Math.Max(a, b));

        public static (double min, double max) MinMax(double a, double b) => (Math.Min(a, b), Math.Max(a, b));


        public static double Choose(int n, int k)
        {
            if (n <= k || k <= 0)
                return 1;
            return ((double)n / k) * Choose(n - 1, k - 1);
        }

        public static void SwapValues<T>(ref T t1, ref T t2)
        {
            T temp = t1;
            t1 = t2;
            t2 = temp;
        }
    }
}
