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
        {
            return FitnessEvaluator.Evaluate(x, function, objective, bcsNb, btsNb);
        }
    }
}