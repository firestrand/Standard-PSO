namespace SPSO_2007
{
    public class Quantum
    {
        public Quantum():this(114)
        {}
        public Quantum(int maxDimensions)
        {
            q = new double[maxDimensions];
        }
        public double[] q;
        public int size;
    };
}