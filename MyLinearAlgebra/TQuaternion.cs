using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLinearAlgebra
{
    public class TQuaternion
    {
        //скалярная часть кватерниона
        protected double q0;
        //векторная часть кватерниона
        protected TVector q;
        public double Q0 { get => q0; set => q0 = value; }
        public TVector Q { get => q; set => q = value; }
        public TQuaternion()
        { }
        public TQuaternion(double l0, double l1, double l2, double l3)
        {
            Q0 = l0;
            Q = new TVector(3);
            Q[0] = l1;
            Q[1] = l2;
            Q[2] = l3;
        }
        public TQuaternion(double phi, TVector e)
        {
            Q0 = Math.Cos(phi / 2);
            if (e.vector.Length == 3)
            {
                Q = e;
                Q = Q.Rationing(Q);
                Q = Math.Sin(phi / 2) * Q;
            }
            else
            {
                Q0 = double.NaN;
                Q = null;
            }

        }
        public TQuaternion(TQuaternion A)
        {
            Q0 = A.Q0;
            Q = A.Q;
        }
        public double this[int index]
        {
            get
            {
                if (index == 0) { return Q0; };
                if ((index >= 1) && (index <= 3)) { return Q[index - 1]; }
                else { return double.NaN; };
            }
            set
            {
                if (index == 0) { Q0 = value; };
                if ((index >= 1) && (index <= 3)) { Q[index - 1] = value; }
                else { };
            }
        }
        public TVector GetVec()
        {
            return Q;
        }
        public double GetScal()
        {
            return Q0;
        }
        public static TQuaternion operator +(TQuaternion A, TQuaternion B)
        {
            TQuaternion C = new TQuaternion();
            C[0] = A[0] + B[0];
            C.Q = A.Q + B.Q;
            return C;
        }
        public static TQuaternion operator -(TQuaternion A, TQuaternion B)
        {
            TQuaternion C = new TQuaternion();
            C[0] = A[0] - B[0];
            C.Q = A.Q - B.Q;
            return C;
        }
        public static TQuaternion operator *(TQuaternion A, TQuaternion B)
        {
            TQuaternion C = new TQuaternion();
            C[0] = A[0] * B[0] - A.Q.Scal(A.Q, B.Q);
            C.Q = A[0] * B.Q + B[0] * A.Q + A.Q * B.Q;
            return C;
        }
        public static TQuaternion operator *(double a, TQuaternion A)
        {
            TQuaternion C = new TQuaternion();
            C[0] = a * A[0];
            C.Q = a * A.Q;
            return C;
        }
        public static TQuaternion operator *(TQuaternion A, TVector B)
        {
            TQuaternion C = new TQuaternion();
            C[0] = -1 * (A.Q.Scal(A.Q, B));
            if (B.vector.Length == 3)
            {
                C.Q = new TVector(3);
                C[1] = A[0] * B[0] + A[2] * B[2] - A[3] * B[1];
                C[2] = A[0] * B[1] - A[1] * B[2] + A[3] * B[0];
                C[3] = A[0] * B[2] + A[1] * B[1] - A[2] * B[0];
                return C;
            }
            else
            {
                return null;
            }
        }
        public TQuaternion conj()
        {
            TQuaternion C = new TQuaternion();
            C.Q0 = Q0;
            C.Q = -1 * Q;
            return C;
        }
        public double Norma()
        {
            double norma = 0;
            norma = Math.Pow(Q0, 2) + Math.Pow(Q[0], 2) + Math.Pow(Q[1], 2) + Math.Pow(Q[2], 2);
            return norma;
        }
        public double Module()
        {
            double module = 0;
            module = Math.Sqrt(Norma());
            return module;
        }
        public TQuaternion Inverse()
        {
            TQuaternion C = new TQuaternion();
            C = conj();
            C.Q0 = Q0 * (1 / Norma());
            C.Q = (1 / Norma()) * C.Q;
            return C;
        }
        public TQuaternion Rationing()
        {
            TQuaternion C = new TQuaternion();
            C.Q0 = Q0 * (1 / Module());
            C.Q = (1 / Module()) * Q;
            return C;
        }
        public bool CheckRationing()
        {
            bool flaq;
            flaq = true;
            double s = Math.Pow(Q0, 2) + Math.Pow(Q[0], 2) + Math.Pow(Q[1], 2) + Math.Pow(Q[2], 2);
            if (Math.Abs(1 - s) < 0.01) { flaq = true; Console.WriteLine("Нормировка кватерниона прошла удачно!"); } else { flaq = false; Console.WriteLine("Нормировка кватениона прошла неудачно!"); };
            return flaq;
        }
        public TQuaternion fromKrylovAngles(double roll, double pitch, double yaw)
        {
            TQuaternion result = new TQuaternion();
            TQuaternion temp = new TQuaternion();
            TQuaternion Roll = new TQuaternion();
            TQuaternion Pitch = new TQuaternion();
            TQuaternion Yaw = new TQuaternion();
            Roll[0] = Math.Cos(roll / 2);
            Roll[1] = Math.Sin(roll / 2);
            Roll[2] = 0;
            Roll[3] = 0;
            Pitch[0] = Math.Cos(pitch / 2);
            Pitch[1] = 0;
            Pitch[2] = Math.Sin(pitch / 2);
            Pitch[3] = 0;
            Yaw[0] = Math.Cos(yaw / 2);
            Yaw[1] = 0;
            Yaw[2] = 0;
            Yaw[3] = Math.Sin(yaw / 2);
            temp = Yaw * Pitch;
            result = temp * Roll;
            return result;
        }
        public TMatrix toMatrix()
        {
            TQuaternion C = new TQuaternion();
            C.Q0 = Q0;
            C.Q = Q;
            C = Rationing();
            TMatrix result = new TMatrix(3, 3);
            result[0, 0] = Math.Pow(C.Q0, 2) + Math.Pow(C[1], 2) - Math.Pow(C[2], 2) - Math.Pow(C[3], 2);
            result[0, 1] = 2 * C.Q0 * C[1] - 2 * C[3] * C.Q0;
            result[0, 2] = 2 * C[1] * C[3] + 2 * C[2] * C.Q0;
            result[1, 0] = 2 * C[1] * C[2] + 2 * C[3] * C.Q0;
            result[1, 1] = Math.Pow(C.Q0, 2) - Math.Pow(C[1], 2) + Math.Pow(C[2], 2) - Math.Pow(C[3], 2);
            result[1, 2] = 2 * C[2] * C[3] - 2 * C[1] * C.Q0;
            result[2, 0] = 2 * C[1] * C[3] - 2 * C[2] * C.Q0;
            result[2, 1] = 2 * C[2] * C[3] + 2 * C[1] * C.Q0;
            result[2, 2] = Math.Pow(C.Q0, 2) - Math.Pow(C[1], 2) - Math.Pow(C[2], 2) + Math.Pow(C[3], 2);
            return result;
        }
    }
}
