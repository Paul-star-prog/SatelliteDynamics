using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLinearAlgebra;

namespace LAB_2_OPRS_FORMS
{
    public class SatelliteCentralGravitation : Satellite
    {
        public SatelliteCentralGravitation(double W, double Theta, double Omega, double I, double E, double A) : base(W, Theta, Omega, I, E, A)
        {
        }
        public override TVector RightPart(double time, TVector ComeVec)
        {
            TVector r = new TVector(3);
            r[0] = ComeVec[0];
            r[1] = ComeVec[1];
            r[2] = ComeVec[2];
            double r_length = r.VectorLength(r);
            TVector dY = new TVector(6);
            dY[0] = ComeVec[3];//X'
            dY[1] = ComeVec[4];//Y'
            dY[2] = ComeVec[5];//Z'
            dY[3] = -u * ComeVec[0] / Math.Pow(r_length, 3);//Vx'
            dY[4] = -u * ComeVec[1] / Math.Pow(r_length, 3);//Vy'
            dY[5] = -u * ComeVec[2] / Math.Pow(r_length, 3);//Vz'
            return dY;
        }
    }
}
