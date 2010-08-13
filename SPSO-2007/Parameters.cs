namespace SPSO_2007
{
    public class Parameters
    {
        public double c;		// Confidence coefficient
        public int clamping;	// Position clamping or not
        public int K;			// Max number of particles informed by a given one
        public double p;		// Probability threshold for random topology	
        // (is actually computed as p(S,K) )
        public int randOrder;	// Random choice of particles or not
        public int rand; // 0 => use KISS. Any other value: use the system RNG
        public int initLink; // How to re-init links
        public int rotation;	// Sensitive to rotation or not
        public int S;			// Swarm size
        public int stop;		// Flag for stop criterion
        public double w;		// Confidence coefficient
    };
}