namespace SPSO_2007
{
    public class SwarmSize
    {
        public SwarmSize():this(114)
        {}
        public SwarmSize(int maxDimensions)
        {
            max = new double[maxDimensions];
            maxInit = new double[maxDimensions];
            min = new double[maxDimensions];
            minInit = new double[maxDimensions];
            q=new Quantum(maxDimensions);
        }
        public int D;
        public double[] max;
        public double[] maxInit;
        public double[] min;
        public double[] minInit;
        public Quantum q;		// Quantisation step size. 0 => continuous problem
    };
}