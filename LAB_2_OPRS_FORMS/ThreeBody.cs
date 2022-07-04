using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLinearAlgebra;

namespace LAB_2_OPRS_FORMS
{
    public class ThreeBody : TMath
    {
        public ThreeBody()
        {
            X0 = new TVector(18);
            // н.у. Луны 
            X0[0] = 86334680.0096; //X, км  5.77103476E-1 // а.е
            X0[1] = -124485059.248;//Y, км -8.3211938E-1 // а.е
            X0[2] = -72642.6297696;//Z, км -4.85579076E-5 // а.е
            X0[3] = 24.83898933473;//Vx, км/сек 5.24078311 // безразм
            X0[4] = 17.31056333294;//Vy, км/сек 3.65235907 // безразм
            X0[5] = -0.08915979106;//Vz, км/сек -0.01881184 // безразм
            // н.у. Земли
            X0[6] = 86104728.5032;//X, км 5.75566367E-1 // а.е
            X0[7] = -124150331.0432;//Y, км -8.29881892E-1 // а.е
            X0[8] = -80290.23772;//Z, км -5.36699450E-5 // а.е
            X0[9] = 2.40435899E+1;//Vx, км/сек 5.07296163E+0 // безразм
            X0[10] = 1.67596567E+1;//Vy, км/сек 3.53591219E+0 // безразм
            X0[11] = 5.93870516E-4;//Vz, км/сек 1.25300854E-4 // безразм
            // н.у. Солнце
            X0[12] = -253.928266416;//X, км -1.69738146E-6 // а.е
            X0[13] = 366.127262;//Y, км 2.44737475E-6 // а.е
            X0[14] = 0.023649;//Z, км 1.58081871E-10 // а.е
            X0[15] = -7.09330769E-5;//Vx, км/сек -1.49661835E-5 // безразм
            X0[16] = -4.94410725E-5;//Vy, км/сек -1.04315813E-5 // безразм
            X0[17] = 1.56493465E-9;//Vz, км/сек 3.30185861E-10 // безразм
        }
        public override TVector RightPart(double time, TVector ComeVec)
        {
            TVector dY = new TVector(18);
            //double k = 4 * Math.PI * Math.PI;
            TVector r1 = new TVector(3);
            r1[0] = ComeVec[0];
            r1[1] = ComeVec[1];
            r1[2] = ComeVec[2];
            TVector r2 = new TVector(3);
            r2[0] = ComeVec[6];
            r2[1] = ComeVec[7];
            r2[2] = ComeVec[8];
            TVector r3 = new TVector(3);
            r3[0] = ComeVec[12];
            r3[1] = ComeVec[13];
            r3[2] = ComeVec[14];
            TVector r12 = r2 - r1;
            TVector r13 = r3 - r1;
            TVector r23 = r3 - r2;
            double l12 = r12.VectorLength(r12);
            double l13 = r13.VectorLength(r13);
            double l23 = r23.VectorLength(r23);
            dY[0] = ComeVec[3];
            dY[1] = ComeVec[4];
            dY[2] = ComeVec[5];
            dY[3] = (386326.4 / Math.Pow(l12, 3)) * r12[0] + (1.326663E+11 / Math.Pow(l13, 3)) * r13[0];
            dY[4] = (386326.4 / Math.Pow(l12, 3)) * r12[1] + (1.326663E+11 / Math.Pow(l13, 3)) * r13[1];
            dY[5] = (386326.4 / Math.Pow(l12, 3)) * r12[2] + (1.326663E+11 / Math.Pow(l13, 3)) * r13[2];
            dY[6] = ComeVec[9];
            dY[7] = ComeVec[10];
            dY[8] = ComeVec[11];
             dY[9] = -(4901.783 / Math.Pow(l12, 3)) * r12[0] + (1.326663E+11 / Math.Pow(l23, 3)) * r23[0];
            dY[10] = -(4901.783 / Math.Pow(l12, 3)) * r12[1] + (1.326663E+11 / Math.Pow(l23, 3)) * r23[1];
            dY[11] = -(4901.783 / Math.Pow(l12, 3)) * r12[2] + (1.326663E+11 / Math.Pow(l23, 3)) * r23[2];
            dY[12] = ComeVec[15];
            dY[13] = ComeVec[16];
            dY[14] = ComeVec[17];
            dY[15] = -(4901.783 / Math.Pow(l13, 3)) * r13[0] - (386326.4 / Math.Pow(l23, 3)) * r23[0];
            dY[16] = -(4901.783 / Math.Pow(l13, 3)) * r13[1] - (386326.4 / Math.Pow(l23, 3)) * r23[1];
            dY[17] = -(4901.783 / Math.Pow(l13, 3)) * r13[2] - (386326.4 / Math.Pow(l23, 3)) * r23[2];
            return dY;
        }
    }
}
