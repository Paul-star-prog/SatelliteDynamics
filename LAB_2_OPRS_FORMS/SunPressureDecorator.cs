using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLinearAlgebra;

namespace LAB_2_OPRS_FORMS
{
    public class SunPressureDecorator : TDecorator
    {
        private TMatrix CoordSun;
        private int index;
        private const double P = 4.56, // световое давление Солнца на орбите Земли [Н / км^2]
                             A = 8.27E-6, m = 1415, // площадь поперечного сечения ИСЗ [км^2], масса ИСЗ [кг] (ГЛОНАСС М)
                             C = 1.0, // коэффициент отражения поверхности ИСЗ (1.0 - отражение зеркальное / 1.44 - отражение диффузионное)
                             a = 1.496E+8; // 1 a.е. (км)
        public SunPressureDecorator(double W, double Theta, double Omega, double I, double E, double A, Satellite satellite, TMatrix CoordSun) : base(W, Theta, Omega, I, E, A, satellite)
        {
            this.CoordSun = CoordSun;
            index = 0;
        }
        private byte sunlight(TVector ComeVec)
        {
            /*
             * функция определения освещенности ИСЗ Солнцем
             * 0 - ИСЗ в тени Земли
             * 1 - ИСЗ освещен Солнцем
            */
            // вектор положения ИСЗ отн-о Земли 
            TVector r = new TVector(3);
            for (int i = 0; i < 3; i++)
                r[i] = ComeVec[i];
            double r_length = r.VectorLength(r);
            // вектор положения Солнца отн-о Земли
            TVector r_sun = new TVector(3);
            for (int i = 0; i < 3; i++)
                r_sun[i] = CoordSun[index, i + 1];
            double r_sun_length = r_sun.VectorLength(r_sun);
            // угол между экваториальным радиусом Земли и геоцентрическим положением ИСЗ
            double phi = Math.Asin(6378.136 / r_length);
            // угол между осью цилиндра тени и геоцентрическим положением ИСЗ
            double psi = - Math.Acos( r.Scal(r, r_sun)/ (r_length * r_sun_length));
            if ((psi > - phi) && ( psi <= phi)) { return 0; }
             else { return 1; }
        }
        private TVector sunlight_gravitation(double time, TVector ComeVec)
        {
            TVector g = new TVector(ComeVec.vector.Length);
            // вектор положения ИСЗ отн-о Земли 
            TVector r = new TVector(3);
            for(int i = 0; i < 3; i++)
              r[i] = ComeVec[i];
            // вектор положения Солнца отн-о Земли
            TVector rM = new TVector(3);
            for(int i = 0; i < 3; i++)
              rM[i] = CoordSun[index, i + 1];
            // вектор положения ИСЗ отн-о Солнцем
            TVector r1 = rM - r;
            // расстояние между ИСЗ и Солнцем
            double r1_length = r1.VectorLength(r1);
            // если текущее время больше, чем интервал выдачи,
            // то увеличиваем текущий индекс прохода по матрице координат Солнца
            if ((time - CoordSun[index, 0]) == (CoordSun[1, 0] - CoordSun[0, 0])) { index++; }
            // получение значения функции тени на текущий момент времени
            double q = sunlight(ComeVec);
            // расчет ускорения, обусловленного давлением солнечного света
            for(int i = 0; i < 3; i++)
              g[i] = -q * P * C * A * a * a * (rM[i] - r[i]) / (m * Math.Pow(r1_length, 3));
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
            // получение ускорения, обусл. солнечным давлением
            TVector g_sunlight = sunlight_gravitation(time, X);
            // добавка к исходным ускорениям ИСЗ ускорения, обусловленного давлением солнечного света 
            for (int i = 3; i < dY.vector.Length; i++)
                dY[i] = g_satellite[i - 3] + g_sunlight[i - 3];
            return dY;
        }
    }
}
