using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace kinoshechka.Pages
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Page
    {
        public Auth()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Метод авторизации выведен в отдельный метод
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Auth1(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    return false;
                }

                string passwordHash = HashPassword(password);

                var user = Core.Context.Users
                    .FirstOrDefault(u => u.Username == username && u.PasswordHash == passwordHash);

                if (user != null)
                {
                    Core.CurrentUserId = user.UserID;
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// Кнопка авторизации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Auth_Click(object sender, RoutedEventArgs e)
        {
            bool isAuth = Auth1(UserNameBox.Text.Trim(), PasswordBox.Password);

            if (isAuth)
            {
                MessageBox.Show("Добро пожаловать!");

                NavigationService.Navigate(new MainPage());
            }
            else
            {
                MessageBox.Show("Неверное имя пользователя или пароль");
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Register());
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }
    }
}
    