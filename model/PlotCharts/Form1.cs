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
namespace PlotCharts
{




  


    public partial class Form1 : Form
    {
        #region Class {get set}
        public Form1()
        {
            InitializeComponent();
        }
        List<double> PreVal = new List<double>();
        List<double> PreAVal = new List<double>();
        //internal class V
        //{
        //    public double X { get; set; }
        //    public V(double x)
        //    {
        //        X = x;
        //    }
        //}
        ////internal class Pulsik
        ////{
        ////    public double X { get; set; }
        ////    public Pulsik(double x)
        ////    {
        ////        X = x;
        ////    }
        ////}
        //internal class PreAVal
        //{
        //    public double X { get; set; }

        //    public PreAVal(double x)
        //    {
        //        X = x;
        //    }
        //}
        //internal class PreVal
        //{
        //    public double X { get; set; }

        //    public PreVal(double x)
        //    {
        //        X = x;
        //    }
        //}
        //internal class U
        //{
        //    public double X { get; set; }
        //    public U(double x)
        //    {
        //        X = x;
        //    }
        //}
        //internal class dV
        //{
        //    public double X { get; set; }
        //    public dV(double x)
        //    {
        //        X = x;
        //    }
        //}

        //internal class dU
        //{
        //    public double X { get; set; }

        //    public dU(double x)
        //    {
        //        X = x;
        //    }
        //}
        #endregion
        #region Variables


        

        Complex[] comp;
        Complex[] comp2;
        double PI = 3.1415926535897932;
        double Zb = 10;
        double test;
        double t;
        double dE0 = 0;
        double A0 = 0;
        double Fi;
        double Zv;
        double F;
        const int N = 1000; //количество разбиений
        double[] InnerArray = new double[N];
        Complex[] Furie = new Complex[N];
     


      




