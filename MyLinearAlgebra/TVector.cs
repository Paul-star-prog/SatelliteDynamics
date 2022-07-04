using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLinearAlgebra
{
    public class TVector
    {
        public int I;
        public double[] vector;
        // доделать копию, нормирование, негатив вектора
        public TVector(int i)
        {
            I = i;
            vector = new double[I];
        }
        public double this[int index]
        {
            get
            {
                return vector[index];
            }
            set
            {
                vector[index] = value;
            }
        }
        public TVector Copy(TVector A)
        {
            TVector C = new TVector(A.I);
            for (int i = 0; i < A.I; i++)
            { C[i] = A[i]; }
            return C;
        }
        public TVector Negative(TVector A)
        {
            TVector C = new TVector(A.I);
            for (int i = 0; i < A.I; i++)
            { C[i] = -A[i]; }
            return C;
        }
        public TVector Rationing(TVector A)
        {
            TVector C = new TVector(A.I);
            for (int i = 0; i < A.I; i++)
            { C[i] = A[i] / VectorLength(A); }
            return C;
        }
        public void WriteVector()
        {
            for (int i = 0; i < I; i++)
            {
                Console.Write("vector[" + i + "]=");
                vector[i] = Double.Parse(Console.ReadLine());
            }
        }
        public void ReadVector()
        {
            for (int i = 0; i < I; i++)
            {
                Console.Write("{0:0.000}\t", vector[i]);
                Console.WriteLine();
            }

        }
        public static TVector operator +(TVector A, TVector B)
        {
            TVector C = new TVector(A.I);

            for (int i = 0; i < A.I; i++)
            {
                C[i] = A[i] + B[i];
            }

            return C;
        }
        public static TVector operator -(TVector A, TVector B)
        {
            TVector C = new TVector(A.I);

            for (int i = 0; i < A.I; i++)
            {
                C[i] = A[i] - B[i];
            }

            return C;
        }
        public double Scal(TVector A, TVector B)
        {
            double scal;
            scal = 0;
            for (int i = 0; i < A.I; i++)
            {
                scal += A[i] * B[i];
            }

            return scal;
        }
        public static TVector operator *(double a, TVector A)
        {
            TVector C = new TVector(A.I);
            for (int i = 0; i < A.I; i++)
            {
                C[i] = a * A[i];
            }
            return C;
        }
        public double VectorNorma(TVector A)
        {
            double norma;
            norma = 0;
            for (int i = 0; i < A.I; i++)
            {
                norma += Math.Pow(A[i], 2);
            }
            return norma;
        }
        public double VectorLength(TVector A)
        {
            double length;
            length = 0;
            length = Math.Sqrt(VectorNorma(A));
            return length;

        }
        public static TVector operator *(TVector A, TVector B)
        {
            TVector C = new TVector(A.I);
            if ((A.I == 3) && (B.I == 3))
            {
                C[0] = A[1] * B[2] - A[2] * B[1];
                C[1] = -A[0] * B[2] + A[2] * B[0];
                C[2] = A[0] * B[1] - A[1] * B[0];
                return C;
            }
            else
            {
                Console.WriteLine("Ошибка!Векторное произв.веторов, размерность 3x3");
                return C = null;
            }

        }
        public static TVector operator *(TMatrix A, TVector B)
        {
            TVector C = new TVector(A.I);
            if (A.J == B.I)
            {
                for (int i = 0; i < A.I; i++)
                {
                    for (int j = 0; j < A.J; j++)
                    {
                        C[i] += A.matr[i, j] * B[j];
                    }
                }
                return C;
            }
            else
            {
                Console.WriteLine("Фатальная Ошибка!!!Кол-во строк Матрицы должны совпадать с размером Вектора");
                return C = null;
            }

        }
        public TVector rotateByRodrig(TVector e1, TVector r, double phi)
        {
            TVector r1 = new TVector(r.vector.Length);
            TVector e = new TVector(r.vector.Length);
            e = e1.Rationing(e1);
            r1 = (1 - Math.Cos(phi)) * r.Scal(r, e) * e + Math.Cos(phi) * r + Math.Sin(phi) * (e * r);
            return r1;
        }
        public TVector rotateByQuaternion(TQuaternion Q, TVector r)
        {
            TQuaternion t = new TQuaternion();
            TVector r1 = new TVector(r.vector.Length);
            t = Q * r;
            t = t * Q.Inverse();
            r1 = t.GetVec();
            return r1;
        }
        public TVector rotatebyMatrix(TMatrix A, TVector r)
        {
            TVector r1 = new TVector(r.vector.Length);
            r1 = A * r;
            return r1;
        }
    }
}
