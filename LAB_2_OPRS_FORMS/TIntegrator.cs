using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLinearAlgebra;

namespace LAB_2_OPRS_FORMS
{
    public abstract class TIntegrtor
    {
        protected int n; // кол-во итераций
        protected double e;
        protected double h;
        protected double zero;
        protected double eps;
        protected TMatrix a;
        protected TVector b;
        protected TVector c;
        public int N { get { return n; } set { n = value; } }

        public double H { get => h; set => h = value; }
        public double Zero { get => zero; set => zero = value; }
        public double Eps { get => eps; set => eps = value; }
        public double E { get => e; set => e = value; }
        public TMatrix A { get => a; set => a = value; }
        public TVector C { get => c; set => c = value; }
        public TVector B { get => b; set => b = value; }
        public abstract void Run(TMath math, double step, double T_end);
    }
}
