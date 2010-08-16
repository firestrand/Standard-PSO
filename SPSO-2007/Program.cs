using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/*
Standard PSO 2007
 Contact for remarks, suggestions etc.:
 Maurice.Clerc@WriteMe.com
 
 Last update
 2010-08-13 PORT to C#
 2010-06-15 Fixed a small bug in position initialisation for discrete problems (thanks to Yue,Shuai)
 2010-03-24 Lennard-Jones problem
 2010-02-06 A few functions of the CEC 2005 benchmark
 2010-01-04 Fixed wrong fitness evaluation for G3 (function code 10)
 2009-12-29 Random number generator KISS (option). For more reproducible results
 2009-07-12 The initialisation space may be smaller than the search space (for tests)
 2009-06-03 Fixed a small mistake about the first best position
 2009-05-05 Step function
 2009-04-19 Schaffer f6, 2D Goldstein-Price
 2009-03-31 A small network optimisation 
 2009-03-12 Schwefel 2.2, Neumaier 3, G3 (constrained)
 2008-10-02 Two Schwefel functions
 2008-08-12 For information: save the best position over all runs
 2007-12-10 Warning about rotational invariance (valid here only on 2D)
 2007-11-22 stop criterion (option): distance to solution < epsilon
            and log_progress evaluation
 2007-11-21 Ackley function
 
  -------------------------------- Contributors 
 The works and comments of the following persons have been taken
 into account while designing this standard.  Sometimes this is for 
 including a feature, and sometimes for leaving out one. 
 
 Auger, Anne
 Blackwell, Tim
 Bratton, Dan
 Clerc, Maurice
 Croussette, Sylvain 
 Dattasharma, Abhi
 Eberhart, Russel
 Hansen, Nikolaus
 Keko, Hrvoje
 Kennedy, James 
 Krohling, Renato
 Langdon, William
 Li, Wentao
 Liu, Hongbo 
 Miranda, Vladimiro
 Poli, Riccardo
 Serra, Pablo
 Stickel, Manfred
 
 -------------------------------- Motivation
Quite often, researchers claim to compare their version of PSO 
with the "standard one", but the "standard one" itself seems to vary!
Thus, it is important to define a real standard that would stay 
unchanged for at least one year.
This PSO version does not intend to be the best one on the market
(in particular, there is no adaptation of the swarm size nor of the
coefficients). This is simply very near to the original version (1995),
with just a few improvements based on some recent works.
 --------------------------------- Metaphors
swarm: A team of communicating people (particles)
At each time step
    Each particle chooses a few informants at random, selects the best
    one from this set, and takes into account the information given by
    the chosen particle.
    If it finds no particle better than itself, then the "reasoning" is:
    "I am the best, so I just take my current velocity and my previous
    best position into account" 
----------------------------------- Parameters/Options
clamping := true/false => whether to use clamping positions or not
randOrder:= true/false => whether to avoid the bias due to the loop
                on particles "for s = 1 to swarm_size ..." or not
rotation := true/false => whether the algorithm is sensitive 
                to a rotation of the landscape or not 
You may also modify the following ones, although suggested values
are either hard coded or automatically computed:
S := swarm size
K := maximum number of particles _informed_ by a given one
w := first cognitive/confidence coefficient
c := second cognitive/confidence coefficient
 ----------------------------------- Equations
For each particle and each dimension
Equation 1:	v(t+1) = w*v(t) + R(c)*(p(t)-x(t)) + R(c)*(g(t)-x(t))
Equation 2:	x(t+1) = x(t) + v(t+1)
where
v(t) := velocity at time t
x(t) := position at time t
p(t) := best previous position of the particle
g(t) := best position amongst the best previous positions
        of the informants of the particle
R(c) := a number coming from a random distribution, which depends on c
In this standard, the distribution is uniform on [0,c]
Note 1:
When the particle has no informant better than itself,
it implies p(t) = g(t)
Therefore, Equation 1 gets modified to:
v(t+1) = w*v(t) + R(c)*(p(t)-x(t))
Note 2:
When the "non sensitivity to rotation" option is activated
(p(t)-x(t)) (and (g(t)-x(t))) are replaced by rotated vectors, 
so that the final DNPP (Distribution of the Next Possible Positions)
is not dependent on the system of co-ordinates.
 ----------------------------------- Information links topology 
A lot of work has been done about this topic. The main result is this: 
There is no "best" topology. Hence the random approach used here.  
 ----------------------------------- Initialisation
Initial positions are chosen at random inside the search space 
(which is supposed to be a hyperparallelepiped, and often even
a hypercube), according to a uniform distribution.
This is not the best way, but the one used in the original PSO.
Each initial velocity is simply defined as the half-difference of two
random positions. It is simple, and needs no additional parameter.
However, again, it is not the best approach. The resulting distribution
is not even uniform, as is the case for any method that uses a
uniform distribution independently for each component.
The mathematically correct approach needs to use a uniform
distribution inside a hypersphere. It is not very difficult,
and was indeed used in some PSO versions.  However, it is quite
different from the original one. 
Moreover, it may be meaningless for some heterogeneous problems,
when each dimension has a different "interpretation".
------------------------------------ From SPSO-06 to SPSO-07
The main differences are:
1. option "non sensitivity to rotation of the landscape"
    Note: although theoretically interesting, this option is quite
        computer time consuming, and the improvement in result may
        only be marginal. 
2. option "random permutation of the particles before each iteration"
    Note: same remark. Time consuming, no clear improvement
3. option "clamping position or not"
    Note: in a few rare cases, not clamping positions may induce an
    infinite run, if the stop criterion is the maximum number of 
    evaluations
        
4. probability p of a particular particle being an informant of another
    particle. In SPSO-06 it was implicit (by building the random infonetwork)
    Here, the default value is directly computed as a function of (S,K),
    so that the infonetwork is exactly the same as in SPSO-06.
    However, now it can be "manipulated" ( i.e. any value can be assigned)
    
5. The search space can be quantised (however this algorithm is _not_
   for combinatorial problems)
Also, the code is far more modular. It means it is slower, but easier
to translate into another language, and easier to modify.
 ----------------------------------- Use
 Define the problem (you may add your own one in problemDef() and perf())
 Choose your options
 Run and enjoy!
   
 */
