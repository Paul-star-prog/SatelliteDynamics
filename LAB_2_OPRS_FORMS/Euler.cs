using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLinearAlgebra;

namespace LAB_2_OPRS_FORMS
{
    public class Euler : TIntegrtor
    {
        protected TVector X, SumX;
        public TVector X1 { get => X; set => X = value; }
        public TVector SumX1 { get => SumX; set => SumX = value; }
        public override void Run(TMath math, double step, double Tk)
        {
            //выделение памяти под начальный вектор состояния
            X = new TVector(math.X0.vector.Length);
            //вектор состояния на начало интнгрирования
            X = math.X0;
            math.T = 0;
            double t = math.T;
            double T = Tk;
            math.SamplingIncremet = step;
            H = math.SamplingIncremet;
            math.PrepareResult(T);
            while (t <= T)
            {
                X = X + (H / 2) * math.RightPart(t, X);
                math.AddResult(X, t);
                t += math.SamplingIncremet;
            }
        }
    }
}
