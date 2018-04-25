using System;
using System.Linq;

namespace Running
{
    public static class Parsing
    {
        private static T Error<T>(string message, Func<T> callback)
        {
            ColorWriter.PrintRed("#" + message + "#");
            return callback();
        }
        
        public static char ParseChar(string optionalChars)
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
    }
}