namespace SPSO_2007
{
    class Program
    {

        //const int D_max = 114;		// Max number of dimensions of the search space
        const int R_max = 500;	// Max number of runs
        const int S_max = 910;	// Max swarm size
        static Random rand = new Random(); //System Random Number Generator

        // Global variables
        static double sqrtD;

        // File(s);
        static FileStream f_run;
        static FileStream f_synth;

        // =================================================
        static void Main(string[] args)
        {
            Position bestBest = new Position(); // Best position over all runs
            // Current dimension
            double errorMean = 0;// Average error
            double errorMin = double.MaxValue;		// Best result over all runs
            double[] errorMeanBest = new double[R_max];
            double evalMean = 0;		// Mean number of evaluations
            int nFailure = 0;		// Number of unsuccessful runs
            double logProgressMean = 0.0;
            int run;
            f_run = File.OpenWrite("f_run.txt");
            f_synth = File.OpenWrite("f_synth.txt");

            // ----------------------------------------------- PROBLEM
            int functionCode = 102;
            /* (see problemDef( ) for precise definitions)
             0 Parabola (Sphere)
             1 Griewank
             2 Rosenbrock (Banana)
             3 Rastrigin
             4 Tripod (dimension 2)
             5 Ackley
             6 Schwefel
             7 Schwefel 1.2
             8 Schwefel 2.22
             9 Neumaier 3
             10 G3
             11 Network optimisation (Warning: see problemDef() and also perf() for
                                      problem elements (number of BTS and BSC)
             12 Schwefel
             13 2D Goldstein-Price
             14 Schaffer f6
             15 Step	
             16 Schwefel 2.21
             17 Lennard-Jones
             18 Gear Train
              CEC 2005 benchmark  (no more than 30D. See cec2005data.c)
             100 F1 (shifted Parabola/Sphere) 
             102 F6 (shifted Rosenbrock) 
             103 F9 (shifted Rastrigin) 
             104 F2 Schwefel 
             105 F7 Griewank  (NOT rotated)
             106 F8 Ackley  (NOT rotated) 
     
             99 Test*/

            int runMax = 100;
            if (runMax > R_max) runMax = R_max;


            // -----------------------------------------------------
            // PARAMETERS
            // * means "suggested value"		

            Parameters param = new Parameters();
            param.clamping = 1;
            // 0 => no clamping AND no evaluation. WARNING: the program
            // 				may NEVER stop (in particular with option move 20 (jumps)) 1
            // *1 => classical. Set to bounds, and velocity to zero

            param.initLink = 0; // 0 => re-init links after each unsuccessful iteration
            // 1 => re-init links after each successful iteration

            param.rand = 1; // 0 => Use KISS as random number generator. 
            // Any other value => use the system one

            param.randOrder = 0; // 0 => at each iteration, particles are modified
            //     always according to the same order 0..S-1
            //*1 => at each iteration, particles numbers are
            //		randomly permutated
            param.rotation = 0;
            // WARNING. Experimental code, completely valid only for dimension 2
            // 0 =>  sensitive to rotation of the system of coordinates
            // 1 => non sensitive (except side effects), 
            // 			by using a rotated hypercube for the probability distribution
            //			WARNING. Quite time consuming!

            param.stop = 0;	// Stop criterion
            // 0 => error < pb.epsilon
            // 1 => eval >= pb.evalMax		
            // 2 => ||x-solution|| < pb.epsilon

            // -------------------------------------------------------
            // Some information
            Console.Write(String.Format("\n Function {0} ", functionCode));
            Console.Write("\n (clamping, randOrder, rotation, stop_criterion) = ({0}, {1}, {2}, {3})",
                   param.clamping, param.randOrder, param.rotation, param.stop);
            //if (param.rand == 0) Console.Write("\n WARNING, I am using the RNG KISS"); //Now just System.Random

            // =========================================================== 
            // RUNs

            // Initialize some objects
            Problem pb = Problem.problemDef(functionCode);

            // You may "manipulate" S, p, w and c
            // but here are the suggested values
            param.S = (int)(10 + 2 * Math.Sqrt(pb.SS.D));	// Swarm size
            if (param.S > S_max) param.S = S_max;
            //param.S=100;
            Console.Write("\n Swarm size {0}", param.S);

            param.K = 3;
            param.p = 1.0 - Math.Pow(1.0 - (1.0 / (param.S)), param.K);
            // (to simulate the global best PSO, set param.p=1)
            //param.p=1;

            // According to Clerc's Stagnation Analysis
            param.w = 1.0 / (2.0 * Math.Log(2.0)); // 0.721
            param.c = 0.5 + Math.Log(2.0); // 1.193

            Console.Write("\n c = {0},  w = {1}", param.c, param.w);
            //---------------
            sqrtD = Math.Sqrt(pb.SS.D);

            //------------------------------------- RUNS	
            for (run = 0; run < runMax; run++)
            {
                //srand (clock () / 100);	// May improve pseudo-randomness            
                Result result = PSO(param, pb);
                
                if (result.error > pb.epsilon) // Failure
                {
                    nFailure = nFailure + 1;
                }

                // Memorize the best (useful if more than one run)
                if (result.error < bestBest.f)
                    bestBest = result.SW.P[result.SW.best].Clone();

                // Result display
                Console.Write("\nRun {0}. Eval {1}. Error {2} \n", run + 1, result.nEval, result.error);
                //for (d=0;d<pb.SS.D;d++) Console.Write(" %f",result.SW.P[result.SW.best].x[d]);

                // Save result
                //TODO: Fix up writing out to files
                /*fConsole.Write(f_run, "\n%i %.0f %e ", run + 1, result.nEval, error);
                    for (d = 0; d < pb.SS.D; d++) fConsole.Write(f_run, " %f", result.SW.P[result.SW.best].x[d]);
                 */

                // Compute/store some statistical information
                if (run == 0)
                    errorMin = result.error;
                else if (result.error < errorMin)
                    errorMin = result.error;
                evalMean = evalMean + result.nEval;
                errorMean = errorMean + result.error;
                errorMeanBest[run] = result.error;
                logProgressMean = logProgressMean - Math.Log(result.error);
            }		// End loop on "run"

            // ---------------------END 
            // Display some statistical information
            evalMean /= runMax;
            errorMean /= runMax;
            logProgressMean /= runMax;

            Console.Write("\n Eval. (mean)= {0}", evalMean);
            Console.Write("\n Error (mean) = {0}", errorMean);
            // Variance
            double variance = 0;
            for (run = 0; run < runMax; run++)
            {    variance += Math.Pow(errorMeanBest[run] - errorMean, 2);}
            variance = Math.Sqrt(variance / runMax);
            Console.Write("\n Std. dev. {0}", variance);
            Console.Write("\n Log_progress (mean) = {0}", logProgressMean);
            // Success rate and minimum value
            Console.Write("\n Failure(s) {0}", nFailure);
            Console.Write("\n Success rate = {0}%", 100 * (1 - nFailure / (double)runMax));

            Console.Write("\n Best min value = {0}", errorMin);
            Console.Write("\nPosition of the optimum: ");
            for (int d = 0; d < pb.SS.D; d++) 
            {Console.Write(" {0}", bestBest.x[d]);}

            // Save	
            //TODO: Fix up writing out to files
            /*fConsole.Write(f_synth, "%f %f %.0f%% %f   ",
                     errorMean, variance, successRate, evalMean);
            for (d = 0; d < pb.SS.D; d++) fConsole.Write(f_synth, " %f", bestBest.x[d]);
            fConsole.Write(f_synth, "\n");
             * */
            Console.ReadLine();
            return; // End of main program
        }
        // ===============================================================
        // PSO
        static Result PSO(Parameters param, Problem pb)
        {
            Velocity aleaV = new Velocity();
            int d;
            int g;
            int[] index = new int[S_max];
            int[] indexTemp = new int[S_max];
            // Iteration number (time step)
            int iterBegin;
            int[,] LINKS = new int[S_max, S_max];	// Information links
            int m;
            int noEval;
            double normPX = 0.0, normGX = 0.0;
            int noStop;
            int outside;
            double p;
            Velocity PX = new Velocity();
            Result R = new Result();
            Matrix RotatePX = new Matrix();
            Matrix RotateGX = new Matrix();
            int s0, s, s1;
            double zz;

            aleaV.size = pb.SS.D;
            RotatePX.size = pb.SS.D;
            RotateGX.size = pb.SS.D;
            // -----------------------------------------------------
            // INITIALISATION
            p = param.p; // Probability threshold for random topology
            R.SW.S = param.S; // Size of the current swarm

            // Position and velocity
            for (s = 0; s < R.SW.S; s++)
            {
                R.SW.X[s].size = pb.SS.D;
                R.SW.V[s].size = pb.SS.D;

                for (d = 0; d < pb.SS.D; d++)
                {
                    R.SW.X[s].x[d] = rand.NextDouble(pb.SS.minInit[d], pb.SS.maxInit[d]);
                }

                for (d = 0; d < pb.SS.D; d++)
                {
                    R.SW.V[s].v[d] =
                        (rand.NextDouble(pb.SS.min[d], pb.SS.max[d]) - R.SW.X[s].x[d]) / 2;
                }
                // Take quantisation into account
                Position.quantis(R.SW.X[s], pb.SS);
            }

            // First evaluations
            for (s = 0; s < R.SW.S; s++)
            {
                R.SW.X[s].f =
                    Problem.perf(R.SW.X[s], pb.function, pb.objective);

                R.SW.P[s] = R.SW.X[s].Clone();	// Best position = current one
                R.SW.P[s].improved = 0;	// No improvement
            }

            // If the number max of evaluations is smaller than 
            // the swarm size, just keep evalMax particles, and finish
            if (R.SW.S > pb.evalMax) R.SW.S = pb.evalMax;
            R.nEval = R.SW.S;

            // Find the best
            R.SW.best = 0;
            double errorPrev;
            switch (param.stop)
            {
                default:
                    errorPrev = R.SW.P[R.SW.best].f; // "distance" to the wanted f value (objective)
                    break;

                case 2:
                    errorPrev = Position.distanceL(R.SW.P[R.SW.best], pb.solution, 2); // Distance to the wanted solution
                    break;
            }

            for (s = 1; s < R.SW.S; s++)
            {
                switch (param.stop)
                {
                    default:
                        zz = R.SW.P[s].f;
                        if (zz < errorPrev)
                        {
                            R.SW.best = s;
                            errorPrev = zz;
                        }
                        break;

                    case 2:
                        zz = Position.distanceL(R.SW.P[R.SW.best], pb.solution, 2);
                        if (zz < errorPrev)
                        {
                            R.SW.best = s;
                            errorPrev = zz;
                        }
                        break;
                }
            }
            // Display the best
            Console.Write(" Best value after init. {0} ", errorPrev);
            //	Console.Write( "\n Position :\n" );
            //	for ( d = 0; d < SS.D; d++ ) Console.Write( " %f", R.SW.P[R.SW.best].x[d] );

            int initLinks = 1;		// So that information links will beinitialized
            // Note: It is also a flag saying "No improvement"
            noStop = 0;
            double error = errorPrev;
            // ---------------------------------------------- ITERATIONS
            int iter = 0;
            while (noStop == 0)
            {
                iter++;

                if (initLinks == 1)	// Random topology
                {
                    // Who informs who, at random
                    for (s = 0; s < R.SW.S; s++)
                    {
                        for (m = 0; m < R.SW.S; m++)
                        {
                            if (rand.NextDouble() < p) LINKS[m, s] = 1;	// Probabilistic method
                            else LINKS[m, s] = 0;
                        }
                    }
                    /*	
                     // Ring topology  (Just for test)
                     for (s = 0; s < R.SW.S; s++)
                     {	
                         for (m = 0; m < R.SW.S; m++)
                         {		    
                             LINKS[m][s] = 0;
                         }
                     }
                     for (s = 0; s < R.SW.S-1; s++)
                     {	
                         for (m = s+1; m < R.SW.S; m++)
                         {		    
                             LINKS[m][s] = 1;
                         }
                     }
                     LINKS[ 0 ][R.SW.S-1]=1;
                     */
                    // Each particle informs itself
                    for (m = 0; m < R.SW.S; m++)
                    {
                        LINKS[m, m] = 1;
                    }
                }

                // The swarm MOVES
                //Console.Write("\nIteration %i",iter);
                for (int i = 0; i < R.SW.S; i++)
                    index[i] = i;
                //Permutate the index order
                if (param.randOrder == 1)
                {
                    index.Shuffle(7, R.SW.S);
                }

                Velocity GX = new Velocity();
                for (s0 = 0; s0 < R.SW.S; s0++)	// For each particle ...
                {
                    s = index[s0];
                    // ... find the first informant
                    s1 = 0;
                    while (LINKS[s1, s] == 0) s1++;
                    if (s1 >= R.SW.S) s1 = s;

                    // Find the best informant			
                    g = s1;
                    for (m = s1; m < R.SW.S; m++)
                    {
                        if (LINKS[m, s] == 1 && R.SW.P[m].f < R.SW.P[g].f)
                            g = m;
                    }

                    //.. compute the new velocity, and move

                    // Exploration tendency
                    for (d = 0; d < pb.SS.D; d++)
                    {
                        R.SW.V[s].v[d] = param.w * R.SW.V[s].v[d];
                    }

                    // Prepare Exploitation tendency  p-x
                    for (d = 0; d < pb.SS.D; d++)
                    {
                        PX.v[d] = R.SW.P[s].x[d] - R.SW.X[s].x[d];
                    }
                    PX.size = pb.SS.D;

                    if (g != s)
                    {
                        for (d = 0; d < pb.SS.D; d++) // g-x
                        {
                            GX.v[d] = R.SW.P[g].x[d] - R.SW.X[s].x[d];
                        }
                        GX.size = pb.SS.D;
                    }

                    // Option "non sentivity to rotation"				
                    if (param.rotation > 0)
                    {
                        normPX = Velocity.normL(PX, 2);
                        if (g != s) normGX = Velocity.normL(GX, 2);
                        if (normPX > 0)
                        {
                            RotatePX = Matrix.MatrixRotation(PX);
                        }

                        if (g != s && normGX > 0)
                        {
                            RotateGX = Matrix.MatrixRotation(GX);
                        }
                    }

                    // Exploitation tendencies
                    switch (param.rotation)
                    {
                        default:
                            for (d = 0; d < pb.SS.D; d++)
                            {
                                R.SW.V[s].v[d] = R.SW.V[s].v[d] +
                                    +rand.NextDouble(0.0, param.c) * PX.v[d];
                            }

                            if (g != s)
                            {
                                for (d = 0; d < pb.SS.D; d++)
                                {
                                    R.SW.V[s].v[d] = R.SW.V[s].v[d]
                                        + rand.NextDouble(0.0, param.c) * GX.v[d];
                                }
                            }
                            break;

                        case 1:
                            // First exploitation tendency
                            if (normPX > 0)
                            {
                                zz = param.c * normPX / sqrtD;
                                aleaV = rand.NextVector(pb.SS.D, zz);
                                Velocity expt1 = RotatePX.VectorProduct(aleaV);

                                for (d = 0; d < pb.SS.D; d++)
                                {
                                    R.SW.V[s].v[d] = R.SW.V[s].v[d] + expt1.v[d];
                                }
                            }

                            // Second exploitation tendency
                            if (g != s && normGX > 0)
                            {
                                zz = param.c * normGX / sqrtD;
                                aleaV = rand.NextVector(pb.SS.D, zz);
                                Velocity expt2 = RotateGX.VectorProduct(aleaV);
                                for (d = 0; d < pb.SS.D; d++)
                                {
                                    R.SW.V[s].v[d] = R.SW.V[s].v[d] + expt2.v[d];
                                }
                            }
                            break;
                    }

                    // Update the position
                    for (d = 0; d < pb.SS.D; d++)
                    {
                        R.SW.X[s].x[d] = R.SW.X[s].x[d] + R.SW.V[s].v[d];
                    }

                    if (R.nEval >= pb.evalMax)
                    {
                        //error= fabs(error - pb.objective);
                        goto end;
                    }
                    // --------------------------
                    noEval = 1;

                    // Quantisation
                    Position.quantis(R.SW.X[s], pb.SS);

                    switch (param.clamping)
                    {
                        case 0:	// No clamping AND no evaluation
                            outside = 0;

                            for (d = 0; d < pb.SS.D; d++)
                            {
                                if (R.SW.X[s].x[d] < pb.SS.min[d] || R.SW.X[s].x[d] > pb.SS.max[d])
                                    outside++;
                            }

                            if (outside == 0)	// If inside, the position is evaluated
                            {
                                R.SW.X[s].f =
                                    Problem.perf(R.SW.X[s], pb.function, pb.objective);
                                R.nEval = R.nEval + 1;
                            }
                            break;

                        case 1:	// Set to the bounds, and v to zero
                            for (d = 0; d < pb.SS.D; d++)
                            {
                                if (R.SW.X[s].x[d] < pb.SS.min[d])
                                {
                                    R.SW.X[s].x[d] = pb.SS.min[d];
                                    R.SW.V[s].v[d] = 0;
                                }

                                if (R.SW.X[s].x[d] > pb.SS.max[d])
                                {
                                    R.SW.X[s].x[d] = pb.SS.max[d];
                                    R.SW.V[s].v[d] = 0;
                                }
                            }

                            R.SW.X[s].f = Problem.perf(R.SW.X[s], pb.function, pb.objective);
                            R.nEval = R.nEval + 1;
                            break;
                    }

                    // ... update the best previous position
                    if (R.SW.X[s].f < R.SW.P[s].f)	// Improvement
                    {
                        R.SW.P[s] = R.SW.X[s].Clone();

                        // ... update the best of the bests
                        if (R.SW.P[s].f < R.SW.P[R.SW.best].f)
                        {
                            R.SW.best = s;
                        }
                    }
                }			// End of "for (s0=0 ...  "	
                // Check if finished
                switch (param.stop)
                {
                    default:
                        error = R.SW.P[R.SW.best].f;
                        break;

                    case 2:
                        error = Position.distanceL(R.SW.P[R.SW.best], pb.solution, 2);
                        break;
                }
                //error= fabs(error - pb.epsilon);

                if (error < errorPrev)	// Improvement
                {
                    initLinks = 0;
                }
                else			// No improvement
                {
                    initLinks = 1;	// Information links will be	reinitialized	
                }

                if (param.initLink == 1) initLinks = 1 - initLinks;

                errorPrev = error;
            end:

                switch (param.stop)
                {
                    case 0:
                    case 2:
                        if (error > pb.epsilon && R.nEval < pb.evalMax)
                        {
                            noStop = 0;	// Won't stop
                        }
                        else
                        {
                            noStop = 1;	// Will stop
                        }
                        break;

                    case 1:
                        if (R.nEval < pb.evalMax)
                            noStop = 0;	// Won't stop
                        else
                            noStop = 1;	// Will stop
                        break;
                }

            } // End of "while nostop ...

            // Console.Write( "\n and the winner is ... %i", R.SW.best );			
            // fConsole.Write( f_stag, "\nEND" );
            R.error = error;
            return R;
        }
    }
}
