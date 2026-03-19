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
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Метод для расчета первой функции
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static double Calc(double x, double y, double z)
        {
            if (x == 0)
            {
                throw new ArgumentException("x не может быть равен 0! (деление на ноль)");
            }

            if (y == x)
            {
                throw new ArgumentException("y не может быть равен x! (деление на ноль)");
            }

            double term1 = Math.Abs(Math.Pow(x, y) - Math.Pow(y / x, 1.0 / 3.0));
            double denominator = 1 + Math.Pow(y - x, 2);
            double exponent = (Math.Cos(y) - z / (y - x)) / denominator;
            double term2 = Math.Pow(Math.Abs(y - x), exponent);

            return term1 + term2;
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(X.Text) ||
                    string.IsNullOrWhiteSpace(Y.Text) ||
                    string.IsNullOrWhiteSpace(Z.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля!");
                    return;
                }

                double x = double.Parse(X.Text.Replace(".", ","));
                double y = double.Parse(Y.Text.Replace(".", ","));
                double z = double.Parse(Z.Text.Replace(".", ","));

                double result = Calc(x, y, z);
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
            Y.Clear();
            Z.Clear();
            Result.Clear();
            X.Focus();
        }
    }
}


            

       
            
        
    
