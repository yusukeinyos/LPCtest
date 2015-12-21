﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InoueLab;

namespace LPCtest
{
    public partial class Form1 : Form
    {
        double[] x;
        double[] x_hat;
        int order;

        public Form1()
        {
            InitializeComponent();
            init();
        }
        //----------------------------------------------------
        void init()
        {
            order = 10;
        }
        //----------------------------------------------------
        double[] LinearPrediction(double[] x, double[] lpc, int order)
        {
            int N = x.Length;
            double[] prediction = new double[N];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < order; j++)
                {
                    if (!(i - j < 0))
                        prediction[i] -= lpc[j] * x[i - j];
                }
            }
            return prediction;
        }
        //----------------------------------------------------
        double[] auto_correlation(double[] x)
        {
            int N = x.Length;
            double[] r = new double[order + 1];
            for (int i = 0; i < order + 1; i++)
                for (int j = 0; j < N; j++)
                    if (i + j < N)
                        r[i] += x[j] * x[j + i];
            return r;
        }
        //----------------------------------------------------
        double[] levinson_durbin(double[] r)
        {
            double[] a = new double[order + 1];
            double e;
            double[] lambda = new double[order + 1];
            a[0] = 1.0;
            a[1] = -r[1] / r[0];
            e = r[0] + r[1] * a[1];
            lambda[0] = -r[1] / r[0];
            for (int i = 1; i < order ; i++)
            {
                double sum = 0;
                for (int j = 0; j < i + 1; j++)
                {
                    sum += a[j] * r[i + 1 - j];
                }
                lambda[i] = -sum / e;
                for (int j = 1; j < i + 1; j++)
                {
                    a[j] = a[j] + lambda[i] * a[i + 1 - j];
                }
                a[i + 1] = lambda[i] * a[0];
                e = (1 - lambda[i] * lambda[i]) * e;
            }
            return a;
        }
        //----------------------------------------------------
        double[][] readData(string filename)
        {
            double[][] d;
            using (StreamReader stream = new StreamReader(filename))
            {
                string all = stream.ReadToEnd();
                var lines = all.Split(new char[] { '\n' }).Where(l => l != "").ToArray();
                d = new double[lines.Length][];
                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i].Split(new char[] { ',' }).Where(a => a != "").ToArray();
                    d[i] = new double[line.Length];
                    for (int j = 0; j < line.Length; j++)
                    {
                        d[i][j] = double.Parse(line[j]);
                    }
                }
            }
            return d;
        }
        //----------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            double[][] data = readData("data.csv");
            x = New.Array(data.Length, d => data[d][1]);
            x_hat = LinearPrediction(x, levinson_durbin(auto_correlation(x)), order);
        }
    }
}
