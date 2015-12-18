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

    }
}
