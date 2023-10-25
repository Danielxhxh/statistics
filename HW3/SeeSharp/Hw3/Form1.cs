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

        public Form1()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;

            textBox1.Text = "10";
            textBox2.Text = "10";
            textBox3.Text = "0.5";
            textBox4.Text = "3";

        }

        public static Random random = new Random();

        private void fillChart()
        {
            int numberOfSystems = int.Parse(textBox1.Text);
            int numberOfAttacks = int.Parse(textBox2.Text);
            int attackNumber = int.Parse(textBox4.Text);
            float probability;

            //int indexNthAttack = 3;

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

            double[] cumulatedFrequency;
            double[] relativeFrequency;
            double[] normalizedRatio;

            int[] lastValues = new int[numberOfSystems];
            int[] nthAttacks = new int[numberOfSystems];

            chart1.Series.Clear();
            chart2.Series.Clear();
            chart3.Series.Clear();
            chart4.Series.Clear();
            chart5.Series.Clear();


            

            (int[], double[], double[], double[]) result;



            for (int i = 0; i < numberOfSystems; i++)
            {

                result = generateCoordinateVector(numberOfAttacks, probability, minProbability, maxProbability);

                y = result.Item1;
                cumulatedFrequency = result.Item2;
                relativeFrequency = result.Item3;
                normalizedRatio = result.Item4;


                lastValues[i] = y[numberOfAttacks - 1];
                nthAttacks[i] = y[attackNumber];

                var series = new Series($"Systems {i+1}");
                series.ChartType = SeriesChartType.Line;
                chart1.ChartAreas[0].AxisX.Minimum = 0;

                series.Points.DataBindXY(x, y);
                chart1.Series.Add(series);

                var series2 = new Series($"Systems {i + 1}");
                series2.ChartType = SeriesChartType.Line;
                chart2.ChartAreas[0].AxisX.Minimum = 0;

                series2.Points.DataBindXY(x, cumulatedFrequency);
                chart2.Series.Add(series2);

                var series3 = new Series($"Systems {i + 1}");
                series3.ChartType = SeriesChartType.Line;
                chart3.ChartAreas[0].AxisX.Minimum = 0;

                series3.Points.DataBindXY(x, relativeFrequency);
                chart3.Series.Add(series3);

                var series4 = new Series($"Systems {i + 1}");
                series4.ChartType = SeriesChartType.Line;
                chart4.ChartAreas[0].AxisX.Minimum = 0;

                series4.Points.DataBindXY(x, normalizedRatio);
                chart4.Series.Add(series4);
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

            int maxNth = nthAttacks.Max();
            int minNth = nthAttacks.Min();
            int axesNthLength = maxNth - minNth + 1;
            int[] yNth = new int[axesNthLength];
            int[] xNth = new int[axesNthLength];

            for (int i = 0; i < axesNthLength; i++)
            {
                yNth[i] = maxNth - i;
            }

            for (int i = 0; i < axesNthLength; i++)
            {
                for (int j = 0; j < nthAttacks.Length; j++)
                {
                    if (yNth[i] == nthAttacks[j])
                    {
                        xNth[i]++;
                    }
                }
            }

            chart5.Series.Clear();
            chart5.Series.Add("Last Attack");
            chart5.Series.Add("N-th Attack");
            chart5.Series["Last Attack"].ChartType = SeriesChartType.Bar;
            chart5.Series["N-th Attack"].ChartType = SeriesChartType.Bar;

            // Bind data to the chart
            for (int i = 0; i < yLast.Length; i++)
            {
                chart5.Series["Last Attack"].Points.AddXY(yLast[i], xLast[i]);
            }
            for (int i = 0; i < yNth.Length; i++)
            {
                chart5.Series["N-th Attack"].Points.AddXY(yNth[i], xNth[i]);
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

        public static (int[], double[], double[], double[]) generateCoordinateVector(int size, float probability, float minProbability, float maxProbability)
        {
            int[] y = new int[size];
            double[] cumulatedFrequency = new double[size];
            double[] relativeFrequency = new double[size];
            double[] normalizedRatio = new double[size];
            y[0] = 0;
            int sum = 0;
            double sumFrequency = 0;
            float value = 0;


            for (int i = 1; i < size; i++)
            {
                value = GenerateRandomDouble(minProbability, maxProbability);

                sum += generateY(value, probability);
                sumFrequency += generateFrequencyY(value, probability);
                
                y[i] = sum;
                cumulatedFrequency[i] = sumFrequency;
                relativeFrequency[i] = sumFrequency/(i+1);
                normalizedRatio[i] = sumFrequency / Math.Sqrt(i + 1);

                Console.WriteLine(relativeFrequency[i]);
            }

            return (y, cumulatedFrequency, relativeFrequency, normalizedRatio);
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
