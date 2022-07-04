using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLinearAlgebra;

namespace LAB_2_OPRS_FORMS
{
    public class SatelliteNormalGravitation : Satellite
    {
        private const double a = 6378.136;
        public SatelliteNormalGravitation(double W, double Theta, double Omega, double I, double E, double A) : base(W, Theta, Omega, I, E, A)
        {
        }
        private double delta(int k)
        {
            if (k == 0) { return 0.5; }
            else return 1;
        }
        private double Prec(int n, int m, double phi)
        {

            if (n < m) { return 0; }
            if ((n == m) && (n == 0) && (m == 0)) { return 1; }
            if (n > m) { return Prec(n - 1, m, phi) * Math.Sin(phi) * Math.Sqrt((4 * n * n - 1) / (n * n - m * m)) - Prec(n - 2, m, phi) * Math.Sqrt((((Math.Pow((n - 1), 2)) - m * m) * (2 * n + 1)) / ((n * n - m * m) * (2 * n - 3))); }
            else return Prec(n - 1, m - 1, phi) * Math.Cos(phi) * Math.Sqrt((2 * n + 1) / (2 * n * delta(m - 1)));

        }
        private double Precderivative(int n, double phi)
        {
            return Math.Sqrt(0.5 * n * (n + 1)) * Prec(n, 1, phi);
        }
        private double J(int n)
        {
            if (n == 2) { return 1082.62575E-6; }
            if (n == 4) { return -2.37089E-6; }
            if (n == 6) { return 6.08E-9; }
            else return -1.40E-11;
        }
        private double C0(int n)
        {
            return -J(n) / (Math.Sqrt(2 * n + 1));
        }
        private double Sum1(double ro, double phi)
        {
            double sum = 0;
            for (int n = 2; n <= 8; n = n + 2)
            {
                sum += (n + 1) * Math.Pow((a / ro), n) * C0(n) * Prec(n, 0, phi);
            }
            return sum;
        }
        private double Sum2(double ro, double phi)
        {
            double sum = 0;
            for (int n = 2; n <= 8; n = n + 2)
            {
                sum += Math.Pow((a / ro), n) * C0(n) * Precderivative(n, phi);
            }
            return sum;
        }
        public override TVector RightPart(double time, TVector ComeVec)
        {
            TVector X = new TVector(ComeVec.vector.Length - 3);
            for (int i = 0; i < X.vector.Length; i++)
                X[i] = ComeVec[i];
            TVector sphere = matrixConvert.DecarteISCToSphericCoord(X);
            TVector g = new TVector(3);
            g[0] = -u / Math.Pow(sphere[2], 2) - (u / Math.Pow(sphere[2], 2)) * Sum1(sphere[2], sphere[1]);
            g[1] = u / Math.Pow(sphere[2], 2) * Sum2(sphere[2], sphere[1]);
            g[2] = 0;
            TVector grectangular = new TVector(g.vector.Length);
            TMatrix A = matrixConvert.SphericCoordToDecarteISC(X, Math.Sqrt(X[0] * X[0] + X[1] * X[1]), sphere[2]);
            grectangular = A * g;
            TVector dY = new TVector(6);
            dY[0] = ComeVec[3];
            dY[1] = ComeVec[4];
            dY[2] = ComeVec[5];
            for (int i = 3; i < dY.vector.Length; i++)
                dY[i] = grectangular[i - 3];
            return dY;
        }
    }
}
