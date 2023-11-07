using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace prova
{
    public partial class Form1 : Form
    {

        private Chart[] charts;
        private Point[] chartLocations;
        private Point[] mouseDownLocations;
        private bool[] isDraggingList;
        private bool[] isResizingList;
        private Size[] resizeStartSize;

        public static Random random = new Random();

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Dock = DockStyle.Fill;

            textBox1.Text = "50";
            textBox2.Text = "1";
            textBox3.Text = "50";
            textBox4.Text = "20";
            textBox5.Text = "0";


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
            int numberOfServers = int.Parse(textBox1.Text);
            int periodTime = int.Parse(textBox2.Text);
            int numberIntervals = int.Parse(textBox3.Text);
            int lambda = int.Parse(textBox4.Text);
            float chosenInterval;

            if (float.TryParse(textBox5.Text, out chosenInterval))
            {
                chosenInterval = (float)Math.Round(chosenInterval, 2); 
                Console.WriteLine($"Parsed chosenInterval: {chosenInterval}");
            }
            else
            {
                Console.WriteLine("Invalid input for chosenInterval.");
            }

            float probability = (float)lambda * (periodTime / (float)numberIntervals);


            float minValue = 0;
            float maxValue = 1;


            float[] x = generateX(numberIntervals, periodTime);
            int[] y;
            
            int[] lastValues = new int[numberOfServers];
            int[] nthAttacks = new int[numberOfServers];


            int min;
            int max;


            chart1.Series.Clear();
            chart2.Series.Clear();


            int[] result;
            int index = Array.IndexOf(x, chosenInterval)-1;

            for (int i = 0; i < numberOfServers; i++)
            {
                result = generateCoordinateVector(numberIntervals, probability, minValue, maxValue);

                y = result;

                lastValues[i] = y[numberIntervals - 1];

             
                if (index > -1)
                {
                    nthAttacks[i] = y[index];
                }
                else
                {
                    Console.WriteLine($"{chosenInterval} is not found in the array.");
                    nthAttacks[i] = y[0];
                }
                

                var series = new Series($"Systems {i + 1}");
                series.ChartType = SeriesChartType.Line;
                chart1.ChartAreas[0].AxisX.Minimum = 0;

                series.Points.DataBindXY(x, y);
                chart1.Series.Add(series);
                CustomAxisInterval(chart1.ChartAreas[0].AxisX, periodTime, numberIntervals);

            }
            

            int maxLast = lastValues.Max();
            int minLast = lastValues.Min();

            int maxNth = nthAttacks.Max();
            int minNth = nthAttacks.Min();

            if (maxLast >= maxNth) max = maxLast;
            else max = maxNth;

            if (minLast < minNth) min = minLast;
            else min = minNth;

            int axesLength = max - min + 1;

            int[] yHistogram = new int[axesLength];
            int[] xLast = new int[axesLength];
            int[] xNth = new int[axesLength];

            for (int i = 0; i < axesLength; i++)
            {
                yHistogram[i] = maxLast - i;
            }

            for (int i = 0; i < axesLength; i++)
            {
                for (int j = 0; j < lastValues.Length; j++)
                {
                    if (yHistogram[i] == lastValues[j])
                    {
                        xLast[i]++;
                    }
                    if (yHistogram[i] == nthAttacks[j])
                    {
                        xNth[i]++;
                    }
                }
            }

            chart2.Series.Clear();
            chart2.Series.Add("Last Attack");
            chart2.Series.Add("Interval Attack");
            chart2.Series["Last Attack"].ChartType = SeriesChartType.Bar;
            chart2.Series["Interval Attack"].ChartType = SeriesChartType.Bar;

            for (int i = 0; i < yHistogram.Length; i++)
            {
                chart2.Series["Last Attack"].Points.AddXY(yHistogram[i], xLast[i]);
                chart2.Series["Interval Attack"].Points.AddXY(yHistogram[i], xNth[i]);
            }

        }

        public static float[] generateX(int numberIntervals, int periodTime)
        {
            float[] x = new float[numberIntervals + 1];
            for (int i = 0; i <= numberIntervals; i++)
            {
                x[i] = (periodTime/ (float)numberIntervals) * i;
                
            }

            return x;
        }

        public static float GenerateRandomDouble(float minValue, float maxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentException("minValue must be less than or equal to maxValue");

            float randomValue = (float)random.NextDouble();
            float range = maxValue - minValue;
            float scaledValue = randomValue * range;
            float result = scaledValue + minValue;

            return result;
        }

        public static int[] generateCoordinateVector(int numberIntervals, float probability, float minValue, float maxValue)
        {
            int[] y = new int[numberIntervals + 1];
            y[0] = 0;
            int sum = 0;
            float value;


            for (int i = 1; i <= numberIntervals; i++)
            {
                value = GenerateRandomDouble(minValue, maxValue);
                sum += generateY(value, probability);
                y[i] = sum;
            }

            return y;
        }

        public static int generateY(float attack, float probability)
        {
            if (attack <= probability) return +1;
            else return 0;
        }

        void CustomAxisInterval(Axis axis, int periodTime,int numberIntervals)
        {
            axis.Minimum = 0;
            axis.Maximum = periodTime;
            axis.Interval = periodTime/ (float)numberIntervals;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            fillChart();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            fillChart();
        }
    }
}

