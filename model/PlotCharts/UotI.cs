using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Numerics;
using System.Threading;

namespace PlotCharts
{
    public partial class UotI : Form
    {

        public UotI()
        {
            InitializeComponent();
        }
        double PI = 3.1415926535897932;
        double Zb = 10;
        double t;
        double dE0 = 0;
        double Fi;
        double Zv;
        double F;


        public double Pulse(double t)
        {
            //double start = 0;//время нач. импульса 
            //double stop = 3; // конец импульса 
            //double fwfront = 1; //  первый фронт 
            //double bwfront = 1; // второй фронт



            double ret = 0.0;
            if ((t >= Data.start) && (t <= (Data.start + Data.fwfront)))
            {
                if (Data.fwfront > 0)
                {
                    ret = 0.5 * (Math.Sin(Math.PI * (t - Data.start) / Data.fwfront)) * Math.PI / Data.fwfront;
                }
            }
            if ((t >= (Data.stop - Data.bwfront)) && (t <= Data.stop))
            {
                if (Data.bwfront > 0)
                {
                    ret = -0.5 * (Math.Sin(Math.PI * (Data.stop - t) / Data.bwfront)) * Math.PI / Data.bwfront;
                }
            }
            return ret;
            // chart2.Series[0].Points.AddXY(t, ret);
            //   Pulsik.Add(ret);



        }

        List<double> V = new List<double>();
        List<double> dV = new List<double>();
        List<double> U = new List<double>();
        List<double> dU = new List<double>();
        List<double> FL = new List<double>();
        private void button1_Click(object sender, EventArgs e)
        {
            //   List<int> Pre = new List<int>();
            V.Clear();
            dV.Clear();
            U.Clear();
            dU.Clear();
            FL.Clear();

            for (int i = 0; i < Data.Nc + 1; i++)
            {
                V.Add(0);
            }

            for (int i = 0; i < Data.Nc + 1; i++)
            {
                dV.Add(0);
            }

            for (int i = 0; i < Data.Nc; i++)
            {
                U.Add(0);
            }

            for (int i = 0; i < Data.Nc; i++)
            {
                dU.Add(0);
            }
            //int step = 20;
            //int discr = Convert.ToInt32(Data.Nt);
            //int[] A = new int[1 + discr];

            //int a=0, b=discr, c=0;

            //for (int z = 0; z <= Data.Nt; z++)
            //{
            //    Pre.Add(0);
            //}

            //for (int i = 0; i <= discr; i++)
            //{
            //   while( Pre.Count > Pre[i + step*2] )
            //   A[i] =  Pre[i+step];

            //}
            progressBar1.Maximum = Convert.ToInt32(Data.Nt) + 1;
            progressBar1.Value = 0;
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "F2";
            chart2.ChartAreas[0].AxisY.LabelStyle.Format = "F2";

            //int L;
            //L = 1;

            //double otn;
            //otn = Data.Nt / 5;
            //otn = Math.Round(otn, 0);
            //L = 1 - Convert.ToInt32(otn);





            //for (int it = 0; it < Data.Nt; it++)
            //{
            //        L = L + Convert.ToInt32(otn);



            //}


            for (int it = 0; it <= Data.Nt; it++)
            {
                //  int a;
                if (trackBar1.Value == it)
                {
                    //        a = trackBar1.Value;
                    break;
                    // Thread.Sleep(1000);

                }

                progressBar1.Value++;
                t = it * Data.dt;

                dE0 = -2 * PI * Data.f0 * Data.Um0 * Math.Sin(2 * PI * Data.f0 * t) + Data.A0 * Pulse(t);

                dV[0] += ((2.0 / (Data.R0 * Data.C1)) * (dE0 - dV[0])) * Data.dt + ((2.0 / (Data.L * Data.C1)) * (V[1] - V[0] + U[0])) * Data.dt;
                dV[Data.Nc] += (2.0 / (Data.Rn * Data.C1)) * (0 - dV[Data.Nc]) * Data.dt + (2.0 / (Data.L * Data.C1)) * (V[Data.Nc - 1] - V[Data.Nc] - U[Data.Nc - 1]) * Data.dt;//dEn = 0, En = Um0*cos(2*PI*f*t)

                for (int i = 0; i < Data.Nc; i++)
                {
                    dU[i] += ((1.0 / (Data.L * Data.C2)) * (V[i] - V[i + 1] - U[i])) * Data.dt - 1.0 / (Data.R * Data.C2) * dU[i] * Data.dt;
                    if (i == 0) continue;
                    dV[i] += ((1.0 / (Data.L * Data.C1)) * (V[i - 1] - 2.0 * V[i] + V[i + 1] + U[i] - U[i - 1]) * Data.dt);
                }
                chart1.Series[0].Points.Clear();
                chart2.Series[0].Points.Clear();
                double q = 0, p = 0, l = 0;
                double a = 0, b = 0, c = 0;

                for (int i = 0; i < Data.Nc; i++)
                {

                    V[i] += dV[i] * Data.dt;
                    U[i] += dU[i] * Data.dt;
                    chart1.Series[0].Points.AddXY(i, V[i]);
                    q = (V[i]);
                    chart2.Series[0].Points.AddXY(i, U[i]);
                    a = (U[i]);

                    if (q > p)
                    {
                        p = q;
                    }
                    if (q < l)
                    {
                        l = q;
                    }




                    if (a > b)
                    {
                        b = a;
                    }
                    if (a < c)
                    {
                        c = a;
                    }
                }
                if (b > chart2.ChartAreas[0].AxisY.Maximum)
                {
                    chart2.ChartAreas[0].AxisY.Maximum = b;

                }
                if (c < chart2.ChartAreas[0].AxisY.Minimum)
                {
                    chart2.ChartAreas[0].AxisY.Minimum = c;

                }
                if (p > chart1.ChartAreas[0].AxisY.Maximum)
                {
                    chart1.ChartAreas[0].AxisY.Maximum = p;

                }
                if (l < chart1.ChartAreas[0].AxisY.Minimum)
                {
                    chart1.ChartAreas[0].AxisY.Minimum = l;

                }




                V[Data.Nc] += dV[Data.Nc] * Data.dt;
                chart1.Series[0].Points.AddXY(Data.Nc, V[Data.Nc]);
                chart2.Update();
                chart1.Update();
                //Thread.Sleep(20);
            }

        }

