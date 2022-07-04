using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using MyLinearAlgebra;

namespace LAB_2_OPRS_FORMS
{
    public abstract class TMath
    {
        protected TMatrix result;//матрица результата
        protected TVector x0;//вектор нач.условий
        protected int rows;//количество строк матрицы
        protected double samplingIncremet;//требуемый интервал выдачи результатов 
        protected double t;//нач. время
        public TMatrix Result { get => result; set => result = value; }
        public TVector X0 { get => x0; set => x0 = value; }
        public int Rows { get => rows; set => rows = value; }
        public double SamplingIncremet { get => samplingIncremet; set => samplingIncremet = value; }
        public double T { get => t; set => t = value; }

        public abstract TVector RightPart(double time, TVector ComeVec);
        public void PrepareResult(double T)//задаем матрицу соотв.размеров
        {
            Result = new TMatrix((int)((T - this.T) / SamplingIncremet) + 1, X0.vector.Length + 1);
            Rows = 0;
        }
        public void AddResult(TVector ComeVec, double time)//добавляем построчно результаты в матрицу
        {
            // Проверим, выходит ли счётчик строк в матрице результатов за пределы последней строки
            // Если да, то увеличим количество строк на 1
            if (Rows == Result.I)
                Result = new TMatrix(Rows + 1, ComeVec.vector.Length + 1);
            // Поместим результаты в последнюю строку матрицы Result
            // Момент времени помещается в 0-ой столбец, вектор состояния - в остальные столбцы
            Result[Rows, 0] = time;
            for (int i = ComeVec.vector.Length; i > 0; i--)
                Result[Rows, i] = ComeVec[i - 1];
            // Увеличим N
            Rows++;
        }
        public void SaveResultToFile(string filename)//добавление матрицы результатов в файл
        {
            //0-время, 1...4-решение СДУ
            FileStream file1;
            file1 = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
            file1.SetLength(0);
            StreamWriter asd1 = new StreamWriter(file1);
            for (int i = 0; i < Result.I - 1; ++i)
            {
                for (int j = 0; j < X0.vector.Length + 1; ++j)
                {
                    asd1.Write(Result[i, j] + " ");
                }
                asd1.WriteLine();
            }
            asd1.Close();
            file1.Dispose();
            file1.Close();
        }
    }
}
