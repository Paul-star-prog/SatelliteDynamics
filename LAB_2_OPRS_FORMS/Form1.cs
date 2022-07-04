using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MyLinearAlgebra;



namespace LAB_2_OPRS_FORMS
{
    public partial class GravitationModels : Form
    {
        TIntegrtor integrator;
        TMath math;
        private struct Param
        {
            public static double W, Theta, Omega, I, E, A, delta, Tk;
            public static int index, index1, ind;
            public static TMatrix Result, InaccuracyResult;
            public static List<int> FactorsIndex = new List<int>();
            public const double u_moon = 4901.783, u_sun = 1.326663E+11;
        }

        public delegate void MyDelegate(Chart chart,double[] x, double[] y, double xmin, double xmax, string name, string name1);

        public void Izmeni(Chart chart, double[] x, double[] y, double xmin, double xmax, string name, string name1)
        {
            chart.ChartAreas[0].AxisX.Minimum = xmin;
            chart.ChartAreas[0].AxisX.Maximum = xmax;
            chart.Series[0].Name = "Evolution " + name + " of " + name1;
            chart.Series[0].Points.DataBindXY(x, y);
        }

        public GravitationModels()
        {
            InitializeComponent();
            integrator = new DormandPrince();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            Param.ind = 0;
        }

        private bool check_fill_oscular()
        {
            return textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" ||
                textBox1.Text == "-" || textBox2.Text == "-" || textBox3.Text == "-" || textBox4.Text == "-" || textBox5.Text == "-" || textBox6.Text == "-" ||
                textBox1.Text == "," || textBox2.Text == "," || textBox3.Text == "," || textBox4.Text == "," || textBox5.Text == "," || textBox6.Text == "," ||
                textBox1.Text == "-," || textBox2.Text == "-," || textBox3.Text == "-," || textBox4.Text == "-," || textBox5.Text == "-," || textBox6.Text == "-," ||
                textBox1.Text == ",-" || textBox2.Text == ",-" || textBox3.Text == ",-" || textBox4.Text == ",-" || textBox5.Text == ",-" || textBox6.Text == ",-";
        }

        private bool check_val_oscular()
        {
            double temp1 = Convert.ToDouble(textBox1.Text);
            double temp2 = Convert.ToDouble(textBox2.Text);
            double temp3 = Convert.ToDouble(textBox3.Text);
            double temp4 = Convert.ToDouble(textBox4.Text);
            double temp5 = Convert.ToDouble(textBox5.Text);
            double temp6 = Convert.ToDouble(textBox6.Text);
            return (temp1 < 6500 || temp1 > 50000 || temp2 < 0 || temp2 > 0.5 || temp3 < 0 || temp3 > Math.PI/2||
                temp4 < - Math.PI / 2 || temp4 > Math.PI / 2 || temp5 < 0 || temp5 > Math.PI * 2 || temp6 < 0 || temp6 > Math.PI * 2);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!check_fill_oscular())
            {
                if (!check_val_oscular())
                {
                    saveOrbitParam();
                    button4.Enabled = true;
                    MessageBox.Show("Параметры сохранены");
                }
                else MessageBox.Show("Заданные параметры содержат недопустимые значения");
            }
            else MessageBox.Show("Невозможно сохранить, проверьте что все поля заполнены");
        }

        private void GravitationModels_Load(object sender, EventArgs e)
        {
        }
       
