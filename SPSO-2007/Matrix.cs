using System;

namespace SPSO_2007
{
    public class Matrix 	// Useful for "non rotation sensitive" option
    {
        public Matrix():this(114){}
        public Matrix(int maxDimensions)
        {
            v = new double[maxDimensions,maxDimensions];
        }
        public int size;
        public double[,] v;
        public static Matrix MatrixProduct(Matrix M1, Matrix M2)
        {
            // Two square matrices of same size
            var product = new Matrix();
            for (int i = 0; i < M1.size; i++)
            {
                for (int j = 0; j < M1.size; j++)
                {
                    double sum = 0.0;
                    for (int k = 0; k < M1.size; k++)
                    {
                        sum += M1.v[i, k] * M2.v[k, j];
                    }
                    product.v[i, j] = sum;
                }
            }
            product.size = M1.size;
            return product;
        }
        public static Matrix MatrixRotation(Velocity V)
        {
            /*
             Define the matrice of the rotation V' => V
             where V'=(1,1,...1)*normV/Math.Sqrt(D)  (i.e. norm(V') = norm(V) )

             */
            int D = V.size;
            //matrix reflex1; // Global variable
            double normV = Velocity.normL(V, 2);
            var reflex2 = new Matrix { size = D };
            var reflex1 = new Matrix {size = D};

            // Reflection relatively to the vector V'=(1,1, ...1)/Math.Sqrt(D)	
            // norm(V')=1
            for (int i = 0; i < D; i++)
            {
                for (int j = 0; j < D; j++)
                {
                    reflex1.v[i, j] = -2.0 /D;
                }
            }
            for (int d = 0; d < D; d++)
            {
                reflex1.v[d, d] = 1 + reflex1.v[d, d];
            }

            //Define the "bisectrix" B of (V',V) as an unit vector
            var B = new Velocity { size = D };
            for (int d = 0; d < D; d++)
            {
                B.v[d] = V.v[d] + normV / Math.Sqrt(D);
            }
            double normB = Velocity.normL(B, 2);

            if (normB > 0)
            {
                for (int d = 0; d < D; d++)
                {
                    B.v[d] = B.v[d] / normB;
                }
            }

            // Reflection relatively to B
            for (int i = 0; i < D; i++)
            {
                for (int j = 0; j < D; j++)
                {
                    reflex2.v[i, j] = -2 * B.v[i] * B.v[j];
                }
            }

            for (int d = 0; d < D; d++)
            {
                reflex2.v[d, d] = 1 + reflex2.v[d, d];
            }

            // Multiply the two reflections
            // => rotation				
            return MatrixProduct(reflex2, reflex1);

        }

        public Velocity VectorProduct(Velocity V)
        {
            var velocity = new Velocity();
            for (int d = 0; d < V.size; d++)
            {
                double sum = 0;
                for (int j = 0; j < V.size; j++)
                {
                    sum = sum + this.v[d, j] * V.v[j];
                }
                velocity.v[d] = sum;
            }
            velocity.size = V.size;
            return velocity;
        }


    };
}