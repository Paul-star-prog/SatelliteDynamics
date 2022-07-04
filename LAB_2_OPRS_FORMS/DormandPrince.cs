using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLinearAlgebra;

namespace LAB_2_OPRS_FORMS
{
    public class DormandPrince : TIntegrtor
    {
        protected TVector B1, B2, Y, Z, X, X_out, SumY, SumZ, Sum_X_Out;
        protected TVector[] K;
        protected double h_new;

        public TVector B11 { get => B1; set => B1 = value; }
        public TVector B21 { get => B2; set => B2 = value; }
        public TVector Y1 { get => Y; set => Y = value; }
        public TVector Z1 { get => Z; set => Z = value; }
        public TVector X1 { get => X; set => X = value; }
        public TVector X_out1 { get => X_out; set => X_out = value; }
        public TVector SumY1 { get => SumY; set => SumY = value; }
        public TVector SumZ1 { get => SumZ; set => SumZ = value; }
        public TVector Sum_X_Out1 { get => Sum_X_Out; set => Sum_X_Out = value; }
        public TVector[] K1 { get => K; set => K = value; }
        public double H_new { get => h_new; set => h_new = value; }

        public DormandPrince()
        {
            //установленная ошибка
            Eps = 1E-12;
            // ошибка на шаге интегрирования
            E = 0;
            //выделение памяти под  коэффициенты метода
            A = new TMatrix(7, 6);
            C = new TVector(7);
            B11 = new TVector(7);
            B21 = new TVector(7);
            B = new TVector(6);
            //Инициализируем размерности коэффициентов K[j]
            K1 = new TVector[7];
            //шаг интегрирования
            H = 1e-1;
            //инициализация коэффициентов метода
            C[0] = 0.0; C[1] = 1.0 / 5.0; C[2] = 3.0 / 10.0; C[3] = 4.0 / 5.0; C[4] = 8.0 / 9.0; C[5] = 1.0; C[6] = 1.0;
            B11[0] = 35.0 / 384.0; B11[1] = 0.0; B11[2] = 500.0 / 1113.0; B11[3] = 125.0 / 192.0; B11[4] = -2187.0 / 6784.0; B11[5] = 11.0 / 84.0; B11[6] = 0.0;
            B21[0] = 5179.0 / 57600.0; B21[1] = 0.0; B21[2] = 7571.0 / 16695.0; B21[3] = 393.0 / 640.0; B21[4] = -92097.0 / 339200.0; B21[5] = 187.0 / 2100.0; B21[6] = 1.0 / 40.0;
            A[0, 0] = 0.0;
            A[1, 0] = 1.0 / 5.0;
            A[2, 0] = 3.0 / 40.0; A[2, 1] = 9.0 / 40.0;
            A[3, 0] = 44.0 / 45.0; A[3, 1] = -56.0 / 15.0; A[3, 2] = 32.0 / 9.0;
            A[4, 0] = 19372.0 / 6561.0; A[4, 1] = -25360.0 / 2187.0; A[4, 2] = 64448.0 / 6561.0; A[4, 3] = -212.0 / 729.0;
            A[5, 0] = 9017.0 / 3168.0; A[5, 1] = -355.0 / 33.0; A[5, 2] = 46732.0 / 5247.0; A[5, 3] = 49.0 / 176.0; A[5, 4] = -5103.0 / 18656.0;
            A[6, 0] = 35.0 / 384.0; A[6, 1] = 0.0; A[6, 2] = 500.0 / 1113.0; A[6, 3] = 125.0 / 192.0; A[6, 4] = -2187.0 / 6784.0; A[6, 5] = 11.0 / 84.0;

        }
        private double RoundError()
        {
            double v = 1;
            double u = 0;
            while (v + 1 > 1)
            {
                u = v;
                v = v / 2;
            };
            return u;
        }
        private void StepLocalError()
        {
            //высчитывание ошибки округления
            double u = RoundError();
            for (int i = 0; i < Y1.vector.Length; i++)
            {
                double max1 = 1e-5 > Math.Abs(Y1[i]) ? 1e-5 : Math.Abs(Y1[i]);
                double max2 = Math.Abs(X1[i]) > 2 * u / Eps ? Math.Abs(X1[i]) : 0.25 * u / Eps;
                double max = max1 > max2 ? max1 : max2;
                E += Math.Pow(h * (Y1[i] - Z1[i]) / max, 2);
            }
        }
        private double CorrectStep()
        {
            double min1 = Math.Pow((E / Eps), 0.2) / 0.25;
            double min2 = 5.0 < min1 ? 5.0 : min1;
            return h / (0.1 > min2 ? 0.1 : min2); ;
        }
        public override void Run(TMath math, double step, double Tk)
        {
            //выделение памяти под начальный вектор состояния
            X1 = new TVector(math.X0.vector.Length);
            //вектор состояния на начало интнгрирования
            X1 = math.X0;
            //вектор состояния на конец интегрирования(4-й порядок)
            Y1 = new TVector(X1.vector.Length);
            //вектор состояния на конец интегрирования(5-й порядок)
            Z1 = new TVector(X1.vector.Length);
            for (int i = 0; i < 7; ++i)
                K1[i] = new TVector(X1.vector.Length);
            //выделение памяти под суммы векторов состояния
            SumY1 = new TVector(X1.vector.Length);
            SumZ1 = new TVector(X1.vector.Length);
            Sum_X_Out1 = new TVector(X1.vector.Length);
            //инициализация времени интегрирования(на начало интегрирования = 0)
            math.T = 0;
            math.SamplingIncremet = step;
            //шаг интегрирования скорректированный
            H_new = math.SamplingIncremet;
            //время окончания интегрирования
            double T = Tk;
            //время интегрирования(на начало интегрирования = 0)
            double t = math.T;
            //количество сделанных итераций
            N = 0;
            //подготовка матрицы результата 
            math.PrepareResult(T);
            //время выдачи результата
            double t_out = t;
            while (t <= (T))
            {
                E = 0;
                H = H_new;
                K1[0] = math.RightPart(t, X1);
                K1[1] = math.RightPart(t + (H * C[1]), X1 + (A[1, 0] * H) * K1[0]);
                K1[2] = math.RightPart(t + (H * C[2]), X1 + H * (A[2, 0] * K1[0] + A[2, 1] * K1[1]));
                K1[3] = math.RightPart(t + (H * C[3]), X1 + H * (A[3, 0] * K1[0] + A[3, 1] * K1[1] + A[3, 2] * K1[2]));
                K1[4] = math.RightPart(t + (H * C[4]), X1 + H * (A[4, 0] * K1[0] + A[4, 1] * K1[1] + A[4, 2] * K1[2] + A[4, 3] * K1[3]));
                K1[5] = math.RightPart(t + (H * C[5]), X1 + H * (A[5, 0] * K1[0] + A[5, 1] * K1[1] + A[5, 2] * K1[2] + A[5, 3] * K1[3] + A[5, 4] * K1[4]));
                K1[6] = math.RightPart(t + (H * C[6]), X1 + H * (A[6, 0] * K1[0] + A[6, 1] * K1[1] + A[6, 2] * K1[2] + A[6, 3] * K1[3] + A[6, 4] * K1[4] + A[6, 5] * K1[5]));
                for (int i = 0; i < X1.vector.Length; ++i)
                {
                    SumY1[i] = 0;
                    SumZ1[i] = 0;
                }
                for (int i = 0; i < K1.Length; ++i)
                {
                    SumY1 += B11[i] * K1[i];
                    SumZ1 += B21[i] * K1[i];
                }
                Y1 = X1 + H * SumY1;// для 4-го порядка точности
                Z1 = X1 + H * SumZ1;// для 5-го порядка точности
                // Расчет локальной ошибки
                StepLocalError();
                E = Math.Pow(E / X1.vector.Length, 0.5);
                // Расчет нового шага
                H_new = CorrectStep();
                //если ошибка на шаге больше , чем максимально допустимая , то делаем шаг заново
                if (E > Eps) continue;
                while ((t_out < t + H) && (t_out <= (T)))
                {
                    double theta = (t_out - t) / H;
                    //вычисляем коэффициенты плотной выдачи
                    B[0] = theta * (1.0 + theta * (-1337.0 / 480.0 + theta * (1039.0 / 360.0 + theta * (-1163.0 / 1152.0))));
                    B[1] = 0.0;
                    B[2] = 100.0 * theta * theta * (1054.0 / 9275.0 + theta * (-4682.0 / 27825.0 + theta * (379.0 / 5565.0))) / 3.0;
                    B[3] = -5.0 * theta * theta * (27.0 / 40.0 + theta * (-9.0 / 5.0 + theta * (83.0 / 96.0))) / 2.0;
                    B[4] = 18225.0 * theta * theta * (-3.0 / 250.0 + theta * (22.0 / 375.0 + theta * (-37.0 / 600.0))) / 848.0;
                    B[5] = -22.0 * theta * theta * (-3.0 / 10.0 + theta * (29.0 / 30.0 + theta * (-17.0 / 24.0))) / 7.0;
                    //получаем результат для выдачи
                    for (int i = 0; i < X1.vector.Length; ++i)
                        Sum_X_Out1[i] = 0;
                    for (int i = 0; i < B.vector.Length; ++i)
                        Sum_X_Out1 += B[i] * K1[i];
                    X_out1 = X1 + H * Sum_X_Out1;
                    //отправляем результаты в матрицу
                    math.AddResult(X_out1, t_out);
                    //дискретно увеличиваем время выдачи
                    t_out += math.SamplingIncremet;
                }
                //Обновляем X решением 5-го порядка и наращиваем время на величину сделанного шага
                X1 = Z1;
                t += H;
                N++;
            }
        }
    }
}
