using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLinearAlgebra;

namespace LAB_2_OPRS_FORMS
{
    public class RungeKutta : TIntegrtor
    {
        protected TVector X, SumX;
        protected TVector[] K;
        public TVector X1 { get => X; set => X = value; }
        public TVector SumX1 { get => SumX; set => SumX = value; }
        public TVector[] K1 { get => K; set => K = value; }
        public RungeKutta()
        {
            A = new TMatrix(4, 3);
            C = new TVector(3);
            B = new TVector(4);
            K1 = new TVector[4];
            A[0, 0] = 0.0;
            A[1, 0] = 0.5;
            A[2, 0] = 0.0; A[2, 1] = 0.5;
            A[3, 0] = 0.0; A[3, 1] = 0.0; A[3, 2] = 1.0;
            C[0] = A[1, 0]; C[1] = A[1, 0]; C[2] = A[2, 0] + A[2, 1];
            B[0] = 1.0 / 6.0; B[1] = 2.0 / 6.0; B[2] = 2.0 / 6.0; B[3] = 1.0 / 6.0;
        }

        public override void Run(TMath math, double step, double Tk)
        {

            //выделение памяти под начальный вектор состояния
            X = new TVector(math.X0.vector.Length);
            //вектор состояния на начало интнгрирования
            X = math.X0;
            SumX1 = new TVector(X.vector.Length);
            for (int i = 0; i < 4; ++i)
                K[i] = new TVector(X.vector.Length);
            math.T = 0;
            double t = math.T;
            double T = Tk;
            math.SamplingIncremet = step;
            H = math.SamplingIncremet;
            //подготовака матрицы результатов
            math.PrepareResult(T);
            while (t <= T)
            {
                K[0] = math.RightPart(t, X);
                K[1] = math.RightPart(t + (H / 2 * C[0]), X + H / 2 * (A[1, 0] * K[0]));
                K[2] = math.RightPart(t + (H / 2 * C[1]), X + H / 2 * (A[2, 0] * K[0] + A[2, 1] * K[1]));
                K[3] = math.RightPart(t + (H / 2 * C[2]), X + H / 2 * (A[3, 0] * K[0] + A[3, 1] * K[1] + A[3, 2] * K[2]));
                for (int i = 0; i < X.vector.Length; ++i)
                { SumX[i] = 0; }
                for (int i = 0; i < K.Length; ++i)
                {
                    SumX += B[i] * K[i];
                }
                X = X + H * SumX;
                math.AddResult(X, t);
                t += math.SamplingIncremet;
                ++N;
            }
        }
    }
}
