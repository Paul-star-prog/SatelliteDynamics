using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLinearAlgebra;

namespace LAB_2_OPRS_FORMS
{
    public abstract class Satellite : TMath
    {
        private double w, theta, omega, i, e, a;
        protected const double u = 386326.4;
        protected MatrixConvert matrixConvert;
        public Satellite(double W, double Theta, double Omega, double I, double E, double A)
        {
            w = W; theta = Theta; omega = Omega;
            i = I; e = E; a = A;
            matrixConvert = new MatrixConvert();
            //инициализация вектора нач.условий
            //пересчёт параметров орбиты из оскулирующих элементов в геоцентрические инерциальные координаты
            X0 = new TVector(6);
            TVector r1 = getRosculating();
            TVector r = new TVector(r1.vector.Length);
            TMatrix A1 = matrixConvert.OscullatingParamToDecarteISC(w, theta, omega, i);
            r = A1 * r1;
            for (int i = 0; i < r.vector.Length; i++)
            {
                X0[i] = r[i];
            }
            TVector v1 = getVosculating();
            TVector v = new TVector(v1.vector.Length);
            v = A1 * v1;
            for (int i = v1.vector.Length; i < 2 * v1.vector.Length; i++)
            {
                X0[i] = v[i - v1.vector.Length];
            }
        }
        protected double getfocalparam()
        {
            return a * (1 - e * e);
        }
        protected TVector getRosculating()
        {
            TVector Rosculating = new TVector(3);
            Rosculating[0] = getfocalparam() / (1 + e * Math.Cos(theta));
            Rosculating[1] = 0;
            Rosculating[2] = 0;
            return Rosculating;
        }
        protected TVector getVosculating()
        {
            TVector Vosculating = new TVector(3);
            Vosculating[0] = Math.Sqrt(u / getfocalparam()) * e * Math.Sin(theta);
            Vosculating[1] = Math.Sqrt(u / getfocalparam()) * (1 + e * Math.Cos(theta));
            Vosculating[2] = 0;
            return Vosculating;
        }
        public override TVector RightPart(double time, TVector ComeVec)
        {
            throw new NotImplementedException();
        }
    }
}
