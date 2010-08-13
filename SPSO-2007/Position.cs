using System;

namespace SPSO_2007
{
    public class Position
    {
        public Position():this(114)
        {
        }
        public Position(int maxDimensions)
        {
            x = new double[maxDimensions];
            f = double.MaxValue;
        }
        public double f;
        public int improved;
        public int size;
        public double[] x;
        public Position Clone()
        {
            Position retVal = new Position();
            retVal.f = this.f;
            retVal.improved = this.improved;
            retVal.size = this.size;
            this.x.CopyTo(retVal.x,0);
            return retVal;
        }

        public static double distanceL(Position x1, Position x2, double L)
        {  // Distance between two positions
            // L = 2 => Euclidean	
            int d;
            double n;

            n = 0;

            for (d = 0; d < x1.size; d++)
                n = n + Math.Pow(Math.Abs(x1.x[d] - x2.x[d]), L);

            n = Math.Pow(n, 1 / L);
            return n;
        }

        public static void quantis(Position position, SwarmSize SS)
        {
            /*
             Quantisatition of a position
             Only values like x+k*q (k integer) are admissible 
             */
            int d;
            double qd;
            for (d = 0; d < position.size; d++)
            {
                qd = SS.q.q[d];

                if (qd > 0.0)	// Note that qd can't be < 0
                {
                    //qd = qd * (SS.max[d] - SS.min[d]) / 2;	      
                    position.x[d] = qd * Math.Floor(0.5 + position.x[d] / qd);
                }
            }
        }
    };
}