        private void chartLoadData(Chart chart, string name,string name1, double[] x, double[] y, double xmin, double xmax)
        {
            BeginInvoke(new MyDelegate(Izmeni), chart, x, y, xmin, xmax, name, name1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 5)
            {
                int count = Param.Result.I;
                double[] x = new double[count];
                double[] y = new double[count];
                for (int i = 0; i < count; i++)
                {
                    x[i] = Param.Result[i, 1];
                    y[i] = Param.Result[i, 2];
                }
                chart1.Series[0].Color = Color.Red;
                double xmin = x.Min();
                double xmax = x.Max();
                Task second = Task.Run(() => chartLoadData(chart1,"Y(t)", "X(t)", x, y, xmin, xmax));
                GC.Collect(2, GCCollectionMode.Forced);
            }
            if ((comboBox1.SelectedIndex != 4) & (comboBox1.SelectedIndex != 5))
            {
                int count = Param.Result.I;
                getIndex();
                int index = Param.index;
                int index1 = Param.index1;
                string name = comboBox2.Text;
                string name1 = comboBox3.Text;
                double[] x = new double[count];
                double[] y = new double[count];
                for (int i = 0; i < count; i++)
                {
                    x[i] = Param.Result[i, index];
                    y[i] = Param.Result[i, index1];
                }
                chart1.Series[0].Color = Color.Red;
                double xmin = x.Min();
                double xmax = x.Max();
                Task second = Task.Run(() => chartLoadData(chart1,name, name1, x, y, xmin, xmax));
                GC.Collect(2, GCCollectionMode.Forced);
            }
            if (comboBox1.SelectedIndex == 4)
            {
                // Луна (геоцентр)
                int count = Param.Result.I;
                int index = comboBox3.SelectedIndex;
                int index1 = comboBox2.SelectedIndex + 1;
                string name = comboBox2.Text;
                string name1 = comboBox3.Text;
                double[] x = new double[count];
                double[] y = new double[count];
                if (index == 0)
                {   // если брать по времени
                    for (int i = 0; i < count; i++)
                    {
                        x[i] = Param.Result[i, index];
                        y[i] = Param.Result[i, index1] - Param.Result[i, index1 + 6];
                    }
                }
                if (index != 0)
                {   // если брать по коорд
                    for (int i = 0; i < count; i++)
                    {
                        x[i] = Param.Result[i, index] - Param.Result[i, index + 6];
                        y[i] = Param.Result[i, index1] - Param.Result[i, index1 + 6];
                    }
                }
                chart1.Series[0].Color = Color.Red;
                double xmin = x.Min();
                double xmax = x.Max();
                Task second = Task.Run(() => chartLoadData(chart1, name, name1, x, y, xmin, xmax));
                GC.Collect(2, GCCollectionMode.Forced);
            }
        }

