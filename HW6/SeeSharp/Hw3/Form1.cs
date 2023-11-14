using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Globalization;

namespace Hw3
{
    public partial class Form1 : Form
    {
        private Chart[] charts;
        private Point[] chartLocations;
        private Point[] mouseDownLocations;
        private bool[] isDraggingList;
        private bool[] isResizingList;
        private Size[] resizeStartSize;
        private static int securityScore;
        private static int[] P = { -20, -30, -40, -50, -60, -70, -80, -90, -100 };

        public static Random random = new Random();

        public Form1()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;

            textBox1.Text = "10";
            textBox2.Text = "10";
            textBox3.Text = "0.5";
            textBox4.Text = "6";


            int numberOfCharts = 2;
            charts = new Chart[numberOfCharts];
            chartLocations = new Point[numberOfCharts];
            mouseDownLocations = new Point[numberOfCharts];
            isDraggingList = new bool[numberOfCharts];
            isResizingList = new bool[numberOfCharts];
            resizeStartSize = new Size[numberOfCharts];

            for (int i = 0; i < numberOfCharts; i++)
            {
                charts[i] = Controls.Find($"chart{i + 1}", true)[0] as Chart;
                chartLocations[i] = new Point(0, 0);
                mouseDownLocations[i] = Point.Empty;
                isDraggingList[i] = false;

                charts[i].MouseDown += Chart_MouseDown;
                charts[i].MouseMove += Chart_MouseMove;
                charts[i].MouseUp += Chart_MouseUp;
            }
        }

        private void Chart_MouseDown(object sender, MouseEventArgs e)
        {
            var chart = (Chart)sender;
            int chartIndex = Array.IndexOf(charts, chart);

            if (e.Button == MouseButtons.Left)
            {
                if (e.X >= chart.Width - 10 && e.Y >= chart.Height - 10)
                {
                    isResizingList[chartIndex] = true;
                    resizeStartSize[chartIndex] = chart.Size;
                }
                else
                {
                    isDraggingList[chartIndex] = true;
                    mouseDownLocations[chartIndex] = e.Location;
                }
            }
        }

        private void Chart_MouseMove(object sender, MouseEventArgs e)
        {
            var chart = (Chart)sender;
            int chartIndex = Array.IndexOf(charts, chart);

            if (isDraggingList[chartIndex])
            {
                int deltaX = e.X - mouseDownLocations[chartIndex].X;
                int deltaY = e.Y - mouseDownLocations[chartIndex].Y;

                chartLocations[chartIndex].X += deltaX;
                chartLocations[chartIndex].Y += deltaY;

                if (chartLocations[chartIndex].X < 0) chartLocations[chartIndex].X = 0;
                if (chartLocations[chartIndex].Y < 0) chartLocations[chartIndex].Y = 0;
                if (chartLocations[chartIndex].X + chart.Width > pictureBox1.Width) chartLocations[chartIndex].X = pictureBox1.Width - chart.Width;
                if (chartLocations[chartIndex].Y + chart.Height > pictureBox1.Height) chartLocations[chartIndex].Y = pictureBox1.Height - chart.Height;

                chart.Location = chartLocations[chartIndex];
            }
            else if (isResizingList[chartIndex])
            {
                int deltaX = e.X - resizeStartSize[chartIndex].Width;
                int deltaY = e.Y - resizeStartSize[chartIndex].Height;

                int newWidth = resizeStartSize[chartIndex].Width + deltaX;
                int newHeight = resizeStartSize[chartIndex].Height + deltaY;

                if (newWidth < 100)
                    newWidth = 100;
                if (newHeight < 100)
                    newHeight = 100;

                chart.Size = new Size(newWidth, newHeight);
            }
            else if (e.X >= chart.Width - 10 && e.Y >= chart.Height - 10)
            {
                chart.BackColor = Color.LightGray;
                chart.Cursor = Cursors.SizeNWSE;
            }
            else
            {
                chart.BackColor = Color.White;
                chart.Cursor = Cursors.Default;
            }
        }

        private void Chart_MouseUp(object sender, MouseEventArgs e)
        {
            var chart = (Chart)sender;
            int chartIndex = Array.IndexOf(charts, chart);

            if (e.Button == MouseButtons.Left)
            {
                isDraggingList[chartIndex] = false;
                isResizingList[chartIndex] = false;
            }
        }

