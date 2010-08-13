using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPSO_2007
{
    static class Extensions
    {
        public static double NextDouble(this Random rand,double lowerBound,double upperBound)
        {
            return lowerBound + rand.NextDouble() * (upperBound - lowerBound);
        }
        public static Velocity NextVector(this Random rand,int dimensions, double coeff)
        {
            Velocity velocity = new Velocity();
            int K = 2; // 1 => uniform distribution in a hypercube
            // 2 => "triangle" distribution
            velocity.size = dimensions;

            for (int d = 0; d < dimensions; d++)
            {
                double rnd = 0.0;
                for (int i = 0; i < K; i++)
                {
                    rnd += rand.NextDouble();
                }
                velocity.v[d] = rnd * coeff / K;
            }

            return velocity;
        }
        public static void Shuffle(this int[] index,int count,int max)
        {
            Random rand = new Random();
            for (int i = 0; i < count; i++) //Shuffle count times
            {
                for (int j = 0; j < max; j++)
                {
                    int r = rand.Next(max);
                    int temp = index[j];
                    index[j] = index[r];
                    index[r] = temp;

                }
            }
        }
    }
}
