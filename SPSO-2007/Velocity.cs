using System;

namespace SPSO_2007
{
    public class Velocity
    {
        public Velocity():this(114)
        {}
        public Velocity(int maxDimension){v=new double[maxDimension];}
        public int size;
        public double[] v;

        public static double normL(Velocity v, double L)
        {   // L-norm of a vector
            int d;
            double n;

            n = 0;

            for (d = 0; d < v.size; d++)
                n = n + Math.Pow(Math.Abs(v.v[d]), L);

            n = Math.Pow(n, 1 / L);
            return n;
        }
    };
}