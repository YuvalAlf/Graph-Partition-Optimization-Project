using System.Diagnostics;

namespace Utils.ExtensionMethods
{
    public static class IntExtensions
    {
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
                return " " + num + " ";
            return num.ToString();
        }
    }
}
