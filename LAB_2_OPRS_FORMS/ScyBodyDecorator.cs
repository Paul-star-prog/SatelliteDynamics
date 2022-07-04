using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLinearAlgebra;

namespace LAB_2_OPRS_FORMS
{
    public class ScyBodyDecorator : TDecorator
    {
        private TMatrix coord;
        private int index;
        private double u;
        public ScyBodyDecorator(double W, double Theta, double Omega, double I, double E, double A, Satellite satellite, TMatrix Coord, double u) : base(W, Theta, Omega, I, E, A, satellite)
        {
            coord = Coord;
            index = 0;
            this.u = u;
        }
        private TVector body_gravity(double time, TVector X)
        {
            TVector g = new TVector(X.vector.Length);
            // вектор положения ИСЗ отн-о Земли 
            TVector r = new TVector(3);
            for(int i = 0; i < 3; i++)
              r[i] = X[i];
            // вектор положения возмущающего тела отн-о Земли
            TVector rM = new TVector(3);
            for(int i = 0; i < 3; i++)
              rM[i] = coord[index, i + 1];
            // вектор положения ИСЗ отн-о возмущающего тела
            TVector r1 = rM - r;
            // если текущее время больше, чем интервал выдачи,
            // то увеличиваем текущий индекс прохода по матрице координат возмущающего тела
            if ((time - coord[index, 0]) == (coord[1, 0] - coord[0, 0])) { index++; }
            // расстояние между ИСЗ и возмущающим телом
            double r1_length = r1.VectorLength(r1);
            // расстояние между Землей и возмущающим телом
            double rM_length = rM.VectorLength(rM);
            // расчет ускорения, обусловленного воздействием возмущающего тела на ИСЗ
            for (int i = 0; i < 3; i++)
               g[i] = u * r1[i] / Math.Pow(r1_length, 3) - u * rM[i] / Math.Pow(rM_length, 3);
            return g;
        }
        public override TVector RightPart(double time, TVector ComeVec)
        {
            // вектор геоцентрического положения ИСЗ
            TVector X = new TVector(ComeVec.vector.Length - 3);
            for (int i = 0; i < X.vector.Length; i++)
                X[i] = ComeVec[i];
            // расчет исходных пр.частей ИСЗ
            TVector satellite = _satellite.RightPart(time, ComeVec);
            TVector dY = new TVector(6);
            // запись исх. скоростей ИСЗ (без изменений)
            for (int i = 0; i < X.vector.Length; i++)
                dY[i] = ComeVec[i + 3];
            TVector g_satellite = new TVector(3);
            // запись исходного ускорения ИСЗ
            for (int i = 0; i < g_satellite.I; i++)
                g_satellite[i] = satellite[i + 3];
            // получение ускорения, обусл. возмущающим телом
            TVector g_body = body_gravity(time, X);
            // добавка к исходным ускорениям ИСЗ ускорения, обусловленного возмущающим телом
            for (int i = 3; i < dY.vector.Length; i++)
                dY[i] = g_satellite[i - 3] + g_body[i - 3];
            return dY;
        }
    }
}
