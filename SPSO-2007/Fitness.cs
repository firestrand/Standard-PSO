using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPSO_2007
{
    public class Fitness
    {
        public int size; 
	    public double[] f;
        public static readonly double	Fmax=1000.0;
	    public static readonly double	Fp=300;
        public static double	S=189000.0;
	    public static double	lmax=14.0;
	    public static double	spm=6.0;
	    public static double	sw=1.25;
	    public static double	G=11500000;
        public Fitness():this(6)
        {}
        public Fitness(int numConstraints)
        { f = new double[numConstraints];}
        public static Fitness Constraint(Position x, int functCode, double epsConstr)
{
	// ff[0] is defined in perf()
	// Variables specific to Coil compressing spring

	double Cf;
	double K;
	double sp;
	double lf;


    Fitness ff = new Fitness();
    ff.f[0] = 0;
	ff.size=1; // Default value

	switch(functCode)
	{

		case 1007:
			ff.size=4;
		//case 7:
			ff.f[1]=0.0193*x.x[2]-x.x[0];
			ff.f[2]=0.00954*x.x[2]-x.x[1];
			ff.f[3]=750*1728-Math.PI*x.x[2]*x.x[2]*(x.x[3]+(4.0/3)*x.x[2]); 
			break;

		case 1008:
			ff.size=5;
		//case 8:
			Cf=1+0.75*x.x[2]/(x.x[1]-x.x[2]) + 0.615*x.x[2]/x.x[1];
			K=0.125*G*Math.Pow(x.x[2],4)/(x.x[0]*x.x[1]*x.x[1]*x.x[1]);
			sp=Fp/K;
			lf=Fmax/K + 1.05*(x.x[0]+2)*x.x[2];

			ff.f[1]=8*Cf*Fmax*x.x[1]/(Math.PI*x.x[2]*x.x[2]*x.x[2]) -S;
			ff.f[2]=lf-lmax;
			ff.f[3]=sp-spm;			
			ff.f[4]=sw- (Fmax-Fp)/K;
			break;

		case 1015:
			ff.size=4 ;
		//case 15:
			ff.f[1]=Math.Abs(x.x[0]*x.x[0]+x.x[1]*x.x[1]+x.x[2]*x.x[2]
			    +x.x[3]*x.x[3]+x.x[4]*x.x[4]-10)-epsConstr; // Constraint h1<=eps
			ff.f[2]=Math.Abs(x.x[1]*x.x[2]-5*x.x[3]*x.x[4])-epsConstr; // Constraint h2<=eps;
			ff.f[3]=Math.Abs(Math.Pow(x.x[0],3)+Math.Pow(x.x[1],3)+1)-epsConstr; // Constraint h3<=eps
			break;

	}

	return ff;
	}
    }
}
