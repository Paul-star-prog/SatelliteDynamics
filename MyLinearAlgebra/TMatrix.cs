using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLinearAlgebra
{
    public class TMatrix
    {
        public int I;//количество строк
        public int J;//количество столбцов
        public int k;
        public double[,] matr;//матрица
        public TMatrix(int i, int j)
        {
            I = i;
            J = j;
            matr = new double[I, J];
        }
        public double this[int i, int j]
        {
            get
            {
                return matr[i, j];
            }
            set
            {
                matr[i, j] = value;
            }
        }
        public void WriteMatrix()
        {
            for (int i = 0; i < I; i++)
            {
                for (int j = 0; j < J; j++)
                {
                    Console.Write("matr[" + (i + 1) + "," + (j + 1) + "]=");
                    matr[i, j] = Double.Parse(Console.ReadLine());
                }
            }
        }
        public void ReadMatrix()
        {
            // Console.WriteLine("Матрица");
            for (int i = 0; i < matr.GetLength(0); i++)
            {
                for (int j = 0; j < matr.GetLength(1); j++)
                {
                    Console.Write("{0:0.00000000}\t", matr[i, j]);
                }
                Console.WriteLine();
            }
        }
        public static TMatrix operator +(TMatrix A, TMatrix B)
        {
            TMatrix C = new TMatrix(A.I, A.J);
            if ((A.I == B.I) && (A.J == B.J))
            {

                for (int i = 0; i < A.I; i++)
                {
                    for (int j = 0; j < A.J; j++)
                    {
                        C[i, j] = A[i, j] + B[i, j];
                    }
                }
                return C;
            }
            else
            {
                Console.WriteLine("Ошибка: у Матриц должна быть одинаковая размерность!");
                return C = null;
            }
        }
        public static TMatrix operator -(TMatrix A, TMatrix B)
        {
            TMatrix C = new TMatrix(A.I, A.J);
            if ((A.I == B.I) && (A.J == B.J))
            {

                for (int i = 0; i < A.I; i++)
                {
                    for (int j = 0; j < A.J; j++)
                    {
                        C[i, j] = A[i, j] - B[i, j];
                    }
                }
                return C;
            }
            else
            {
                Console.WriteLine("Ошибка: у Матриц должна быть одинаковая размерность!");
                return C = null;
            }
        }

        public static TMatrix operator *(TMatrix A, TMatrix B)
        {
            TMatrix C = new TMatrix(A.I, B.J);
            for (int i = 0; i < A.I; i++)
            {
                for (int j = 0; j < B.J; j++)
                {

                    for (int k = 0; k < A.J; k++)
                    {
                        C[i, j] += A[i, k] * B[k, j];
                    }
                }
            }
            return C;
        }

        public static TMatrix operator *(double a, TMatrix A)
        {
            TMatrix C = new TMatrix(A.I, A.J);

            for (int i = 0; i < A.I; i++)
            {
                for (int j = 0; j < A.J; j++)
                {
                    C[i, j] = a * A[i, j];
                }
            }
            return C;
        }

        public TMatrix Transpose(TMatrix A)
        {

            TMatrix C = new TMatrix(A.matr.GetLength(1), A.matr.GetLength(0));
            for (int i = 0; i < A.matr.GetLength(0); i++)
            {
                for (int j = 0; j < A.matr.GetLength(1); j++)
                {
                    C[j, i] = A[i, j];
                }
            }
            return C;
        }
        public double Determinant(TMatrix A, int m)
        {
            if (m <= 0)
            {
                Console.WriteLine("Ошибка!Введите корректную матрицу!");
                return 0;
            }
            else if (m == 1)
            {
                return A[0, 0];
            }
            else if (m == 2)
            {
                return (A[0, 0] * A[1, 1] - A[0, 1] * A[1, 0]);
            }
            else if (m >= 3)
            {
                TMatrix C = new TMatrix(m - 1, m - 1);
                double det = 0;
                int a, b;
                for (int j = 0; j < m; j++)
                {
                    a = 0;
                    for (int k = 1; k < m; k++)
                    {
                        b = 0;
                        for (int s = 0; s < m; s++)
                            if (s != j)
                            {
                                C[a, b] = A[k, s];
                                b++;
                            }
                        a++;
                    }
                    det += Math.Pow(-1, (double)j + 2) * A[0, j] * Determinant(C, m - 1);
                }
                return det;
            }
            else
            {
                return 0;
            }


        }

        public TMatrix Single()
        {
            TMatrix C = new TMatrix(I, J);
            for (int i = 0; i < I; i++)
            {
                for (int j = 0; j < J; j++)
                {
                    if (i == j)
                    {
                        C[i, j] = 1;
                    }
                    else
                    {
                        C[i, j] = 0;
                    }
                }
            }
            return C;
        }
        public TMatrix Negative(TMatrix A)
        {
            TMatrix C = new TMatrix(A.I, A.J);
            for (int i = 0; i < A.I; i++)
            {
                for (int j = 0; j < A.J; j++)
                {
                    C[i, j] = -A[i, j];
                }
            }
            return C;

        }
        public bool SymmCheck(TMatrix A)
        {
            bool flaq;
            flaq = false;
            if (A.I == A.J)
            {
                for (int i = 0; i < A.I; i++)
                {
                    for (int j = i + 1; j < A.J; j++)
                    {
                        if (A[i, j] == A[j, i])
                        {
                            flaq = true;
                        }
                        else
                        {
                            flaq = false;
                        }
                    }
                }
                return flaq;
            }
            else
            {
                return flaq = false;
            }

        }
        public bool SylvesterCriterion(TMatrix A)
        {
            bool flaq;
            flaq = true;
            double[] det = new double[A.I];
            k = 0;
            for (int i = 0; i < A.I; i++)
            {
                det[i] = A.Determinant(A, i + 1);
            }
            for (int i = 0; i < A.I; i++)
            {
                if (det[i] > 0) { k = k + 1; }
            }
            if (k == A.I) { flaq = true; }
            else { flaq = false; }
            return flaq;
        }
        public TMatrix CholeckiyDecomposition(TMatrix A)
        {
            bool flaq;
            flaq = SymmCheck(A);//проверка на симм.
            TMatrix L = new TMatrix(A.I, A.J);//L-нижняя треуг.матрица, A- полож.опред.симм.матрица
            bool flaq1;
            flaq1 = SylvesterCriterion(A);//проверка на полож.опред.
            if ((flaq == true) && (A.I == A.J) && (flaq1 == true))
            {

                L[0, 0] = Math.Sqrt(A[0, 0]);//вычисление 1-го эл.
                for (int i = 1; i < A.I; i++)
                {
                    double SumEl = 0;
                    for (int j = 1; j < A.J + 1; j++)
                    {
                        if (j - 1 < i)
                        {
                            for (int k = 0; k < j - 1; k++)
                                SumEl += L[i, k] * L[j - 1, k];//сумма произв.строк матр.
                            L[i, j - 1] = (A[i, j - 1] - SumEl) / L[j - 1, j - 1];//вычисл.эл.левее диаг.эл.
                        }
                    }
                    double SumEl1 = 0;
                    for (int k = 0; k < i; k++)
                    {
                        SumEl1 += L[i, k] * L[i, k];//сумма квадр.эл.левее диаг.
                    }
                    L[i, i] = Math.Sqrt(A[i, i] - SumEl1);//вычисление диаг.эл.матрицы
                }
                return L;
            }
            else
            {
                Console.WriteLine("Фатальная ошибка:Введите СИММ. ПОЛОЖИТЕЛЬНО ОПРЕД. КВАДР. Матрицу!");
                return L = null;
            }
        }
        public TMatrix SymmInv(TMatrix A)
        {
            TMatrix L = new TMatrix(A.I, A.J);
            TMatrix X = new TMatrix(A.I, A.J);
            L = CholeckiyDecomposition(A);
            if (L != null)
            {
                double sum, sum1, sum2;
                int n;
                n = A.I - 1;
                X[n, n] = 1 / (L[n, n] * L[n, n]);//расчет нижнего эл.диагонали 
                int q = 1;
                for (int i = n; i >= 0; i--)
                {

                    for (int j = n - q; j >= 0; j--)
                    {
                        q = q + 1;
                        sum = 0;
                        for (int k = j + 1; k <= n; k++)
                        {
                            sum = sum + L[k, j] * X[k, i];
                        }
                        X[i, j] = (-sum) / L[j, j];
                        X[j, i] = X[i, j];
                    }
                }
                q = 1;
                for (int i = n - 1; i >= 0; i--)  //расчет средней строки
                {
                    for (int j = n - q; j >= 0; j--)
                    {
                        q = q + 1;
                        sum1 = 0;
                        for (int k = i + 1; k <= n; k++)
                        {
                            sum1 = sum1 + L[k, i] * X[k, i];
                        }
                        sum2 = 0;
                        for (int k = j + 1; k <= n; k++)
                        {
                            sum2 = sum2 + L[k, j] * X[k, i];
                        }

                        if (i == j)
                            X[i, j] = (1 / (L[i, i]) - sum1) / L[i, i];
                        else
                        {
                            X[i, j] = (-sum2) / L[j, j];
                            X[j, i] = X[i, j];

                        }
                    }
                }
                for (int i = n - 2; i >= 0; i--)//расчет верхней строки
                {
                    for (int j = n - 2; i >= 0; i--)
                    {
                        sum = 0;
                        for (int k = j + 1; k <= n; k++)
                        {
                            sum = sum + L[k, i] * X[k, i];
                        }
                        X[i, j] = (1 / (L[i, i]) - sum) / L[i, i];
                    }
                }
                //L_1 = Lt * X;
                return X;
            }
            else
            {
                return X = null;
            }
        }
        public TMatrix CholeckiyInv(TMatrix A)
        {
            TMatrix L = new TMatrix(A.I, A.J);
            TMatrix X = new TMatrix(A.I, A.J);
            TMatrix Lt = new TMatrix(A.I, A.J);
            TMatrix L_1 = new TMatrix(A.I, A.J);
            L = CholeckiyDecomposition(A);
            if (L != null)
            {
                Lt = Transpose(L);
                X = SymmInv(A);
                L_1 = Lt * X;
                return L_1;
            }
            else
            {
                return L_1 = null;
            }

        }
        public void CholeckiyCheck(TMatrix A)
        {
            bool flaq;
            flaq = true;
            TMatrix L = new TMatrix(A.I, A.J);
            TMatrix L_1 = new TMatrix(A.I, A.J);
            TMatrix E = new TMatrix(A.I, A.J);
            TMatrix E_1 = new TMatrix(A.I, A.J);
            L = CholeckiyDecomposition(A);
            L_1 = CholeckiyInv(A);
            E = Single();
            E_1 = L * L_1;
            for (int i = 0; i < A.I; i++)
            {
                for (int j = 0; j < A.J; j++)
                {
                    if (Math.Abs(E[i, j] - E_1[i, j]) < 0.01) { flaq = true; }
                    else { flaq = false; }
                }
            }
            if (flaq == true)
            {
                Console.WriteLine();
                Console.WriteLine("ПРОВЕРКА!Перемножение Нижней Треуг. Матрицы и Обратной для нее:");
                E_1.ReadMatrix();
            }
            if (flaq == false)
            {
                Console.WriteLine("Матрица неединичная!");
                E_1.ReadMatrix();
            }
        }
        public TMatrix Copy(TMatrix A)
        {
            TMatrix Rez = new TMatrix(A.I, A.J);
            for (int i = 0; i < A.I; i++)
            {
                for (int j = 0; j < A.J; j++)
                {
                    Rez[i, j] = A[i, j];
                }
            }
            return Rez;
        }
        public TMatrix SwapString(TMatrix A)
        {
            TMatrix C = new TMatrix(A.I, A.J);
            if (A.I % 2 == 0)
            {
                for (int i = 0; i < (A.I - 1); i++)
                    for (int j = 0; j < A.J; j++)
                    {
                        C[i, j] = A[A.I - i - 1, j];
                        C[A.I - i - 1, j] = A[i, j];
                    }
                if (A.I % 2 != 0)
                {
                    int x = (A.I - 1);
                    for (int j = 0; j < A.J; j++)
                    {
                        C[x, j] = A[x, j];
                    }
                }
                return C;
            }
            else
            {
                for (int i = 0; i < (A.I - 1) / 2; i++)
                    for (int j = 0; j < A.J; j++)
                    {
                        C[i, j] = A[A.I - i - 1, j];
                        C[A.I - i - 1, j] = A[i, j];
                    }
                if (A.I % 2 != 0)
                {
                    int x = (A.I - 1) / 2;
                    for (int j = 0; j < A.J; j++)
                    {
                        C[x, j] = A[x, j];
                    }
                }
                return C;
            }
        }

        public TMatrix SwapColumns(TMatrix A)
        {
            TMatrix C = new TMatrix(A.I, A.J);
            if (A.J % 2 == 0)
            {
                for (int i = 0; i < A.I; i++)
                    for (int j = 0; j < (A.J - 1); j++)
                    {
                        C[i, j] = A[i, A.J - j - 1];
                        C[i, A.J - j - 1] = A[i, j];
                    }
                if (A.J % 2 != 0)
                {
                    int x = (A.J - 1);
                    for (int i = 0; i < A.I; i++)
                    {
                        C[i, x] = A[i, x];
                    }
                }
                return C;
            }
            else
            {
                for (int i = 0; i < A.I; i++)
                    for (int j = 0; j < (A.J - 1) / 2; j++)
                    {
                        C[i, j] = A[i, A.J - j - 1];
                        C[i, A.J - j - 1] = A[i, j];
                    }
                if (A.J % 2 != 0)
                {
                    int x = (A.J - 1) / 2;
                    for (int i = 0; i < A.I; i++)
                    {
                        C[i, x] = A[i, x];
                    }
                }
                return C;
            }
        }
        public TMatrix SwapRows(int i, int j)
        {
            TMatrix C = new TMatrix(I, J);
            for (int k = 0; k < 3; ++k)
            {
                var buff = C[i, k];
                C[i, k] = C[j, k];
                C[j, k] = buff;
            }
            return C;
        }
        public TMatrix SwapColumns(int i, int j)
        {
            TMatrix C = new TMatrix(I, J);
            for (int k = 0; k < 3; ++k)
            {
                var buff = C[k, i];
                C[k, i] = C[k, j];
                C[k, j] = buff;
            }
            return C;
        }
        public void IndexNotZeroRow(TMatrix A, int K, out int N)
        {
            N = 0;
            for (int i = K; i < A.I; ++i)
            {
                if (A[i, K] != 0) { N = i; break; };
            }
        }
    }
}
