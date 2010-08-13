using System.Linq;

namespace SPSO_2007
{
    public class Swarm
    {
        public Swarm() : this(910) { }
        public Swarm(int maxSwarmSize)
        {
            P = new Position[maxSwarmSize];
            X = new Position[maxSwarmSize];
            V = new Velocity[maxSwarmSize];
            for (int i = 0; i < maxSwarmSize; i++)
            {
                P[i] = new Position();
                X[i] = new Position();
                V[i] = new Velocity();
            }
            //P = Enumerable.Repeat(new Position(), maxSwarmSize).ToArray();
            //X = Enumerable.Repeat(new Position(), maxSwarmSize).ToArray();
            //V = Enumerable.Repeat(new Velocity(), maxSwarmSize).ToArray();
        }
        public int best; 					// rank of the best particle
        public Position[] P;	// Previous best positions found by each particle
        public int S; 						// Swarm size 
        public Velocity[] V;	// Velocities
        public Position[] X;	// Positions 
    }
}