        private void UotI_Load(object sender, EventArgs e)
        {
            trackBar1.Maximum = Convert.ToInt32(Data.Nt) + 1;
            chart1.ChartAreas[0].AxisY.Maximum = 0.001;
            chart1.ChartAreas[0].AxisY.Minimum = -0.001;
            chart2.ChartAreas[0].AxisY.Maximum = 0.001;
            chart2.ChartAreas[0].AxisY.Minimum = -0.001;
            chart1.Series[0].Color = Color.Red;
            chart1.Series["1"].Color = Color.Red;
            chart1.Series["3"].Color = Color.Red;
            chart2.Series[0].Color = Color.Blue;
            chart2.Series["1"].Color = Color.Blue;
            chart2.Series["3"].Color = Color.Blue;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            chart1.SaveImage("F:\\novapraim\\datau.png", ChartImageFormat.Gif);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            trackBar1.Value++;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
           

        }

        private void button4_Click(object sender, EventArgs e)
        {
            V.Clear();
            dV.Clear();
            U.Clear();
            dU.Clear();
            FL.Clear();

            for (int i = 0; i < Data.Nc + 1; i++)
            {
                V.Add(0);
            }

            for (int i = 0; i < Data.Nc + 1; i++)
            {
                dV.Add(0);
            }

            for (int i = 0; i < Data.Nc; i++)
            {
                U.Add(0);
            }

            for (int i = 0; i < Data.Nc; i++)
            {
                dU.Add(0);
            }
            //int step = 20;
            //int discr = Convert.ToInt32(Data.Nt);
            //int[] A = new int[1 + discr];

            //int a=0, b=discr, c=0;

            //for (int z = 0; z <= Data.Nt; z++)
            //{
            //    Pre.Add(0);
            //}

            //for (int i = 0; i <= discr; i++)
            //{
            //   while( Pre.Count > Pre[i + step*2] )
            //   A[i] =  Pre[i+step];

            //}
            progressBar1.Maximum = Convert.ToInt32(Data.Nt) + 1;
            progressBar1.Value = 0;
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "F2";
            chart2.ChartAreas[0].AxisY.LabelStyle.Format = "F2";

            //int L;
            //L = 1;

            //double otn;
            //otn = Data.Nt / 5;
            //otn = Math.Round(otn, 0);
            //L = 1 - Convert.ToInt32(otn);





            //for (int it = 0; it < Data.Nt; it++)
            //{
            //        L = L + Convert.ToInt32(otn);



            //}


            for (int it = 0; it <= Data.Nt; it++)
            {
                //  int a;
                if (trackBar1.Value == it)
                {
                    //        a = trackBar1.Value;
                    break;
                    // Thread.Sleep(1000);

                }

                progressBar1.Value++;
                t = it * Data.dt;

                dE0 = -2 * PI * Data.f0 * Data.Um0 * Math.Sin(2 * PI * Data.f0 * t) + Data.A0 * Pulse(t);

                dV[0] += ((2.0 / (Data.R0 * Data.C1)) * (dE0 - dV[0])) * Data.dt + ((2.0 / (Data.L * Data.C1)) * (V[1] - V[0] + U[0])) * Data.dt;
                dV[Data.Nc] += (2.0 / (Data.Rn * Data.C1)) * (0 - dV[Data.Nc]) * Data.dt + (2.0 / (Data.L * Data.C1)) * (V[Data.Nc - 1] - V[Data.Nc] - U[Data.Nc - 1]) * Data.dt;//dEn = 0, En = Um0*cos(2*PI*f*t)

                for (int i = 0; i < Data.Nc; i++)
                {
                    dU[i] += ((1.0 / (Data.L * Data.C2)) * (V[i] - V[i + 1] - U[i])) * Data.dt - 1.0 / (Data.R * Data.C2) * dU[i] * Data.dt;
                    if (i == 0) continue;
                    dV[i] += ((1.0 / (Data.L * Data.C1)) * (V[i - 1] - 2.0 * V[i] + V[i + 1] + U[i] - U[i - 1]) * Data.dt);
                }
                chart1.Series["1"].Points.Clear();
                chart2.Series["1"].Points.Clear();
                double q = 0, p = 0, l = 0;
                double a = 0, b = 0, c = 0;
              

                for (int i = 0; i < Data.Nc; i++)
                {

                    V[i] += dV[i] * Data.dt;
                    U[i] += dU[i] * Data.dt;
                    chart1.Series["1"].Points.AddXY(i, V[i]);
                    q = (V[i]);
                    chart2.Series["1"].Points.AddXY(i, U[i]);
                    a = (U[i]);

                    if (q > p)
                    {
                        p = q;
                    }
                    if (q < l)
                    {
                        l = q;
                    }




                    if (a > b)
                    {
                        b = a;
                    }
                    if (a < c)
                    {
                        c = a;
                    }
                }
                if (b > chart2.ChartAreas[0].AxisY.Maximum)
                {
                    chart2.ChartAreas[0].AxisY.Maximum = b;

                }
                if (c < chart2.ChartAreas[0].AxisY.Minimum)
                {
                    chart2.ChartAreas[0].AxisY.Minimum = c;

                }
                if (p > chart1.ChartAreas[0].AxisY.Maximum)
                {
                    chart1.ChartAreas[0].AxisY.Maximum = p;

                }
                if (l < chart1.ChartAreas[0].AxisY.Minimum)
                {
                    chart1.ChartAreas[0].AxisY.Minimum = l;

                }




                V[Data.Nc] += dV[Data.Nc] * Data.dt;
                chart1.Series["1"].Points.AddXY(Data.Nc, V[Data.Nc]);
                chart2.Update();
                chart1.Update();
                //Thread.Sleep(20);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            V.Clear();
            dV.Clear();
            U.Clear();
            dU.Clear();
            FL.Clear();

            for (int i = 0; i < Data.Nc + 1; i++)
            {
                V.Add(0);
            }

            for (int i = 0; i < Data.Nc + 1; i++)
            {
                dV.Add(0);
            }

            for (int i = 0; i < Data.Nc; i++)
            {
                U.Add(0);
            }

            for (int i = 0; i < Data.Nc; i++)
            {
                dU.Add(0);
            }
            //int step = 20;
            //int discr = Convert.ToInt32(Data.Nt);
            //int[] A = new int[1 + discr];

            //int a=0, b=discr, c=0;

            //for (int z = 0; z <= Data.Nt; z++)
            //{
            //    Pre.Add(0);
            //}

            //for (int i = 0; i <= discr; i++)
            //{
            //   while( Pre.Count > Pre[i + step*2] )
            //   A[i] =  Pre[i+step];

            //}
            progressBar1.Maximum = Convert.ToInt32(Data.Nt) + 1;
            progressBar1.Value = 0;
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "F2";
            chart2.ChartAreas[0].AxisY.LabelStyle.Format = "F2";

            //int L;
            //L = 1;

            //double otn;
            //otn = Data.Nt / 5;
            //otn = Math.Round(otn, 0);
            //L = 1 - Convert.ToInt32(otn);





            //for (int it = 0; it < Data.Nt; it++)
            //{
            //        L = L + Convert.ToInt32(otn);



            //}


            for (int it = 0; it <= Data.Nt; it++)
            {
                //  int a;
                if (trackBar1.Value == it)
                {
                    //        a = trackBar1.Value;
                    break;
                    // Thread.Sleep(1000);

                }

                progressBar1.Value++;
                t = it * Data.dt;

                dE0 = -2 * PI * Data.f0 * Data.Um0 * Math.Sin(2 * PI * Data.f0 * t) + Data.A0 * Pulse(t);

                dV[0] += ((2.0 / (Data.R0 * Data.C1)) * (dE0 - dV[0])) * Data.dt + ((2.0 / (Data.L * Data.C1)) * (V[1] - V[0] + U[0])) * Data.dt;
                dV[Data.Nc] += (2.0 / (Data.Rn * Data.C1)) * (0 - dV[Data.Nc]) * Data.dt + (2.0 / (Data.L * Data.C1)) * (V[Data.Nc - 1] - V[Data.Nc] - U[Data.Nc - 1]) * Data.dt;//dEn = 0, En = Um0*cos(2*PI*f*t)

                for (int i = 0; i < Data.Nc; i++)
                {
                    dU[i] += ((1.0 / (Data.L * Data.C2)) * (V[i] - V[i + 1] - U[i])) * Data.dt - 1.0 / (Data.R * Data.C2) * dU[i] * Data.dt;
                    if (i == 0) continue;
                    dV[i] += ((1.0 / (Data.L * Data.C1)) * (V[i - 1] - 2.0 * V[i] + V[i + 1] + U[i] - U[i - 1]) * Data.dt);
                }
                chart1.Series["3"].Points.Clear();
                chart2.Series["3"].Points.Clear();
                double q = 0, p = 0, l = 0;
                double a = 0, b = 0, c = 0;


                for (int i = 0; i < Data.Nc; i++)
                {

                    V[i] += dV[i] * Data.dt;
                    U[i] += dU[i] * Data.dt;
                    chart1.Series["3"].Points.AddXY(i, V[i]);
                    q = (V[i]);
                    chart2.Series["3"].Points.AddXY(i, U[i]);
                    a = (U[i]);

                    if (q > p)
                    {
                        p = q;
                    }
                    if (q < l)
                    {
                        l = q;
                    }




                    if (a > b)
                    {
                        b = a;
                    }
                    if (a < c)
                    {
                        c = a;
                    }
                }
                if (b > chart2.ChartAreas[0].AxisY.Maximum)
                {
                    chart2.ChartAreas[0].AxisY.Maximum = b;

                }
                if (c < chart2.ChartAreas[0].AxisY.Minimum)
                {
                    chart2.ChartAreas[0].AxisY.Minimum = c;

                }
                if (p > chart1.ChartAreas[0].AxisY.Maximum)
                {
                    chart1.ChartAreas[0].AxisY.Maximum = p;

                }
                if (l < chart1.ChartAreas[0].AxisY.Minimum)
                {
                    chart1.ChartAreas[0].AxisY.Minimum = l;

                }




                V[Data.Nc] += dV[Data.Nc] * Data.dt;
                chart1.Series["3"].Points.AddXY(Data.Nc, V[Data.Nc]);
                chart2.Update();
                chart1.Update();
                //Thread.Sleep(20);
            }
        }
    }
}
