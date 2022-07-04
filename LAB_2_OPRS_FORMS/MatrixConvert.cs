using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLinearAlgebra;

namespace LAB_2_OPRS_FORMS
{
    public class MatrixConvert
    {
        public TMatrix OscullatingParamToDecarteISC(double w, double theta, double omega, double i)
        {
            TMatrix A = new TMatrix(3, 3);
            A[0, 0] = Math.Cos(w + theta) * Math.Cos(omega) - Math.Sin(w + theta) * Math.Sin(omega) * Math.Cos(i);
            A[0, 1] = -Math.Sin(w + theta) * Math.Cos(omega) - Math.Cos(w + theta) * Math.Sin(omega) * Math.Cos(i);
            A[0, 2] = Math.Sin(i) * Math.Sin(omega);
            A[1, 0] = Math.Cos(w + theta) * Math.Sin(omega) + Math.Sin(w + theta) * Math.Cos(omega) * Math.Cos(i);
            A[1, 1] = -Math.Sin(w + theta) * Math.Sin(omega) + Math.Cos(w + theta) * Math.Cos(omega) * Math.Cos(i);
            A[1, 2] = -Math.Sin(i) * Math.Cos(omega);
            A[2, 0] = Math.Sin(w + theta) * Math.Sin(i);
            A[2, 1] = Math.Cos(w + theta) * Math.Sin(i);
            A[2, 2] = Math.Cos(i);
            return A;
        }
        public TMatrix SphericCoordToDecarteISC(TVector X, double r0, double ro)
        {
            TMatrix Res = new TMatrix(X.vector.Length, X.vector.Length);
            Res[0, 0] = X[0] / ro;
            Res[0, 1] = -(X[0] * X[2]) / (ro * r0);
            Res[0, 2] = -X[1] / r0;
            Res[1, 0] = X[1] / ro;
            Res[1, 1] = -(X[1] * X[2]) / (ro * r0);
            Res[1, 2] = X[0] / r0;
            Res[2, 0] = X[2] / ro;
            Res[2, 1] = r0 / ro;
            Res[2, 2] = 0;
            return Res;
        }
        public TVector DecarteISCToSphericCoord(TVector X)
        {
            TVector sph = new TVector(3);
            sph[0] = Math.Atan2(X[1], X[0]);
            sph[1] = Math.Atan2(X[2], Math.Sqrt(X[0] * X[0] + X[1] * X[1]));
            sph[2] = Math.Sqrt(X[0] * X[0] + X[1] * X[1] + X[2] * X[2]);
            return sph;
        }
    }
}
