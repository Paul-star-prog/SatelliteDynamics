using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLinearAlgebra;

namespace LAB_2_OPRS_FORMS
{
    public abstract class TDecorator : Satellite
    {
        protected Satellite _satellite;
        public TDecorator(double W, double Theta, double Omega, double I, double E, double A, Satellite satellite) : base(W, Theta, Omega, I, E, A)
        {
            this._satellite = satellite;
        }
        public override TVector RightPart(double time, TVector ComeVec)
        {
            if (this._satellite != null)
            {
                return base.RightPart(time, ComeVec);
            }
            else
            {
                return null;
            }
        }
    }
}
