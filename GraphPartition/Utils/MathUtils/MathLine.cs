using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.MathUtils
{
    public sealed class MathLine
    {
        public double X1 { get; }
        public double X2 { get; }
        public double Y1 { get; }
        public double Y2 { get; }
        public double M { get; }
        public double N { get; }

        private MathLine(double x1, double x2, double y1, double y2, double m, double n)
        {
            X1 = x1;
            X2 = x2;
            Y1 = y1;
            Y2 = y2;
            M = m;
            N = n;
        }

        public static MathLine Create(double x1, double x2, double y1, double y2)
        {
            var m = (y2 - y1) / (x2 - x1);
            var n = -m * x1 + y1;
            return new MathLine(x1, x2, y1, y2, m, n);
        }

        public double Compute(double x) => x * M + N;
    }
}
