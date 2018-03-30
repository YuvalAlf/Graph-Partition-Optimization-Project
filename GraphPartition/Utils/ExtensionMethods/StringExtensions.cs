using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string AsString(this IEnumerable<char> @this)
        {
            return new string(@this.ToArray());
        }

        public static Predicate<string> IsDouble()
        {
            return str =>
            {
                double res;
                return double.TryParse(str, out res);
            };
        }
        public static Predicate<string> IsInt()
        {
            return str =>
            {
                int res;
                return int.TryParse(str, out res);
            };
        }

        public static Predicate<string> InRange(double min, double max)
        {
            return str =>
            {
                var num = double.Parse(str);
                return num >= min && num <= max;
            };
            
        }
    }
}