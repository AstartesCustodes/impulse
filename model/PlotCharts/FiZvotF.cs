using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Numerics;

namespace PlotCharts
{
    public partial class FiZvotF : Form
    {

        double PI = 3.1415926535897932;
        double Zb = 10;
        double t;
        double dE0 = 0;
        double Fi;
        double Zv;
        double F;
        public FiZvotF()
        {
            InitializeComponent();
        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }

       

        private void FiZvotF_Load(object sender, EventArgs e)
        {
           
        }
        private void button1_Click(object sender, EventArgs e)
        {



            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart1.Series[0].Color = Color.Red;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "F2";
            chart2.ChartAreas[0].AxisX.LabelStyle.Format = "F2";
            chart2.Series[0].Color = Color.Red;










            //double num = Data.fv - Data.fn;
            Complex j = new Complex(0, 1);
            //for (double f = Data.fn; f < Data.fv; f++)
            //{

            //    double w = 2 * Math.PI * f;
            //    Complex Z1 = 2 / (j * w * Data.C1);
            //    Complex Z2 = 1 / (j * w * Data.C2);
            //    Complex Z3 = j * w * Data.L;

            //    Complex Zc = Complex.Sqrt((Z1 * Z1 * (Z2 + Z3)) / (2 * Z1 + Z2 + Z3));
            //    Complex A11 = (Z1 + Z2 + Z3) / Z1;
            //    Complex phase = Complex.Log(A11 + Complex.Sqrt(A11 + 1) * Complex.Sqrt(A11 - 1));

            //    chart1.Series[0].Points.AddXY(f, Zc.Real);
            //    chart2.Series[0].Points.AddXY(f, phase.Magnitude);


            //}


            for (int it = 0; it <= Data.Nd; it++)
            {
                F = Data.fn + it * (Data.fv - Data.fn) / Data.Nd;
                double w = 2 * Math.PI * F;
                Complex Z1 = 2 / (j * w * Data.C1);
                Complex Z2 = 1 / (j * w * Data.C2);
                Complex Z3 = j * w * Data.L;
                Complex Zc = Complex.Sqrt((Z1 * Z1 * (Z2 + Z3)) / (2 * Z1 + Z2 + Z3));

                chart1.Series[0].Points.AddXY(F, Zc.Real);

            }

            for (int it = 0; it <= Data.Nd; it++)
            {
                F = Data.fn + it * (Data.fv - Data.fn) / Data.Nd;
                Fi = 2 * Math.Asin(Math.Sqrt(((F * F / (Data.fn * Data.fn)) - 1) * Data.C1 / (2 * Data.C2)));
                chart2.Series[0].Points.AddXY(F, Fi);

            }
            chart2.ChartAreas[0].AxisX.Interval = 1;
            //for (int it = 0; it <= Data.Nd; it++)
            //{
            //    F = Data.fn + it * (Data.fv - Data.fn) / Data.Nd;
            //    Zv = 2 / (2 * PI * F) / Data.C1 * Math.Sqrt(Data.C2);
            //    chart1.Series[0].Points.AddXY(Zv, F);

            //}

            //  Complex A11 = (Z1 + Z2 + Z3) / Z1;
            // Complex phase = Complex.Log(A11 + Complex.Sqrt(A11 + 1) * Complex.Sqrt(A11 - 1));
            //// chart2.Series[0].Points.AddXY(F, phase.Magnitude);
        }
    }

}
