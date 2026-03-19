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

        /// <summary>
        /// Вычисляет вторую функцию (кусочная):
        /// l = 2f(x)³ + 3p², при x > |p|
        /// l = |f(x) - p|, при 3 < x < |p|
        /// l = (f(x) - p)², при x = |p|
        /// </summary>
        /// <param name="x">Параметр x</param>
        /// <param name="p">Параметр p</param>
        /// <param name="fx">Значение функции f(x)</param>
        /// <returns>Результат вычисления функции l</returns>
        /// <exception cref="ArgumentException">
        /// Выбрасывается, если ни одно условие не выполняется
        /// </exception>
        public static double CalcFormula2(double x, double p, double fx)
        {
            double absP = Math.Abs(p);

            if (x > absP)
            {
                // Первая ветвь: l = 2f(x)³ + 3p²
                return 2 * Math.Pow(fx, 3) + 3 * Math.Pow(p, 2);
            }
            else if (x > 3 && x < absP)
            {
                // Вторая ветвь: l = |f(x) - p|
                return Math.Abs(fx - p);
            }
            else if (Math.Abs(x - absP) < 1e-10) // Сравнение с погрешностью
            {
                // Третья ветвь: l = (f(x) - p)²
                return Math.Pow(fx - p, 2);
            }
            else
            {
                throw new ArgumentException(
                    $"Для x={x} и |p|={absP} не определена ветвь функции");
            }
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
                double fx;
                if (ShX.IsChecked == true)
                    fx = Math.Sinh(x);
                else if (X2.IsChecked == true)
                    fx = Math.Pow(x, 2);
                else 
                    fx = Math.Exp(x);

                double result = CalcFormula2(x, p, fx);

                Result.Text = result.ToString("F6");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Ошибка вычисления: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
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