        private void fillChart()
        {
            int numberOfSystems = int.Parse(textBox1.Text);
            int numberOfAttacks = int.Parse(textBox2.Text);
            securityScore = int.Parse(textBox4.Text);
            float probability;

            if (float.TryParse(textBox3.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out probability))
            {
                Console.WriteLine("Conversione riuscita. Valore float: " + probability);
            }
            else
            {
                Console.WriteLine("Conversione non riuscita. L'input non è un valore float valido.");
            }

            float minProbability = 0;
            float maxProbability = 1;



            int[] x = generateX(numberOfAttacks);
            int[] y;

            int[] lastValues = new int[numberOfSystems];

            chart1.Series.Clear();
            chart2.Series.Clear();

            int[] result;



            for (int i = 0; i < numberOfSystems; i++)
            {

                result = generateCoordinateVector(numberOfAttacks, probability, minProbability, maxProbability);

                y = result;

                lastValues[i] = y[numberOfAttacks - 1];

                var series = new Series($"Systems {i + 1}");
                series.ChartType = SeriesChartType.Line;
                chart1.ChartAreas[0].AxisX.Minimum = 0;

                series.Points.DataBindXY(x, y);
                chart1.Series.Add(series);
            }


            int maxLast = lastValues.Max();
            int minLast = lastValues.Min();
            int axesLastLength = maxLast - minLast + 1;
            int[] yLast = new int[axesLastLength];
            int[] xLast = new int[axesLastLength];

            for (int i = 0; i < axesLastLength; i++)
            {
                yLast[i] = maxLast - i;
            }

            for (int i = 0; i < axesLastLength; i++)
            {
                for (int j = 0; j < lastValues.Length; j++)
                {
                    if (yLast[i] == lastValues[j])
                    {
                        xLast[i]++;
                    }
                }
            }

            chart2.Series.Clear();
            chart2.Series.Add("Last Attack");
            chart2.Series["Last Attack"].ChartType = SeriesChartType.Bar;

            // Bind data to the chart
            for (int i = 0; i < yLast.Length; i++)
            {
                chart2.Series["Last Attack"].Points.AddXY(yLast[i], xLast[i]);
            }


        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            fillChart();

        }

        public static int[] generateX(int size)
        {
            int[] x = new int[size];
            for (int i = 0; i < size; i++)
            {
                x[i] = i;
            }

            return x;
        }

        public static float GenerateRandomDouble(float minProbability, float maxProbability)
        {
            if (minProbability > maxProbability)
                throw new ArgumentException("minProbability must be less than or equal to maxProbability");

            float randomValue = (float)random.NextDouble(); // Generates a random double between 0 and 1
            float range = maxProbability - minProbability;
            float scaledValue = randomValue * range;
            float result = scaledValue + minProbability;

            return result;
        }

        public static int[] generateCoordinateVector(int size, float probability, float minProbability, float maxProbability)
        {
            int[] y = new int[size];
            y[0] = 0;
            int sum = 0;
            float value = 0;



            for (int i = 1; i < size; i++)
            {
                value = GenerateRandomDouble(minProbability, maxProbability);
                sum += generateY(value, probability);

                y[i] = sum;

            }

            return y;
        }

        public static int minSecurityScore(int[] score, int indexOfP,int securityScore) {
            bool insecure = false;
            bool secure = false;

            for (int i = 0; i < score.Length; i++) {
                if (score[i] == P[indexOfP])
                {
                    insecure = true;
                }
                else if(score[i] == securityScore)
                {
                    secure = true;
                }
            }

            for (int i = 0; i < score.Length; i++) {
                if (insecure) {
                    return P[indexOfP];
                }else if (secure)
                {
                    return securityScore;
                }
            }

            return score[score.Length];
        }


        //conta tutti i valori che ha raggiunto
        public static int[] valueCounter(int lastValue)
        {
            int[] values = new int[securityScore - lastValue];
            for(int i=0; i<(securityScore - lastValue); i++)
            {

            }
        }





        public static int generateY(float attack, float probability)
        {
            if (attack <= probability) return -1;
            else return +1;
        }

        public static int generateFrequencyY(float attack, float probability)
        {
            if (attack <= probability) return 0;
            else return +1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fillChart();

        }

    }
}
