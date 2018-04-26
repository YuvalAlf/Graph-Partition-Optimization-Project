using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils.IO
{
    public static class Parsing
    {
        private static T Error<T>(string message, Func<T> callback)
        {
            ColorWriter.PrintRed("#" + message + "#");
            return callback();
        }
        
        public static char ParseChar(IEnumerable<char> optionalChars)
        {
            Func<char> callback = () => ParseChar(optionalChars);
            var str = Console.ReadLine();
            if (str.Length != 1)
                return Error("You have to type a char", callback);
            if (!optionalChars.Contains(str[0]))
                return Error("Char is not optional, choose: " + string.Join(" / ", optionalChars), callback);

            return str[0];
        }

        public static int ParseInt(int min, int max, Predicate<int> predicate, string errorMsg)
        {
            Func<int> callback = () => ParseInt(min, max, predicate, errorMsg);
            var str = Console.ReadLine();
            if (int.TryParse(str, out int num))
            {
                if (num < min)
                    return Error("Input is bellow " + min, callback);
                if (num > max)
                    return Error("Input is above " + max, callback);
                if (!predicate(num))
                    return Error(errorMsg, callback);
                return num;
            }
            return Error("Input isn't an integer", callback);
        }

        public static double ParseDouble(double min, double max)
        {
            Func<double> callback = () => ParseDouble(min, max);
            var str = Console.ReadLine();
            if (double.TryParse(str, out double num))
            {
                if (num < min)
                    return Error("Input is bellow " + min, callback);
                if (num > max)
                    return Error("Input is above " + max, callback);
                return num;
            }
            return Error("Input isn't a floating-point value", callback);
        }

        public static string ParseString(Predicate<string> predicate, string errorMsg)
        {
            Func<string> callback = () => ParseString(predicate, errorMsg);
            var str = Console.ReadLine();
            if (predicate(str))
                return str;
            return Error(errorMsg, callback);
        }
    }
}
