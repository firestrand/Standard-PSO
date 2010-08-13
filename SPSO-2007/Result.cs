namespace SPSO_2007
{
    public class Result
    {
        public Result():this(910){}
        public Result(int maxSwarmSize)
        {
            SW=new Swarm(maxSwarmSize);
        }
        public double nEval; 		// Number of evaluations  
        public Swarm SW;	// Final swarm
        public double error;		// Numerical result of the run
    };
}