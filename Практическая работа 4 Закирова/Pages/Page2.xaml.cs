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

namespace Практическая_работа_4_Закирова.Pages
{
    /// <summary>
    /// Логика взаимодействия для Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
        }

        private double CalculateFx(double x)
        {
            if (ShX.IsChecked == true)
                return Math.Sinh(x); // гиперболический синус
            else if (X2.IsChecked == true)
                return x * x;
            else 
                return Math.Exp(x);
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(X.Text) ||
                    string.IsNullOrWhiteSpace(P.Text))
                {
                    MessageBox.Show("Заполните все поля!");
                    return;
                }

                double x = double.Parse(X.Text.Replace(".", ","));
                double p = double.Parse(P.Text.Replace(".", ","));
                double fx = CalculateFx(x);
                double absP = Math.Abs(p);
                double result;

                if (x > absP)
                {
                    // l = 2f(x)³ + 3p²
                    result = 2 * Math.Pow(fx, 3) + 3 * Math.Pow(p, 2);
                }
                else if (x > 3 && x < absP)
                {
                    // l = |f(x) - p|
                    result = Math.Abs(fx - p);
                }
                else if (x == absP)
                {
                    // l = (f(x) - p)²
                    result = Math.Pow(fx - p, 2);
                }
                else
                {
                    MessageBox.Show("Для введенных значений не определена ветвь функции!\n" +
                        $"Условия: x > |p| или 3 < x < |p| или x = |p|\n" +
                        $"Ваши значения: x={x}, |p|={absP}");
                    return;
                }

                Result.Text = result.ToString("F6");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка вычисления: {ex.Message}");
            }
        
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            X.Clear();
            P.Clear();
            Result.Clear();
            Exp.IsChecked = true;
            X.Focus();
        }
    }
}
