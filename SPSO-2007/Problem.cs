using System;

namespace SPSO_2007
{
    public class Problem
    {
        public Problem()
        {
            solution = new Position();
            SS = new SwarmSize();
        }
        public int constraint;			// Number of constraints
        public double epsilon; 	// Admissible error
        public int evalMax; 		// Maximum number of fitness evaluations
        public int function; 		// Function code
        public double objective; 	// Objective value
        // Solution position (if known, just for tests)	
        public Position solution;
        public SwarmSize SS;		// Search space
        //For Network problems
        private static int bcsNb;
        private static int btsNb;
        //Constants
        const int zero = 0;			// 1.0e-30 // To avoid numerical instabilities
        public static Problem problemDef(int functionCode)
        {
            int d;
            Problem pb = new Problem();

            int nAtoms; // For Lennard-Jones problem
            double[] lennard_jones = new[] { -1, -3, -6, -9.103852, -12.71, -16.505384, -19.821489, -24.113360, -28.422532, -32.77, -37.97, -44.33, -47.84, -52.32 };


            pb.function = functionCode;
            pb.epsilon = 0.00000;	// Acceptable error (default). May be modified below
            pb.objective = 0;       // Objective value (default). May be modified below

            // Define the solution point, for test
            // NEEDED when param.stop = 2 
            // i.e. when stop criterion is distance_to_solution < epsilon
            for (d = 0; d < 30; d++)
            {
                pb.solution.x[d] = 0;
            }


            // ------------------ Search space
            switch (pb.function)
            {
                case 0:			// Parabola
                    pb.SS.D = 30;//  Dimension							

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.min[d] = -100; // -100
                        pb.SS.max[d] = 100;	// 100
                        pb.SS.q.q[d] = 0;	// Relative quantisation, in [0,1].   
                    }

                    pb.evalMax = 100000;// Max number of evaluations for each run
                    pb.epsilon = 0.0; // 1e-3;	
                    pb.objective = 0;

                    // For test purpose, the initialisation space may be different from
                    // the search space. If so, just modify the code below

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.maxInit[d] = pb.SS.max[d]; // May be a different value
                        pb.SS.minInit[d] = pb.SS.min[d]; // May be a different value
                    }


