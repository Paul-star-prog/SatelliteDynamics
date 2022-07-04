using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLinearAlgebra;

namespace LAB_2_OPRS_FORMS
{
    public class Arenstorf : TMath
    {
        private const double u = 0.012277471, u_1 = 1 - u;
        public Arenstorf()
        {
            //инициализация вектора нач.условий
            X0 = new TVector(4);
            X0[0] = 0.994;//x(0)
            X0[1] = 0;//y(0)
            X0[2] = 0;//x'(0)
            X0[3] = -2.0015851063790825;//y'(0)
        }

        public override TVector RightPart(double time, TVector ComeVec)//[0]-x,[1]-y,[2]-x',[3]-y'
        {
            double D1, D2;
            D1 = Math.Pow(Math.Pow(ComeVec[0] + u, 2) + Math.Pow(ComeVec[1], 2), 1.5);
            D2 = Math.Pow(Math.Pow(ComeVec[0] - u_1, 2) + Math.Pow(ComeVec[1], 2), 1.5);
            TVector dY = new TVector(4);
            dY[0] = ComeVec[2];
            dY[1] = ComeVec[3];
            dY[2] = ComeVec[0] + 2 * ComeVec[3] - u_1 * (ComeVec[0] + u) / D1 - u * (ComeVec[0] - u_1) / D2;
            dY[3] = ComeVec[1] - 2 * ComeVec[2] - u_1 * ComeVec[1] / D1 - u * ComeVec[1] / D2;
            return dY;
        }
    }
}
