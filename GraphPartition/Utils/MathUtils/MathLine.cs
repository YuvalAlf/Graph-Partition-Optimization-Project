using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        public double DistanceFrom(Point point)
        {
            var x = point.X;
            var y = point.Y;
            var A = x - X1;
            var B = y - Y1;
            var C = X2 - X1;
            var D = Y2 - Y1;

            var dot = A * C + B * D;
            var len_sq = C * C + D * D;
            var param = -1.0;
            if (len_sq != 0) //in case of 0 length line
                param = dot / len_sq;

            var xx = 0.0;
            var yy = 0.0;

            if (param < 0)
            {
                xx = Y1;
                yy = Y1;
            }
            else if (param > 1)
            {
                xx = X2;
                yy = Y2;
            }
            else
            {
                xx = X1 + param * C;
                yy = Y1 + param * D;
            }

            var dx = x - xx;
            var dy = y - yy;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public double Compute(double x) => X1 == X2 ? (Y1 + Y2) / 2.0 : x * M + N;
    }
}
