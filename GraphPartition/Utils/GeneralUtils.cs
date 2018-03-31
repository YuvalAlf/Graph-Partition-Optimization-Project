﻿using System;

namespace Utils
{
    public static class GeneralUtils
    {
        public static (int min, int max) MinMax(int a, int b)
        {
            return (Math.Min(a, b), Math.Max(a, b));
        }
        public static (double min, double max) MinMax(double a, double b)
        {
            return (Math.Min(a, b), Math.Max(a, b));
        }
    }
}
