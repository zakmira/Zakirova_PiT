using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms.DataVisualization.Charting;
namespace Практическая_работа_4_Закирова.Pages
{
    /// <summary>
    /// Логика взаимодействия для Page3.xaml
    /// </summary>
    public partial class Page3 : Page
    {
        public Page3()
        {
            InitializeComponent();

            Chart.ChartAreas.Add(new ChartArea("Main"));
            var series = new Series("График функции")
            {
                ChartType = SeriesChartType.Line,
                IsValueShownAsLabel = false,
                Color = System.Drawing.Color.Blue,
                BorderWidth = 2
            };

            Chart.Series.Add(series);

        }

        private void UpdateChart(List<double> xValues, List<double> yValues)
        {
            Series series = Chart.Series.FirstOrDefault();

            if (series != null)
            {
                series.Points.Clear();

                // точки графика
                for (int i = 0; i < xValues.Count; i++)
                {
                    series.Points.AddXY(xValues[i], yValues[i]);
                }

                Chart.ChartAreas[0].AxisX.Title = "x";
                Chart.ChartAreas[0].AxisY.Title = "y";
                
            }
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> errors = new List<string>();

                if (string.IsNullOrWhiteSpace(B.Text) ||
                    string.IsNullOrWhiteSpace(X0.Text) ||
                    string.IsNullOrWhiteSpace(Xk.Text) ||
                    string.IsNullOrWhiteSpace(Dx.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля!");
                    return;
                }

                if (errors.Count > 0)
                {
                    MessageBox.Show("Обнаружены ошибки ввода:\n\n" + string.Join("\n", errors));
                    return;
                }

                double b = double.Parse(B.Text.Replace(".", ","));
                double x0 = double.Parse(X0.Text.Replace(".", ","));
                double xk = double.Parse(Xk.Text.Replace(".", ","));
                double dx = double.Parse(Dx.Text.Replace(".", ","));

                if (dx <= 0)
                    errors.Add("Шаг dx должен быть положительным");
                if (x0 >= xk)
                    errors.Add("x₀ должно быть меньше xₖ");

                if (errors.Count > 0)
                {
                    MessageBox.Show("Ошибки в параметрах:\n\n" + string.Join("\n", errors));
                    return;
                }

                List<double> xValues = new List<double>();
                List<double> yValues = new List<double>();

                string results = "x\t\ty\n-------------------\n";

                for (double x = x0; x <= xk + dx / 2; x += dx)
                {
                    if (x + Math.Exp(0.82) < 0)
                    {
                        MessageBox.Show($"x = {x:F2} вне области определения!");
                        continue;
                    }

                    double y = 0.0025 * b * Math.Pow(x, 3) + Math.Sqrt(x + Math.Exp(0.82));

                    results += $"{x,6:F2}\t{y,10:F6}\n";
                    xValues.Add(x);
                    yValues.Add(y);
                }

                Result.Text = results;

                UpdateChart(xValues, yValues);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            B.Text = "2.3";
            X0.Text = "-1";
            Xk.Text = "4";
            Dx.Text = "0.5";
            Result.Clear();

            Series series = Chart.Series.FirstOrDefault();
            if (series != null)
            {
                series.Points.Clear();
            }
        }
    }
}