        public double Pulse(double t)
        {
           
            double ret = 0.0;
            if ((t >= Data.start) && (t <= (Data.start + Data.fwfront)))
            {
                if (Data.fwfront > 0)
                {
                    ret = 0.5 * (Math.Sin(Math.PI * (t - Data.start) / Data.bwfront)) * Math.PI / Data.fwfront;
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
        
        }


        #endregion

        List<double> Pulsik = new List<double>();
        public void button1_Click(object sender, EventArgs e)
        {
            Complex j = new Complex(0, 1);

            PreAVal.Clear();

            Pulsik.Clear();

            chart1.Series[0].Points.Clear();
          

            chart3.Series[0].Points.Clear();
            chart4.Series[0].Points.Clear();
            chart5.Series[0].Points.Clear();

            #region list 
           
            List<double> V = new List<double>();
            List<double> dV = new List<double>();
            List<double> U = new List<double>();
            List<double> dU = new List<double>();
            //List<double> PreVal = new List<double>();
            //List<double> PreAVal = new List<double>();
            //List<double> V = new List<double>();
            //List<double> dV = new List<double>();
            //List<double> U = new List<double>();
            //List<double> dU = new List<double>();

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

            #endregion



            double start = 0;//время нач. импульса 
            double stop = 3; // конец импульса 
            double fwfront = 1; //  первый фронт 
            double bwfront = 1; // второй фронт
            for (int it = 0; it <= Data.Nt; it++)
            {

                double ret = (t >= start && t <= stop) ? 1.0 : 0.0;
                if ((t >= start) && (t <= (start + fwfront)))
                {
                    if (fwfront > 0)
                    {
                        ret *= 0.5 * (1 - Math.Cos(Math.PI * (t - start) / fwfront));
                    }
                }
                if ((t >= (stop - bwfront)) && (t <= stop))
                {
                    if (bwfront > 0)
                    {
                        ret *= 0.5 * (1 - Math.Cos(Math.PI * (stop - t) / bwfront));
                    }
                }
                //      chart2.Series[0].Points.AddXY(t, ret);
                PreVal.Add(ret);

            }



            for (int it = 0; it <= Data.Nt; it++)
            {



                t = it * Data.dt;

                
           







                
              if ((t >= Data.start))
              {
                dE0 = -2 * PI * Data.f0 * Data.Um0 * Math.Sin(2 * PI * Data.f0 * t) + Data.A0 * Pulse(t);
                }
                if ((t >= (Data.stop)))
             {
                   dE0 = 0;
                }




                dV[0] += ((2.0 / (Data.R0 * Data.C1)) * (dE0 - dV[0])) * Data.dt + ((2.0 / (Data.L * Data.C1)) * (V[1] - V[0] + U[0])) * Data.dt;
                dV[Data.Nc] += (2.0 / (Data.Rn * Data.C1)) * (0 - dV[Data.Nc]) * Data.dt + (2.0 / (Data.L * Data.C1)) * (V[Data.Nc - 1] - V[Data.Nc] - U[Data.Nc - 1]) * Data.dt;

                for (int i = 0; i < Data.Nc; i++)
                {
                    dU[i] += ((1.0 / (Data.L * Data.C2)) * (V[i] - V[i + 1] - U[i])) * Data.dt - 1.0 / (Data.R * Data.C2) * dU[i] * Data.dt;
                    if (i == 0) continue;
                    dV[i] += ((1.0 / (Data.L * Data.C1)) * (V[i - 1] - 2.0 * V[i] + V[i + 1] + U[i] - U[i - 1]) * Data.dt);
                }

                for (int i = 0; i < Data.Nc; i++)
                {
                    V[i] += dV[i] * Data.dt;
                    U[i] += dU[i] * Data.dt;
                  
                }

                V[Data.Nc] += dV[Data.Nc] * Data.dt;

                if ((it % 1) == 0)
                {
                    chart1.Series[0].Points.AddXY(t, V[0]);

                    chart4.Series[0].Points.AddXY(t, V[Data.Nc]);

                    //   chart1.Series[0].
                    //     PreAVal.Add(V[0]);
                    //  PreVal.Add(V[Data.Nc]);
                    

                }
            Pulsik.Add(V[Data.Nc]);
           // chart2.Series[0].Points.AddXY(t, (U[it])/PreVal[it]);
            PreAVal.Add(V[0]);

            }
            int L=-2,czet=0;

            label17.Text = String.Concat(string.Concat(Pulsik.Count.ToString()));
            double[] v0= new double[256];
            double[] v1 = new double[256];



       
            double otn,otn2;
            otn = PreAVal.Count / 256;
            otn = Math.Round(otn, 0);
            otn2 = Pulsik.Count / 256;
            otn2 = Math.Round(otn, 0);

            for (int it = 0; it < 256; it++)
            {
                if (Data.Time <= 5)
                {
                    v0[it] = PreAVal[it];
                    czet++;
                }
                else
                {
                    L = L + Convert.ToInt32(otn);
                    v0[it] = PreAVal[L];
                    czet++;
                }

            }
            L = -2;
            for (int it = 0; it < 256; it++)
            {
                if (Data.Time <= 5)
                {
                    v1[it] = Pulsik[it];
                    czet++;
                }
                else
                {
                    L = L + Convert.ToInt32(otn2);
                    v1[it] = Pulsik[L];
                    czet++;
                }

            }


            chart3.Series[0].Points.Clear();
            chart5.Series[0].Points.Clear();
            // chart3.ChartAreas[0].AxisX.Maximum = 3 * Data.fh;
            chart3.ChartAreas[0].AxisX.Minimum = 0.0;
            chart5.ChartAreas[0].AxisX.Minimum = 0.0;
       
            // double compCount = Math.Log(Data.Nt) / Math.Log(2);
            int mass = 256;//(int)(Math.Pow(2, Math.Truncate(compCount)));
            comp = new Complex[mass];
            comp2 = new Complex[mass];
            for (int k = 0; k < mass; k++)
            {
                comp[k] = v0[k];
                comp2[k] = v1[k];
            }
            Complex[] compSpec;
            compSpec = FFT.fft(comp);
            for (int p = 1; p < mass / 2; p++)
            {
                chart3.Series[0].Points.AddXY((p / (mass * Data.dt)) / otn, (compSpec[p].Magnitude / 50));
                //        chart1.Update();
            }
            //   chart3.ChartAreas[0].RecalculateAxesScale();


            Complex[] compSpec2;
            compSpec2 = FFT.fft(comp2);
            for (int pek = 1; pek < mass / 2; pek++)
            {

                chart5.Series[0].Points.AddXY((pek / (mass * Data.dt)) / otn, (compSpec2[pek].Magnitude / 50));
                //        chart1.Update();
            }
            chart5.ChartAreas[0].RecalculateAxesScale();
            chart5.Update();
     chart3.Update();
            //    InnerArray[i] = PreAVal[i];  //задаем форму сигнала


            label17.Text = String.Concat(string.Concat(PreAVal.Count.ToString()));

//
        //    chart1.Series[0].IsXValueIndexed = true;


            chart1.Series[0].IsXValueIndexed = true;

            chart1.Series[0].ChartType = SeriesChartType.Spline;
            //chart1.Series[0].Color = Color.Red;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "F2";



            // chart1.ChartAreas[0].AxisX.Maximum = 1; //Задаешь максимальные значения координат
            // chart1.ChartAreas[0].AxisY.Maximum = ;


            //  chart1.ChartAreas[0].CursorX.


         //   chart3.ChartAreas[0].AxisX.Maximum = 5;

         //   chart3.ChartAreas[0].AxisX.Minimum = 0;
            chart3.Series[0].ChartType = SeriesChartType.Line;
            chart3.ChartAreas[0].AxisX.LabelStyle.Format = "F2";
            chart3.ChartAreas[0].AxisX.Interval = 1; // и можешь интервалы настроить по своему усмотрению
                                                     //       chart3.ChartAreas[0].AxisY.Interval = 0.5;
            chart3.Series[0].Color = Color.Red;
          

            chart4.Series[0].IsXValueIndexed = true;
            chart4.Series[0].ChartType = SeriesChartType.Line;
            chart4.ChartAreas[0].AxisX.LabelStyle.Format = "F2";


           // chart5.ChartAreas[0].AxisX.Maximum = 5;
            chart5.Series[0].ChartType = SeriesChartType.Line;
            chart5.ChartAreas[0].AxisX.LabelStyle.Format = "F2";
            chart5.ChartAreas[0].AxisX.Interval = 1;
            chart5.Series[0].Color = Color.Red;

            // Zv = 2 / (Data.w0 * Data.C1) * Math.Sqrt(Data.C2);//initialize


            Complex Z1 = 2 / (j * Data.w0 * Data.C1);
            Complex Z2 = 1 / (j * Data.w0 * Data.C2);
            Complex Z3 = j * Data.w0 * Data.L;
            Complex Zc = Complex.Sqrt((Z1 * Z1 * (Z2 + Z3)) / (2 * Z1 + Z2 + Z3));



            label10.Text = String.Concat(string.Concat(Math.Round(Zc.Real, 6).ToString()));



            }
        private void button3_Click(object sender, EventArgs e)
        {
            Data.start = Convert.ToDouble(textBox12.Text);
            Data.stop = Convert.ToDouble(textBox13.Text);
            Data.fwfront = Convert.ToDouble(textBox14.Text);
            Data.bwfront = Convert.ToDouble(textBox15.Text);
            Data.A0 = Convert.ToDouble(textBox16.Text);
            Data.Time = Convert.ToDouble(textBox1.Text);
            Data.Nc = Convert.ToInt32(textBox2.Text);
            Data.Nd = Convert.ToInt32(textBox3.Text);
            Data.fn = Convert.ToDouble(textBox4.Text);
            Data.fv = Convert.ToDouble(textBox5.Text);
            Data.f0 = Convert.ToDouble(textBox7.Text);
            Data.wv = 2 * PI * Data.fv;
            Data.wn = 2 * PI * Data.fn;
            Data.w0 = 2 * PI * Data.f0;
            //Convert.ToDouble(textBox6.Text);
            
            Data.R = Convert.ToDouble(textBox8.Text);
            Data.R0 = Convert.ToDouble(textBox9.Text);
            Data.Rn = Convert.ToDouble(textBox10.Text);
            Data.Um0 = Convert.ToDouble(textBox11.Text);
            Data.dt = 1 / (Data.fv * Data.Nd);// Convert.ToDouble(textBox17.Text); 
            Data.Nt = Data.Time / Data.dt;
            label5.Text = String.Concat(string.Concat(Data.Nt.ToString()));
            Data.C1 = Convert.ToDouble(textBox6.Text);                                
            Data.C2 = (Data.C1 / 2) * (((Data.wv / Data.wn) * (Data.wv / Data.wn)) - 1);
            Data.L = 1 / (Data.wn * Data.wn * Data.C2);
            label24.Text = String.Concat(string.Concat(Data.C2.ToString()));
        }


        #region Stuff
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
      
      
        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }
       
        private void chart2_Click_1(object sender, EventArgs e)
        {

        }
      

        #endregion

        private void button4_Click(object sender, EventArgs e)
        {
           FiZvotF f2 = new FiZvotF();
            f2.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            UotI f3 = new UotI();
            f3.Show();
        }
       
       
       

        private void button7_Click(object sender, EventArgs e)
        {
            chart4.SaveImage("F:\\novapraim\\data.png", ChartImageFormat.Png);
          //  F:\novapraim
        }

        #region cursor



        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
                 
            Point mousePoint = new Point(e.X, e.Y);
            chart1.ChartAreas[0].CursorX.SetCursorPixelPosition(mousePoint, true);
            chart1.ChartAreas[0].CursorY.SetCursorPixelPosition(mousePoint, true);
            //chart1.  
            try
            {
              //  double y2value = chart1.ChartAreas[0].CursorX.;
                double yvalue = chart1.ChartAreas[0].AxisY2.PixelPositionToValue(e.Y);
                double xvalue = chart1.ChartAreas[0].AxisX2.PixelPositionToValue(e.X);
                label4.Text = String.Concat(string.Concat(Math.Round(xvalue/100, 4).ToString(), " ; "), Math.Round(yvalue, 4).ToString());
            }
            catch
            {

            }
            finally
            {

            }

        }

