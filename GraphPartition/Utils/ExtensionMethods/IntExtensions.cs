using System;
using System.Diagnostics;
using System.Linq;

namespace Utils.ExtensionMethods
{
    public static class IntExtensions
    {
        public static string AsString(this int @this, int digits)
        {
            var digitsToPad = digits - @this.CountDigits();
            digitsToPad = Math.Max(0, digitsToPad);
            return Enumerable.Repeat('0', digitsToPad).Concat(@this.ToString()).AsString();
        }

        public static int CountDigits(this int num)
        {
            Debug.Assert(num >= 0);
            if (num <= 9)
                return 1;
            return 1 + (num / 10).CountDigits();
        }

        public static string AsPaddedString(this int num)
        {
            if (num.CountDigits() <= 1)
                return num.ToString();
            return " " + num + " ";
        }
    }
}
