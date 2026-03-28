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
    /// Логика взаимодействия для Register.xaml
    /// </summary>
    public partial class Register : Page
    {
        public Register()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Регистрация, вынесенная в отдельный метод
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="confirmPassword"></param>
        /// <returns></returns>
        public bool RegisterUser(string username, string password, string confirmPassword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) ||
                    string.IsNullOrWhiteSpace(password) ||
                    string.IsNullOrWhiteSpace(confirmPassword))
                {
                    return false;
                }

                if (username.Length < 3)
                {
                    return false;
                }

                if (password.Length < 6)
                {
                    return false;
                }

                if (password != confirmPassword)
                {
                    return false;
                }

                var existingUser = Core.Context.Users
                    .FirstOrDefault(u => u.Username == username);

                if (existingUser != null)
                {
                    return false;
                }

                string passwordHash = HashPassword(password);

                var newUser = new Users
                {
                    Username = username,
                    PasswordHash = passwordHash,
                    CreatedAt = DateTime.Now
                };

                Core.Context.Users.Add(newUser);
                Core.Context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// Обработчик регистрации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            bool isRegistered = RegisterUser(
                UserNameBox.Text.Trim(),
                PasswordBox.Password,
                ConfirmPasswordBox.Password);

            if (isRegistered)
            {
                MessageBox.Show("Регистрация прошла успешно");
                NavigationService.Navigate(new Auth());
            }
            else
            {
                MessageBox.Show("Ошибка регистрации");
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Auth());
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Auth());
        }
    }
}