        private void chart4_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePoint = new Point(e.X, e.Y);
            chart4.ChartAreas[0].CursorX.SetCursorPixelPosition(mousePoint, true);
            chart4.ChartAreas[0].CursorY.SetCursorPixelPosition(mousePoint, true);
            //chart1.  
            try
            {
                double yvalue = chart4.ChartAreas[0].AxisY2.PixelPositionToValue(e.Y);
                double xvalue = chart4.ChartAreas[0].AxisX2.PixelPositionToValue(e.X);
                label4.Text = String.Concat(string.Concat(Math.Round(xvalue/100, 4).ToString(), " ; "), Math.Round(yvalue, 4).ToString());
            }
            catch
            {

            }
            finally
            {

            }
        }



        #endregion

        

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }

    static class Data
    {
  
        public static double start { get; set; }
        public static double stop { get; set; }
        public static double fwfront { get; set; }
        public static double bwfront { get; set; }
        public static double A0 { get; set; }

        public static int Nd { get; set; }
        public static double Nt { get; set; }  // 
        public static double fv { get; set; }
        public static double fn { get; set; }
        public static double f0 { get; set; }
        public static double C2 { get; set; }
        public static double C1 { get; set; }
        //  public static double Zb { get; set; }
        public static double dt { get; set; }
        public static double L { get; set; }
        public static double wn { get; set; }
        public static double w0 { get; set; }
        public static double wv { get; set; }
        public static double t { get; set; }
        public static double Time { get; set; }
        public static double R { get; set; }
        public static double R0 { get; set; }
        public static double Rn { get; set; }
        public static double Um0 { get; set; }
        //   public static double dE0 { get; set; }
        public static int Nc { get; set; }
        //   public static double Fa { get; set; }
        //  public static double Zv { get; set; }
        //  public static double F { get; set; }




    }
    public class FFT
    {
       
             private static Complex w(int k, int N)
        {
            if (k % N == 0) return 1;
            double arg = -2 * Math.PI * k / N;
            return new Complex(Math.Cos(arg), Math.Sin(arg));
        }
             public static Complex[] fft(Complex[] x)
        {
            Complex[] X;
            int N = x.Length;
            if (N == 2)
            {
                X = new Complex[2];
                X[0] = x[0] + x[1];
                X[1] = x[0] - x[1];
            }
            else
            {
                Complex[] x_even = new Complex[N / 2];
                Complex[] x_odd = new Complex[N / 2];
                for (int i = 0; i < N / 2; i++)
                {
                    x_even[i] = x[2 * i];
                    x_odd[i] = x[2 * i + 1];
                }
                Complex[] X_even = fft(x_even);
                Complex[] X_odd = fft(x_odd);
                X = new Complex[N];
                for (int i = 0; i < N / 2; i++)
                {
                    X[i] = X_even[i] + w(i, N) * X_odd[i];
                    X[i + N / 2] = X_even[i] - w(i, N) * X_odd[i];
                }
            }
            return X;
        }
             public static Complex[] nfft(Complex[] X)
        {
            int N = X.Length;
            Complex[] X_n = new Complex[N];
            for (int i = 0; i < N / 2; i++)
            {
                X_n[i] = X[N / 2 + i];
                X_n[N / 2 + i] = X[i];
            }
            return X_n;
        }
    }
}