                    break;
                case 100: // CEC 2005 F1
                    pb.SS.D = 30;//30; 
                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.min[d] = -100;
                        pb.SS.max[d] = 100;
                        pb.SS.q.q[d] = 0;

                    }
                    pb.evalMax = pb.SS.D * 10000;
                    pb.epsilon = 0.000001;	//Acceptable error
                    pb.objective = -450;       // Objective value

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.maxInit[d] = pb.SS.max[d];
                        pb.SS.minInit[d] = pb.SS.min[d];
                    }
                    break;

                case 102:		// Rosenbrock. CEC 2005 F6
                    pb.SS.D = 10;	// 10

                    // Boundaries
                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.min[d] = -100; pb.SS.max[d] = 100;
                        pb.SS.q.q[d] = 0;

                    }

                    pb.evalMax = pb.SS.D * 10000;
                    pb.epsilon = 0.01;	//0.01 Acceptable error
                    pb.objective = 390;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.maxInit[d] = pb.SS.max[d];
                        pb.SS.minInit[d] = pb.SS.min[d];
                    }
                    break;

                case 103:// CEC 2005 F9, Rastrigin
                    pb.SS.D = 30;
                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.min[d] = -5;
                        pb.SS.max[d] = 5;
                        pb.SS.q.q[d] = 0;

                    }
                    pb.epsilon = 0.01; // 0.01;	// Acceptable error
                    pb.objective = -330;       // Objective value
                    pb.evalMax = pb.SS.D * 10000;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.maxInit[d] = pb.SS.max[d];
                        pb.SS.minInit[d] = pb.SS.min[d];
                    }
                    break;

                case 104:// CEC 2005 F2  Schwefel
                    pb.SS.D = 10;
                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.min[d] = -100;
                        pb.SS.max[d] = 100;
                        pb.SS.q.q[d] = 0;

                    }
                    pb.epsilon = 0.00001;	// Acceptable error
                    pb.objective = -450;       // Objective value
                    pb.evalMax = pb.SS.D * 10000;


                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.maxInit[d] = pb.SS.max[d];
                        pb.SS.minInit[d] = pb.SS.min[d];
                    }
                    break;


                case 105:// CEC 2005 F7  Griewank (NON rotated)
                    pb.SS.D = 10;	 // 10 
                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.min[d] = -600;
                        pb.SS.max[d] = 600;
                        pb.SS.q.q[d] = 0;

                    }
                    pb.epsilon = 0.01;	//Acceptable error
                    pb.objective = -180;       // Objective value
                    pb.evalMax = pb.SS.D * 10000;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.maxInit[d] = pb.SS.max[d];
                        pb.SS.minInit[d] = pb.SS.min[d];
                    }
                    break;


                case 106:// CEC 2005 F8 Ackley (NON rotated)
                    pb.SS.D = 10;
                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.min[d] = -32;
                        pb.SS.max[d] = 32;
                        pb.SS.q.q[d] = 0;
                    }
                    pb.epsilon = 0.0001;	// Acceptable error
                    pb.objective = -140;       // Objective value
                    pb.evalMax = pb.SS.D * 10000;
                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.maxInit[d] = pb.SS.max[d];
                        pb.SS.minInit[d] = pb.SS.min[d];
                    }
                    break;
                    /*
                        case 100:			// Parabola
                            pb.SS.D =10;//  Dimension							

                        for (d = 0; d < pb.SS.D; d++)
                        {   
                            pb.SS.min[d] = -100; // -100
                            pb.SS.max[d] = 100;	// 100
                            pb.SS.q.q[d] = 0;	// Relative quantisation, in [0,1].   
                        }

                        pb.evalMax = 100000;// Max number of evaluations for each run
                        pb.epsilon=0.00000;

                        for (d = 0; d < pb.SS.D; d++)
                        {  
                            pb.SS.maxInit[d]=pb.SS.max[d];
                            pb.SS.minInit[d]=pb.SS.min[d];
                        }
                        break;
                */
                case 1:		// Griewank
                    pb.SS.D = 10;

                    // Boundaries
                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.min[d] = -100;
                        pb.SS.max[d] = 100;
                        pb.SS.q.q[d] = 0;
                    }

                    pb.evalMax = 400000;
                    pb.epsilon = 0.05;
                    pb.objective = 0;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.maxInit[d] = pb.SS.max[d];
                        pb.SS.minInit[d] = pb.SS.min[d];
                    }
                    break;

                case 2:		// Rosenbrock
                    pb.SS.D = 30;	// 30

                    // Boundaries
                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.min[d] = -30; // -30; 
                        pb.SS.max[d] = 30; // 30;			
                        pb.SS.q.q[d] = 0;
                    }
                    pb.epsilon = 0;
                    pb.evalMax = 300000; //2.e6;  // 40000 
                    pb.objective = 0;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.maxInit[d] = 30; //pb.SS.max[d];
                        pb.SS.minInit[d] = 15; //pb.SS.min[d];
                    }
                    break;


                case 3:		// Rastrigin
                    pb.SS.D = 10;

                    // Boundaries
                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.min[d] = -5.12;
                        pb.SS.max[d] = 5.12;
                        pb.SS.q.q[d] = 0;
                    }

                    pb.evalMax = 3200;
                    pb.epsilon = 0.0;
                    pb.objective = 0;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.minInit[d] = pb.SS.min[d];
                        pb.SS.maxInit[d] = pb.SS.max[d];
                    }
                    break;

                case 4:		// Tripod
                    pb.SS.D = 2;	// Dimension

                    // Boundaries
                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.min[d] = -100;
                        pb.SS.max[d] = 100;
                        pb.SS.q.q[d] = 0;
                    }

                    pb.evalMax = 10000;
                    pb.epsilon = 0.0001;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.maxInit[d] = pb.SS.max[d];
                        pb.SS.minInit[d] = pb.SS.min[d];
                    }
                    break;

                case 5: // Ackley
                    pb.SS.D = 10;
                    // Boundaries
                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.min[d] = -32; // 32
                        pb.SS.max[d] = 32;
                        pb.SS.q.q[d] = 0;
                    }
                    pb.evalMax = 3200;
                    pb.epsilon = 0.0;
                    pb.objective = 0;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.maxInit[d] = pb.SS.max[d];
                        pb.SS.minInit[d] = pb.SS.min[d];
                    }
                    break;

                case 6: // Schwefel. Min on (A=420.8687, ..., A)
                    pb.SS.D = 30;
                    //pb.objective=-pb.SS.D*420.8687*sin(Math.Sqrt(420.8687));
                    pb.objective = -12569.5;
                    pb.epsilon = 2569.5;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.min[d] = -500;
                        pb.SS.max[d] = 500;
                        pb.SS.q.q[d] = 0;
                    }
                    pb.evalMax = 300000;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.maxInit[d] = pb.SS.max[d];
                        pb.SS.minInit[d] = pb.SS.min[d];
                    }
                    break;

                case 7: // Schwefel 1.2
                    pb.SS.D = 40;
                    pb.objective = 0;
                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.min[d] = -100;
                        pb.SS.max[d] = 100;
                        pb.SS.q.q[d] = 0;
                    }
                    pb.evalMax = 40000;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.maxInit[d] = pb.SS.max[d];
                        pb.SS.minInit[d] = pb.SS.min[d];
                    }
                    break;

                case 8: // Schwefel 2.22
                    pb.SS.D = 30;
                    pb.objective = 0;
                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.min[d] = -10;
                        pb.SS.max[d] = 10;
                        pb.SS.q.q[d] = 0;
                    }
                    pb.evalMax = 100000;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.maxInit[d] = pb.SS.max[d];
                        pb.SS.minInit[d] = pb.SS.min[d];
                    }
                    break;

                case 9: // Neumaier 3
                    pb.SS.D = 40;
                    pb.objective = 0;
                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.min[d] = -pb.SS.D * pb.SS.D;
                        pb.SS.max[d] = -pb.SS.min[d];
                        pb.SS.q.q[d] = 0;
                    }
                    pb.evalMax = 40000;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.maxInit[d] = pb.SS.max[d];
                        pb.SS.minInit[d] = pb.SS.min[d];
                    }
                    break;

                case 10: // G3 (constrained)
                    pb.SS.D = 10;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.min[d] = 0;
                        pb.SS.max[d] = 1;
                        pb.SS.q.q[d] = 0;
                    }
                    pb.evalMax = 340000;
                    pb.objective = 0;
                    pb.epsilon = 1e-6;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.maxInit[d] = pb.SS.max[d];
                        pb.SS.minInit[d] = pb.SS.min[d];
                    }

                    break;

                case 11: // Network
                    // btsNb=5; bcsNb=2;
                    btsNb = 19; bcsNb = 4;
                    pb.SS.D = bcsNb * btsNb + 2 * bcsNb;
                    pb.objective = 0;
                    for (d = 0; d < bcsNb * btsNb; d++) // Binary representation. 1 means: there is a link
                    {
                        pb.SS.min[d] = 0;
                        pb.SS.max[d] = 1;
                        pb.SS.q.q[d] = 1;
                    }

                    for (d = bcsNb * btsNb; d < pb.SS.D; d++) // 2D space for the BSC positions
                    {
                        pb.SS.min[d] = 0;
                        pb.SS.max[d] = 20; //15;
                        pb.SS.q.q[d] = 0;
                    }

                    pb.evalMax = 50;
                    pb.objective = 0;
                    pb.epsilon = 0;

                    break;

                case 12: // Schwefel
                    pb.SS.D = 30;
                    pb.objective = 0;
                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.min[d] = -500;
                        pb.SS.max[d] = 500;
                        pb.SS.q.q[d] = 0;
                    }
                    pb.evalMax = 200000;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.maxInit[d] = pb.SS.max[d];
                        pb.SS.minInit[d] = pb.SS.min[d];
                    }
                    break;

                case 13:		  // 2D Goldstein-Price function (f_min=3, on (0,-1))
                    pb.SS.D = 2;	// Dimension
                    pb.objective = 0;

                    pb.SS.min[0] = -100;
                    pb.SS.max[0] = 100;
                    pb.SS.q.q[0] = 0;
                    pb.SS.min[1] = -100;
                    pb.SS.max[1] = 100;
                    pb.SS.q.q[1] = 0;
                    pb.evalMax = 720;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.maxInit[d] = pb.SS.max[d];
                        pb.SS.minInit[d] = pb.SS.min[d];
                    }
                    break;
                case 14: // Schaffer f6	 
                    pb.SS.D = 2;	// Dimension
                    pb.objective = 0;

                    pb.SS.min[0] = -100;
                    pb.SS.max[0] = 100;
                    pb.SS.q.q[0] = 0;
                    pb.SS.min[1] = -100;
                    pb.SS.max[1] = 100;
                    pb.SS.q.q[1] = 0;

                    pb.evalMax = 4000;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.maxInit[d] = pb.SS.max[d];
                        pb.SS.minInit[d] = pb.SS.min[d];
                    }

                    break;

                case 15: // Step
                    pb.SS.D = 20;
                    pb.objective = 0;
                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.min[d] = -100;
                        pb.SS.max[d] = 100;
                        pb.SS.q.q[d] = 0;
                    }
                    pb.evalMax = 2500;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.maxInit[d] = pb.SS.max[d];
                        pb.SS.minInit[d] = pb.SS.min[d];
                    }
                    break;

                case 16: // Schwefel 2.21
                    pb.SS.D = 30;
                    pb.objective = 0;
                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.min[d] = -100;
                        pb.SS.max[d] = 100;
                        pb.SS.q.q[d] = 0;
                    }
                    pb.evalMax = 100000;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.maxInit[d] = pb.SS.max[d];
                        pb.SS.minInit[d] = pb.SS.min[d];
                    }
                    break;

                case 17: // Lennard-Jones
                    nAtoms = 2; // in {2, ..., 15}
                    pb.SS.D = 3 * nAtoms; pb.objective = lennard_jones[nAtoms - 2];
                    pb.evalMax = 5000 + 3000 * nAtoms * (nAtoms - 1); // Empirical rule
                    pb.epsilon = 1e-6;
                    // Note: with this acceptable error, nAtoms=10 seems to be the maximum
                    //       possible value for a non-null success rate  (5%)

                    //pb.SS.D=3*21; pb.objective=-81.684;	
                    //pb.SS.D=3*27; pb.objective=-112.87358;
                    //pb.SS.D=3*38; pb.objective=-173.928427;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.min[d] = -2;
                        pb.SS.max[d] = 2;
                        pb.SS.q.q[d] = 0;
                    }

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.maxInit[d] = pb.SS.max[d];
                        pb.SS.minInit[d] = pb.SS.min[d];
                    }
                    break;
                case 18: //Gear Train
                    pb.SS.D=4;

		            for (d = 0; d < pb.SS.D; d++)                  
		            {
			            pb.SS.min[d]=12;
			            pb.SS.max[d]=60;
			            pb.SS.q.q[d] = 1;
                        pb.SS.maxInit[d] = pb.SS.max[d];
                        pb.SS.minInit[d] = pb.SS.min[d];
		            }

		            pb.evalMax = 20000 ; 
		            pb.epsilon = 1e-13;	
		            pb.objective =2.7e-12 ; 
		            break;
                case 19: // Compression spring
                    pb.constraint = 4;
                    pb.SS.D = 3;

                    pb.SS.min[0] = 1; pb.SS.max[0] = 70; pb.SS.q.q[0] = 1;
                    pb.SS.min[1] = 0.6; pb.SS.max[1] = 3; pb.SS.q.q[1] = 0;
                    pb.SS.min[2] = 0.207; pb.SS.max[2] = 0.5; pb.SS.q.q[2] = 0.001;

                    //for (d = 0; d < pb.SS.D; d++)
                    //{
                    //    pb.SS.maxS[d] = pb.SS.max[d];
                    //    pb.SS.minS[d] = pb.SS.min[d];
                    //}
                    pb.evalMax = 20000;
                    pb.epsilon = 1e-10;
                    pb.objective = 2.6254214578;
                    break;

                case 99: // Test

                    pb.SS.D = 2;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.min[d] = -100;
                        pb.SS.max[d] = 100;
                        pb.SS.q.q[d] = 0;
                    }

                    pb.evalMax = 40000;
                    pb.objective = 0.0;
                    pb.epsilon = 0.00;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.maxInit[d] = pb.SS.max[d];
                        pb.SS.minInit[d] = pb.SS.min[d];
                    }
                    break;

                    //TODO: Figure out why the following is unreachable
                    /*
                    // 2D Peaks function
                    pb.SS.D = 2;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.min[d] = -3;
                        pb.SS.max[d] = 3;
                        pb.SS.q.q[d] = 0;
                    }

                    pb.evalMax = 50000;
                    pb.objective = -6.551133;
                    pb.epsilon = 0.001;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.maxInit[d] = pb.SS.max[d];
                        pb.SS.minInit[d] = pb.SS.min[d];
                    }
                    break;
                    // Quartic
                    pb.SS.D = 50;
                    pb.objective = 0;
                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.min[d] = -10;
                        pb.SS.max[d] = 10;
                        pb.SS.q.q[d] = 0;
                    }
                    pb.evalMax = 25000;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.maxInit[d] = pb.SS.max[d];
                        pb.SS.minInit[d] = pb.SS.min[d];
                    }

                    break;


                    pb.SS.D = 2;	// Dimension
                    pb.objective = -2;

                    pb.SS.min[0] = -2;
                    pb.SS.max[0] = 2;
                    pb.SS.q.q[0] = 0;
                    pb.SS.min[1] = -3;
                    pb.SS.max[1] = 3;
                    pb.SS.q.q[1] = 0;

                    pb.evalMax = 10000;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.maxInit[d] = pb.SS.max[d];
                        pb.SS.minInit[d] = pb.SS.min[d];
                    }

                    break;
                    pb.SS.D = 1;	// Dimension
                    // Boundaries
                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.min[d] = -10;
                        pb.SS.max[d] = 10;
                        pb.SS.q.q[d] = 0;
                    }
                    pb.objective = -1000; // Just a sure too small value for the above search space
                    pb.evalMax = 1000;

                    for (d = 0; d < pb.SS.D; d++)
                    {
                        pb.SS.maxInit[d] = pb.SS.max[d];
                        pb.SS.minInit[d] = pb.SS.min[d];
                    }
                    break;
                     */

            }

            pb.SS.q.size = pb.SS.D;
            return pb;
        }

        public static double perf(Position x, int function, double objective)
        {				// Evaluate the fitness value for the particle of rank s   
            int d;
            double DD;
            int k;
            int n;
            double f = 0.0, p, xd, x1, x2, x3, x4;
            double s11, s12, s21, s22;
            double sum1, sum2;
            double t0, tt, t1;
            Position xs = new Position();
            // Shifted Parabola/Sphere (CEC 2005 benchmark)		
            double[] offset_0 =
                { 
                    -3.9311900e+001, 5.8899900e+001, -4.6322400e+001, -7.4651500e+001, -1.6799700e+001,
                    -8.0544100e+001, -1.0593500e+001, 2.4969400e+001, 8.9838400e+001, 9.1119000e+000, 
                    -1.0744300e+001, -2.7855800e+001, -1.2580600e+001, 7.5930000e+000, 7.4812700e+001,
                    6.8495900e+001, -5.3429300e+001, 7.8854400e+001, -6.8595700e+001, 6.3743200e+001, 
                    3.1347000e+001, -3.7501600e+001, 3.3892900e+001, -8.8804500e+001, -7.8771900e+001, 
                    -6.6494400e+001, 4.4197200e+001, 1.8383600e+001, 2.6521200e+001, 8.4472300e+001
                };
            // Shifted Rosenbrock (CEC 2005 benchmark)
            double[] offset_2 =
                { 
                    8.1023200e+001, -4.8395000e+001,  1.9231600e+001, -2.5231000e+000,  7.0433800e+001, 
                    4.7177400e+001, -7.8358000e+000, -8.6669300e+001,  5.7853200e+001, -9.9533000e+000,
                    2.0777800e+001,  5.2548600e+001,  7.5926300e+001,  4.2877300e+001, -5.8272000e+001,
                    -1.6972800e+001,  7.8384500e+001,  7.5042700e+001, -1.6151300e+001,  7.0856900e+001,
                    -7.9579500e+001, -2.6483700e+001,  5.6369900e+001, -8.8224900e+001, -6.4999600e+001,
                    -5.3502200e+001, -5.4230000e+001,  1.8682600e+001, -4.1006100e+001, -5.4213400e+001
                };
            // Shifted Rastrigin (CEC 2005)
            double[] offset_3 =
                { 
                    1.9005000e+000, -1.5644000e+000, -9.7880000e-001, -2.2536000e+000,  2.4990000e+000,
                    -3.2853000e+000,  9.7590000e-001, -3.6661000e+000,  9.8500000e-002, -3.2465000e+000,
                    3.8060000e+000, -2.6834000e+000, -1.3701000e+000,  4.1821000e+000,  2.4856000e+000, 
                    -4.2237000e+000,  3.3653000e+000,  2.1532000e+000, -3.0929000e+000,  4.3105000e+000, 
                    -2.9861000e+000,  3.4936000e+000, -2.7289000e+000, -4.1266000e+000, -2.5900000e+000, 
                    1.3124000e+000, -1.7990000e+000, -1.1890000e+000, -1.0530000e-001, -3.1074000e+000
                };

            // Shifted Schwefel (F2 CEC 2005)
            double[] offset_4 =
                { 
                    3.5626700e+001, -8.2912300e+001, -1.0642300e+001, -8.3581500e+001,  8.3155200e+001,
                    4.7048000e+001, -8.9435900e+001, -2.7421900e+001,  7.6144800e+001, -3.9059500e+001,
                    4.8885700e+001, -3.9828000e+000, -7.1924300e+001,  6.4194700e+001, -4.7733800e+001,
                    -5.9896000e+000 ,-2.6282800e+001, -5.9181100e+001,  1.4602800e+001, -8.5478000e+001,
                    -5.0490100e+001,  9.2400000e-001,  3.2397800e+001,  3.0238800e+001, -8.5094900e+001,
                    6.0119700e+001, -3.6218300e+001, -8.5883000e+000, -5.1971000e+000,  8.1553100e+001 
                };

            // Shifted Griewank (CEC 2005)
            double[] offset_5 =
                { 
                    -2.7626840e+002, -1.1911000e+001, -5.7878840e+002, -2.8764860e+002, -8.4385800e+001,
                    -2.2867530e+002, -4.5815160e+002, -2.0221450e+002, -1.0586420e+002, -9.6489800e+001,
                    -3.9574680e+002, -5.7294980e+002, -2.7036410e+002, -5.6685430e+002, -1.5242040e+002,
                    -5.8838190e+002, -2.8288920e+002, -4.8888650e+002, -3.4698170e+002, -4.5304470e+002,
                    -5.0658570e+002, -4.7599870e+002, -3.6204920e+002, -2.3323670e+002, -4.9198640e+002,
                    -5.4408980e+002, -7.3445600e+001, -5.2690110e+002, -5.0225610e+002, -5.3723530e+002 
                };

            // Shifted Ackley (CEC 2005)
            double[] offset_6 =
                { 
                    -1.6823000e+001,  1.4976900e+001,  6.1690000e+000,  9.5566000e+000,  1.9541700e+001,
                    -1.7190000e+001, -1.8824800e+001,  8.5110000e-001, -1.5116200e+001,  1.0793400e+001,
                    7.4091000e+000,  8.6171000e+000, -1.6564100e+001, -6.6800000e+000,  1.4543300e+001,
                    7.0454000e+000, -1.8621500e+001,  1.4556100e+001, -1.1594200e+001, -1.9153100e+001,
                    -4.7372000e+000,  9.2590000e-001,  1.3241200e+001, -5.2947000e+000,  1.8416000e+000,
                    4.5618000e+000, -1.8890500e+001,  9.8008000e+000, -1.5426500e+001,  1.2722000e+000
                };
            /*
                // Shifted Parabola/Sphere (CEC 2005 benchmark)		
                static double offset_0[30] =
                { 
                    -3.9311900e+001, 5.8899900e+001, -4.6322400e+001, -7.4651500e+001, -1.6799700e+001,
                    -8.0544100e+001, -1.0593500e+001, 2.4969400e+001, 8.9838400e+001, 9.1119000e+000, 
                    -1.0744300e+001, -2.7855800e+001, -1.2580600e+001, 7.5930000e+000, 7.4812700e+001,
                    6.8495900e+001, -5.3429300e+001, 7.8854400e+001, -6.8595700e+001, 6.3743200e+001, 
                    3.1347000e+001, -3.7501600e+001, 3.3892900e+001, -8.8804500e+001, -7.8771900e+001, 
                    -6.6494400e+001, 4.4197200e+001, 1.8383600e+001, 2.6521200e+001, 8.4472300e+001
                };
                */
            /*
             static float bts[5][2]=
             {
                 {6.8, 9.0},
                 {8.3, 7.9},
                 {6.6, 5.6},
                 {10, 5.4},
                 {8, 3} 
             };
             */
            float[,] bts = new float[,]
                               {
                                   {6, 9},
                                   {8, 7},
                                   {6, 5},
                                   {10, 5},
                                   {8, 3} ,
                                   {12, 2},
                                   {4, 7},
                                   {7, 3},
                                   {1, 6},
                                   {8, 2},
                                   {13, 12},
                                   {15, 7},
                                   {15, 11},
                                   {16, 6},
                                   {16, 8},
                                   {18, 9},
                                   {3, 7},
                                   {18, 2},
                                   {20, 17}
                               };
            float btsPenalty = 100;
            double z1, z2;

            xs = x.Clone();

            switch (function)
            {
                case 100: // Parabola (Sphere) CEC 2005
                    f = -450;
                    for (d = 0; d < xs.size; d++)
                    {
                        xd = xs.x[d];
                        xs.x[d] = xd - offset_0[d];
                        f = f + xd * xd;
                    }
                    break;

                case 102:  // Rosenbrock 

                    for (d = 0; d < xs.size; d++)
                    {
                        xs.x[d] = xs.x[d] - offset_2[d];
                    }

                    f = 390;

                    for (d = 1; d < xs.size; d++)
                    {
                        tt = xs.x[d - 1] - 1;
                        f = f + tt * tt;

                        tt = xs.x[d - 1] * xs.x[d - 1] - xs.x[d];
                        f = f + 100 * tt * tt;
                    }

                    break;

                case 103: // Rastrigin 
                    for (d = 0; d < xs.size; d++)
                    {
                        xs.x[d] = xs.x[d] - offset_3[d];
                    }
                    f = -330;
                    k = 10;

                    for (d = 0; d < xs.size; d++)
                    {
                        xd = xs.x[d];
                        f = f + xd * xd - k * Math.Cos(2 * Math.PI * xd);
                    }
                    f = f + xs.size * k;
                    break;

                case 104: // Schwefel (F2)
                    for (d = 0; d < xs.size; d++)
                    {
                        xs.x[d] = xs.x[d] - offset_4[d];
                    }

                    f = -450;
                    for (d = 0; d < xs.size; d++)
                    {
                        sum2 = 0.0;
                        for (k = 0; k <= d; k++)
                        {
                            sum2 += xs.x[k];
                        }
                        f += sum2 * sum2;
                    }
                    break;


                case 105: // Griewank. WARNING: in the CEC 2005 benchmark it is rotated
                    sum1 = 0.0;
                    sum2 = 1.0;
                    f = -180;
                    for (d = 0; d < xs.size; d++)
                    {
                        xd = xs.x[d] - offset_5[d];
                        sum1 += xd * xd;
                        sum2 *= Math.Cos(xd / Math.Sqrt(1.0 + d));
                    }
                    f = f + 1.0 + sum1 / 4000.0 - sum2;
                    break;

                case 106: // Ackley 
                    f = -140;

                    sum1 = 0.0;
                    sum2 = 0.0;
                    for (d = 0; d < xs.size; d++)
                    {
                        xd = xs.x[d] - offset_6[d];
                        sum1 += xd * xd;
                        sum2 += Math.Cos(2.0 * Math.PI * xd);
                    }
                    sum1 = -0.2 * Math.Sqrt(sum1 / xs.size);
                    sum2 /= xs.size;
                    f = f + 20.0 + Math.E - 20.0 * Math.Exp(sum1) - Math.Exp(sum2);
                    break;
                    /*
                        case 100:
                            for (d = 0; d < xs.size; d++) 
                        {
                            xs.x[d]=xs.x[d]-offset_0[d];
                        }
                */
                case 0:		// Parabola (Sphere)
                    f = 0;

                    for (d = 0; d < xs.size; d++)
                    {
                        xd = xs.x[d];
                        f = f + xd * xd;
                    }
                    break;

                case 1:		// Griewank
                    f = 0;
                    p = 1;

                    for (d = 0; d < xs.size; d++)
                    {
                        xd = xs.x[d];
                        f = f + xd * xd;
                        p = p * Math.Cos(xd / Math.Sqrt((double)(d + 1)));
                    }
                    f = f / 4000 - p + 1;
                    break;

                case 2:		// Rosenbrock
                    f = 0;
                    t0 = xs.x[0] + 1;	// Solution on (0,...0) when
                    // offset=0
                    for (d = 1; d < xs.size; d++)
                    {

                        t1 = xs.x[d] + 1;
                        tt = 1 - t0;
                        f += tt * tt;
                        tt = t1 - t0 * t0;
                        f += 100 * tt * tt;
                        t0 = t1;
                    }
                    break;

                case 3:		// Rastrigin
                    k = 10;
                    f = 0;

                    for (d = 0; d < xs.size; d++)
                    {
                        xd = xs.x[d];
                        f = f + xd * xd - k * Math.Cos(2 * Math.PI * xd);
                    }
                    f = f + xs.size * k;
                    break;

                case 4:		// 2D Tripod function
                    // Note that there is a big discontinuity right on the solution
                    // point. 
                    x1 = xs.x[0];
                    x2 = xs.x[1];
                    s11 = (1.0 - Math.Sign(x1)) / 2;
                    s12 = (1.0 + Math.Sign(x1)) / 2;
                    s21 = (1.0 - Math.Sign(x2)) / 2;
                    s22 = (1.0 + Math.Sign(x2)) / 2;

                    //f = s21 * (fabs (x1) - x2); // Solution on (0,0)
                    f = s21 * (Math.Abs(x1) + Math.Abs(x2 + 50)); // Solution on (0,-50)  
                    f = f + s22 * (s11 * (1 + Math.Abs(x1 + 50) + Math.Abs(x2 - 50)) + s12 * (2 + Math.Abs(x1 - 50) + Math.Abs(x2 - 50)));
                    break;

                case 5:  // Ackley
                    sum1 = 0;
                    sum2 = 0;
                    DD = x.size;
                    for (d = 0; d < x.size; d++)
                    {
                        xd = xs.x[d];
                        sum1 = sum1 + xd * xd;
                        sum2 = sum2 + Math.Cos(2 * Math.PI * xd);
                    }
                    f = -20 * Math.Exp(-0.2 * Math.Sqrt(sum1 / DD)) - Math.Exp(sum2 / DD) + 20 + Math.Exp(1);

                    break;

                case 6: // Schwefel
                    f = 0;
                    for (d = 0; d < x.size; d++)
                    {
                        xd = xs.x[d];
                        f = f - xd * Math.Sin(Math.Sqrt(Math.Abs(xd)));
                    }
                    break;

                case 7: // Schwefel 1.2
                    f = 0;
                    for (d = 0; d < x.size; d++)
                    {
                        xd = xs.x[d];
                        sum1 = 0;
                        for (k = 0; k <= d; k++) sum1 = sum1 + xd;
                        f = f + sum1 * sum1;
                    }
                    break;

                case 8: // Schwefel 2.22
                    sum1 = 0; sum2 = 1;
                    for (d = 0; d < x.size; d++)
                    {
                        xd = Math.Abs(xs.x[d]);
                        sum1 = sum1 + xd;
                        sum2 = sum2 * xd;
                    }
                    f = sum1 + sum2;
                    break;

                case 9: // Neumaier 3
                    sum1 = 0; sum2 = 1;
                    for (d = 0; d < x.size; d++)
                    {
                        xd = xs.x[d] - 1;
                        sum1 = sum1 + xd * xd;
                    }
                    for (d = 1; d < x.size; d++)
                    {
                        sum2 = sum2 + xs.x[d] * xs.x[d - 1];
                    }

                    f = sum1 + sum2;
                    break;

                case 10: // G3 (constrained) 
                    // min =0 on (1/Math.Sqrt(D), ...)
                    f = 1;
                    sum1 = 0;
                    for (d = 0; d < x.size; d++)
                    {
                        xd = xs.x[d];
                        f = f * xd;
                        sum1 = sum1 + xd * xd;
                    }
                    f = Math.Abs(1 - Math.Pow(x.size, x.size / 2) * f) + x.size * Math.Abs(sum1 - 1);
                    break;

                case 11: // Network  btsNb BTS, bcdNb BSC

                    f = 0;
                    // Constraint: each BTS has one link to one BSC 
                    for (d = 0; d < btsNb; d++)
                    {
                        sum1 = 0;
                        for (k = 0; k < bcsNb; k++) sum1 = sum1 + xs.x[d + k * btsNb];
                        if (sum1 < 1 - zero || sum1 > 1 + zero) f = f + btsPenalty;

                    }
                    // Distances
                    for (d = 0; d < bcsNb; d++) //For each BCS d
                    {
                        for (k = 0; k < btsNb; k++) // For each BTS k
                        {
                            if (xs.x[k + d * btsNb] < 1) continue;
                            // There is a link between BTS k and BCS d
                            n = bcsNb * btsNb + 2 * d;
                            z1 = bts[k, 0] - xs.x[n];
                            z2 = bts[k, 1] - xs.x[n + 1];
                            f = f + Math.Sqrt(z1 * z1 + z2 * z2);
                        }
                    }
                    break;

                case 12: // Schwefel
                    f = 418.98288727243369 * x.size;
                    for (d = 0; d < x.size; d++)
                    {
                        xd = xs.x[d];
                        f = f - xd * Math.Sin(Math.Sqrt(Math.Abs(xd)));
                    }
                    break;

                case 13: // 2D Goldstein-Price function
                    x1 = xs.x[0]; x2 = xs.x[1];

                    f = (1 + Math.Pow(x1 + x2 + 1, 2) * (19 - 14 * x1 + 3 * x1 * x1 - 14 * x2 + 6 * x1 * x2 + 3 * x2 * x2))
                        * (30 + Math.Pow(2 * x1 - 3 * x2, 2) *
                           (18 - 32 * x1 + 12 * x1 * x1 + 48 * x2 - 36 * x1 * x2 + 27 * x2 * x2));
                    break;

                case 14:  //Schaffer F6
                    x1 = xs.x[0]; x2 = xs.x[1];
                    f = 0.5 + (Math.Pow(Math.Sin(Math.Sqrt(x1 * x1 + x2 * x2)), 2) - 0.5) / Math.Pow(1.0 + 0.001 * (x1 * x1 + x2 * x2), 2);

                    break;

                case 15: // Step
                    f = 0;
                    for (d = 0; d < x.size; d++)
                    {
                        xd = (int)(xs.x[d] + 0.5);
                        f = f + xd * xd;
                    }
                    break;

                case 16: // Schwefel 2.21
                    f = 0;
                    for (d = 0; d < x.size; d++)
                    {
                        xd = Math.Abs(xs.x[d]);
                        if (xd > f) f = xd;
                    }
                    break;

                case 17: // Lennard-Jones
                    f = lennard_jones(xs);
                    break;
                case 18: // Gear train
			        x1= xs.x[0]; // {12,13, ... 60}
		            x2= xs.x[1];// {12,13, ... 60}
		            x3= xs.x[2];// {12,13, ... 60}
		            x4= xs.x[3];// {12,13, ... 60}

		            f=1.0/6.931 - x1*x2/(x3*x4);
		            f=f*f;
		            break;
                case 19:
                    x1=xs.x[0]; // {1,2, ... 70}
		            x2= xs.x[1];//[0.6, 3]
		            x3= xs.x[2];// relaxed form [0.207,0.5]  dx=0.001
		            // In the original problem, it is a list of
		            // acceptable values
		            // {0.207,0.225,0.244,0.263,0.283,0.307,0.331,0.362,0.394,0.4375,0.5}

		            f=Math.PI*Math.PI*x2*x3*x3*(x1+2)*0.25;
		            // Constraints
                    //TODO: Merge in Constraints
                    //ff=constraint(xs,pb.function,pb.epsConstr);
                    //if(pb.constraint==0)
                    //{
                    //    if (ff.f[1]>0) {c=1+ff.f[1]; f=f*c*c*c;}
                    //    if (ff.f[2]>0) {c=1+ff.f[1]; f=f*c*c*c;}
                    //    if (ff.f[3]>0) {c=1+ff.f[3]; f=f*c*c*c;}
                    //    if (ff.f[4]>0) {c=1+pow(10,10)*ff.f[4]; f=f*c*c*c;}
                    //    if (ff.f[5]>0) {c=1+pow(10,10)*ff.f[5]; f=f*c*c*c;}
                    //}
		break;
                case 99: // Test

                    x1 = xs.x[0]; x2 = xs.x[1];
                    sum1 = x1 * x1 + x2 * x2;
                    f = 0.5 + (Math.Pow(Math.Sin(Math.Sqrt(sum1)), 2) - 0.5) / (1 + 0.001 * sum1 * sum1);
                    break;


                    //TODO: Figure out why the following is unreachable
                    /*
                f=0;
            
                for(d=0;d<x.size;d++)
                {
                        sum1=0;
                        for(k=0;k<d+1;k++)
                        {
                            sum1=sum1+xs.x[d];
                        }
                    
                    f=f+sum1*sum1;	
                }
            
                break;
    
            f=1.e6*xs.x[0]*xs.x[0];
                        
                for (d=1;d<x.size;d++)
            {
                xd = xs.x[d];
                f=f+xd*xd;
            }
        
            break;

    // 2D Peaks function
            x1=xs.x[0];
            x2=xs.x[1];

            f=3*(1-x1)*(1-x1)*Math.Exp(-x1*x1-(x2+1)*(x2+1))
            -10*(x1/5-Math.Pow(x1,3)-Math.Pow(x2,5))*Math.Exp(-x1*x1-x2*x2)
            -(1./3)*Math.Exp(-(x1+1)*(x1+1) - x2*x2);

            break;

                    // Quartic
                    f=0;
                for (d=0;d<x.size;d++)
            {
                xd = xs.x[d];
                f=f+(d+1)*Math.Pow(xd,4)+alea_normal(0,1);
            }	

                break;	

                x1=xs.x[0]; x2=xs.x[1];
                f=(1-x1)*(1-x1)*Math.Exp(-x1*x1-(x2+1)*(x2+1))-(x1-x1*x1*x1-Math.Pow(x2,5))*Math.Exp(-x1*x1-x2*x2);
                f=-f; // To minimise
                break;

                xd=xs.x[0];
                f=xd*(xd+1)*Math.Cos(xd);
                break;*/

            }

            return Math.Abs(f - objective);
        }
        static double lennard_jones(Position x)
        {
            /*
                This is for black-box optimisation. Therefore, we are not supposed to know
                that there are some symmetries. That is why the dimension of the problem is
                3*nb_of_atoms, as it could be 3*nb_of_atoms-6
            */
            const int dim = 3;
            int nPoints = x.size / dim;
            var x1 = new Position { size = dim };
            var x2 = new Position { size = dim };

            double f = 0;
            for (int i = 0; i < nPoints - 1; i++)
            {
                for (int d = 0; d < dim; d++) x1.x[d] = x.x[3 * i + d];
                for (int j = i + 1; j < nPoints; j++)
                {
                    for (int d = 0; d < dim; d++) x2.x[d] = x.x[3 * j + d];

                    double dist = Position.distanceL(x1, x2, 2);
                    double zz = Math.Pow(dist, -6);
                    f = f + zz * (zz - 1);
                }
            }
            f = 4 * f;
            return f;
        }
    }
}