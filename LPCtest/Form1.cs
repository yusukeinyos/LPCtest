using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LPCtest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //----------------------------------------------------
        double[] auto_correlation(double[] x)
        {
            int N = x.Length;
            double[] r = new double[N];
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N - i; j++)
                    r[i] += x[j] * x[j + i];
            return r;
        }
        //----------------------------------------------------
        double[] levinson_durbin(double[] r, int order)
        {
            double[] a = new double[order + 1];
            double e;
            double[] lambda = new double[order + 1];
            a[0] = 1.0;
            a[1] = -r[1] / r[0];
            e = r[0] + r[1] * a[1];
            lambda[0] = -r[1] / r[0];
            for (int i = 1; i < order + 1; i++)
            {
                double sum = 0;
                for (int j = 0; j < i; j++)
                {
                    sum += a[j] * r[i + 1 - j];
                }
                lambda[i] = -sum / e;
                for (int j = 1; j < i + 1; j++)
                {
                    a[j] = a[j] + lambda[i] * a[i + 1 - j];
                }
                a[i + 1] = lambda[i] * a[0];
            }
            return a;
        }
        //----------------------------------------------------
    }
}
