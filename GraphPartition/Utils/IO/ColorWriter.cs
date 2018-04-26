using System;

namespace Utils.IO
{
    public static class ColorWriter
    {
        public static void PrintCyan(string str) => PrintLineColor(str, ConsoleColor.Cyan);
        public static void PrintRed(string str) => PrintLineColor(str, ConsoleColor.Red);

        public static void PrintLineColor(string str, ConsoleColor color)
        {
            var beginColor = Console.ForegroundColor;
            foreach (var ch in str)
            {
                if (ch == '#')
                    Console.ForegroundColor = Console.ForegroundColor == color ? beginColor : color;
                else
                    Console.Write(ch);
            }
            Console.WriteLine();
            Console.ForegroundColor = beginColor;
        }
    }
}