        private void getIndex()
        {
            if (comboBox1.SelectedIndex == 2)
            {   // Земля (гелиоцентр)
                Param.index1 = comboBox2.SelectedIndex + 1 + 6;
                if (comboBox3.SelectedIndex == 0)
                {
                    Param.index = 0;
                }
                else
                {
                    Param.index = comboBox3.SelectedIndex + 6;
                }
            }
            if (comboBox1.SelectedIndex == 3)
            {   // Луна (гелиоцентр)
                Param.index1 = comboBox2.SelectedIndex + 1;
                Param.index = comboBox3.SelectedIndex;
            }
            if ((comboBox1.SelectedIndex == 0) | (comboBox1.SelectedIndex == 1) | (comboBox1.SelectedIndex == 5))
            {
                // ИСЗ + аренсторф
                Param.index1 = comboBox2.SelectedIndex + 1;
                Param.index = comboBox3.SelectedIndex;
            }
        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            panel1.Visible = true;
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

        private void getModel()
        {
            if (comboBox1.SelectedIndex == 0) { math = new SatelliteCentralGravitation(Param.W, Param.Theta, Param.Omega, Param.I, Param.E, Param.A); }
            if (comboBox1.SelectedIndex == 1) { math = new SatelliteNormalGravitation(Param.W, Param.Theta, Param.Omega, Param.I, Param.E, Param.A); }
            if ((comboBox1.SelectedIndex == 2) | (comboBox1.SelectedIndex == 3) | (comboBox1.SelectedIndex == 4))
            { math = new ThreeBody(); };
            if (comboBox1.SelectedIndex == 5) { math = new Arenstorf(); }
        }

        private void saveOrbitParam()
        {
            Param.A = Convert.ToDouble(textBox1.Text);
            Param.E = Convert.ToDouble(textBox2.Text);
            Param.I = Convert.ToDouble(textBox3.Text);
            Param.W = Convert.ToDouble(textBox4.Text);
            Param.Theta = Convert.ToDouble(textBox5.Text);
            Param.Omega = Convert.ToDouble(textBox6.Text);
        }

        private void getStep(double TK)
        {
            if ((comboBox1.SelectedIndex == 0) | (comboBox1.SelectedIndex == 1)) { Param.delta = 60; Param.Tk = TK; }
            if ((comboBox1.SelectedIndex == 2) | (comboBox1.SelectedIndex == 3) | (comboBox1.SelectedIndex == 4))
            {
                double col_day = TK;
                // конечное время интегрирования
                Param.Tk = col_day * 86400 ;
                // шаг интегрирования
                Param.delta = 60; 
            }
            if (comboBox1.SelectedIndex == 5) { Param.delta = 0.01; Param.Tk = TK * 17.0652165601579625; }
        }

        private void setScyBodyDecorator(List<TDecorator> decorators, Satellite Satellite1, TMatrix LocaleCoord, double locale_u )
        {
            if (decorators.Count != 0)
            {
                decorators.Add(new ScyBodyDecorator(Param.W, Param.Theta, Param.Omega, Param.I, Param.E, Param.A, decorators[decorators.Count - 1], LocaleCoord, locale_u));
            }
            else decorators.Add(new ScyBodyDecorator(Param.W, Param.Theta, Param.Omega, Param.I, Param.E, Param.A, Satellite1, LocaleCoord, locale_u));
        }

        private void setSunPressureDecorator(List<TDecorator> decorators, Satellite Satellite1, TMatrix LocaleCoord)
        {
            if (decorators.Count != 0)
            {
                decorators.Add(new SunPressureDecorator(Param.W, Param.Theta, Param.Omega, Param.I, Param.E, Param.A, decorators[decorators.Count - 1], LocaleCoord));
            }
            else decorators.Add(new SunPressureDecorator(Param.W, Param.Theta, Param.Omega, Param.I, Param.E, Param.A, Satellite1, LocaleCoord));
        }

        private TDecorator addsatelliteFactors(TMatrix Result)
        {
            Satellite Satellite1 = new SatelliteNormalGravitation(Param.W, Param.Theta, Param.Omega, Param.I, Param.E, Param.A);
            List<TDecorator> decorators = new List<TDecorator>();
            for (int i = 0; i < Param.FactorsIndex.Count; i++)
            {
                    switch (Param.FactorsIndex[i])
                    {
                        case 0:
                          setScyBodyDecorator(decorators, Satellite1, moonCoord(Result), Param.u_moon);
                        break;
                        case 1:
                          setScyBodyDecorator(decorators, Satellite1, sunCoord(Result), Param.u_sun);
                        break;
                        case 2:
                          setSunPressureDecorator(decorators, Satellite1, sunCoord(Result));
                        break;
                    }               
            }
            return  decorators[decorators.Count - 1];
        }
        private TMatrix sunCoord(TMatrix Result)
        {
            TMatrix Coord = new TMatrix(Result.I, 4);
            for (int i = 0; i < Result.I; i++)
            {
                Coord[i, 0] = Result[i, 0];
                for (int j = 1; j < 4; j++)
                 Coord[i, j] = -Result[i, j + 6];
            }
            return Coord;
        }
        private TMatrix moonCoord(TMatrix Result)
        {
            TMatrix Coord = new TMatrix(Result.I, 4);
            for (int i = 0; i < Result.I; i++)
            {
                Coord[i, 0] = Result[i, 0];
                for (int j = 1; j < 4; j++)
                 Coord[i, j] = Result[i, j] - Result[i, j + 6];
            }
            return Coord;
        }
        private void satelliteWithFactors(double delta, double TK, out TMatrix Result)
        {
            // моделирование 3 тел
            math = new ThreeBody();
            integrator.Run(math, delta, TK);
            // моделирование ИСЗ с выбранными факторами
            TDecorator decorator = addsatelliteFactors(math.Result);
            integrator.Run(decorator, delta, TK);
            Result = decorator.Result;
            decorator.Result = null;
            decorator = null;
            math = null;
            GC.Collect(2, GCCollectionMode.Forced);
        }
        private void check_numbers(object sender, KeyPressEventArgs e, string text)
        {
            char number = e.KeyChar;
            if (((number <= 47 || number >= 58) && number != 8 && number != 44))
            {
                e.Handled = true;
            }
            if (number == 45 && text == "")
            {
                e.Handled = false;
            }
            if (number == 44 && text.Contains(","))
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            check_numbers(sender, e, textBox1.Text);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            check_numbers(sender, e, textBox2.Text);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            check_numbers(sender, e, textBox3.Text);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            check_numbers(sender, e, textBox4.Text);
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            check_numbers(sender, e, textBox5.Text);
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            check_numbers(sender, e, textBox6.Text);
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            check_numbers(sender, e, textBox7.Text);
        }

        async void button4_Click_1(object sender, EventArgs e)
        {
            if (textBox7.Text != "" && textBox7.Text != "-")
            {
                if (Convert.ToDouble(textBox7.Text) >= 0)
                {
                    button5.Enabled = false;
                    button2.Enabled = true;
                    getStep(Convert.ToDouble(textBox7.Text));
                    progressBar2.Increment(30);
                    double TK = Param.Tk;
                    double delta = Param.delta;
                    bool flaq = false;
                    if (((comboBox1.SelectedIndex == 0) || (comboBox1.SelectedIndex == 1)) && (checkedListBox1.CheckedItems.Count != 0))
                    { flaq = true; }
                    else { getModel(); }
                    progressBar2.Increment(30);
                    await Task.Run(() =>
                    {
                       bool f = true;
                       while (f)
                       {
                         if (flaq)
                         { satelliteWithFactors(delta, TK, out Param.Result); }
                          else {integrator.Run(math, delta, TK); }
                         f = false;
                       }
                    });
                    progressBar2.Increment(40);
                    MessageBox.Show("Моделирование Завершено");
                    if (math != null)
                    {
                        Param.Result = math.Result;
                        math.Result = null;
                        math = null;
                    }
                    if (comboBox1.SelectedIndex == 1) { button5.Enabled = true; }
                    GC.Collect(2, GCCollectionMode.Forced);
                    progressBar2.Maximum = 0;
                    progressBar2.Maximum = 100;
                }
                else MessageBox.Show("Невозможно провести моделирование: время меньше 0");
            }
            else MessageBox.Show("Невозможно провести моделирование: время не задано");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((comboBox1.SelectedIndex == 0) | (comboBox1.SelectedIndex == 1))
            { label11.Text = "Время наблюдения (сек)"; comboBox2.Enabled = true; comboBox3.Enabled = true; }
            if ((comboBox1.SelectedIndex == 2) | (comboBox1.SelectedIndex == 3) | (comboBox1.SelectedIndex == 4))
            { label11.Text = "Время наблюдения (дни)"; comboBox2.Enabled = true; comboBox3.Enabled = true; }
            if (comboBox1.SelectedIndex == 5)
            { label11.Text = "Количество полных витков"; comboBox2.Enabled = false; comboBox3.Enabled = false; }
        }

        private void checkedListBox1_Click(object sender, EventArgs e)
        {
           if ((checkedListBox1.GetItemCheckState(checkedListBox1.SelectedIndex) == CheckState.Checked) && (checkedListBox1.SelectedIndex != -1)) 
           { checkedListBox1.SetItemCheckState(checkedListBox1.SelectedIndex, CheckState.Unchecked); Param.FactorsIndex.Remove(checkedListBox1.SelectedIndex); }
           else { checkedListBox1.SetItemCheckState(checkedListBox1.SelectedIndex, CheckState.Checked); Param.FactorsIndex.Add(checkedListBox1.SelectedIndex); }
        }

        private TMatrix getInaccuracyData(TMatrix Perturbate, TMatrix Unperturbate)
        {
            // Невязка между коорд. в невозмущ. поле ГПЗ и коорд. в возмущ. ГПЗ 
            TMatrix Result = new TMatrix(Perturbate.I, Perturbate.J);
            for (int i = 0; i < Result.I; i++)
            {
                for (int j = 0; j < Result.J; j++)
                {
                    Result[i, j] = Unperturbate[i, j] - Perturbate[i, j];
                }
            }
            return Result;
        }
        private TMatrix CalcInaccuracy()
        {
            // ИСЗ в невозмущенном ГПЗ
            Satellite satellite_unperturbate = new SatelliteCentralGravitation(Param.W, Param.Theta, Param.Omega, Param.I, Param.E, Param.A);
            // интегрирование ИСЗ в невозмущенном ГПЗ
            integrator.Run(satellite_unperturbate, Param.delta, Param.Tk);
            return getInaccuracyData(Param.Result, satellite_unperturbate.Result);
        }
        async void button5_Click(object sender, EventArgs e)
        {
                button6.Enabled = true;
                progressBar3.Increment(30);
                await Task.Run(() =>
                {
                    bool f = true;
                    while (f)
                    {
                        Param.InaccuracyResult = CalcInaccuracy();
                        f = false;
                    }
                });
                progressBar3.Increment(70);
                MessageBox.Show("Расчет невязки завершен");
                GC.Collect(2, GCCollectionMode.Forced);
                progressBar3.Maximum = 0;
                progressBar3.Maximum = 100;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int count = Param.Result.I;
            double[] x = new double[count];
            double[] y = new double[count];
            for (int i = 0; i < count; i++)
            {
                x[i] = Param.Result[i, 0];
                y[i] = Param.InaccuracyResult[i, comboBox4.SelectedIndex + 1];
            }
            chart2.Series[0].Color = Color.Red;
            double xmin = x.Min();
            double xmax = x.Max();
            string name = Convert.ToString(comboBox4.SelectedItem);
            Task task = Task.Run(() => chartLoadData(chart2, name, "time", x, y, xmin, xmax));
            GC.Collect(2, GCCollectionMode.Forced);
        }
    }
}
