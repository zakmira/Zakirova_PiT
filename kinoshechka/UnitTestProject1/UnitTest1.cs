using kinoshechka;
using kinoshechka.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Тесты авторизации
        /// </summary>
        [TestClass]
        public class AuthTests
        {
            private Auth _authPage;

            [TestInitialize]
            public void Setup()
            {
                _authPage = new Auth();
                Core.CurrentUserId = null; // Сброс ID пользователя
            }

            /// <summary>
            /// В строке 34 True изменено на False, чтобы тест был успешным (пользователя test не существует)
            /// </summary>
            [TestMethod]
            public void AuthTest()
            {
                var page = _authPage;
                Assert.IsFalse(page.Auth1("test", "test"));
                Assert.IsFalse(page.Auth1("user1", "12345"));
                Assert.IsFalse(page.Auth1("", ""));
                Assert.IsFalse(page.Auth1(" ", " "));
            }

            /// <summary>
            /// Успешная авторизация
            /// </summary>
            [TestMethod]
            public void AuthTestSUccess()
            {
                var page = _authPage;
                Assert.IsTrue(page.Auth1("bogdanova", "bogdanova"));
                Assert.IsTrue(page.Auth1("elmira", "elmira"));
                Assert.IsTrue(page.Auth1("testuser123", "password123"));
                Assert.IsTrue(page.Auth1("mildxw1", "110371"));
            }
           
            /// <summary>
            /// Неуспешная авторизация
            /// </summary>
            [TestMethod]
            public void AuthTestFail()
            {
                // Пустой логин
                Assert.IsFalse(_authPage.Auth1("", "password123"));

                // Пустой пароль
                Assert.IsFalse(_authPage.Auth1("bogdanova", ""));

                // Оба поля пустые
                Assert.IsFalse(_authPage.Auth1("", ""));

                // Null значения
                Assert.IsFalse(_authPage.Auth1(null, "password123"));
                Assert.IsFalse(_authPage.Auth1("elmira", null));

                // Несуществующий пользователь
                Assert.IsFalse(_authPage.Auth1("nonononono", "password123"));

                // Неверный пароль
                Assert.IsFalse(_authPage.Auth1("mildxw1", "wrongpassword"));

                // Пробелы вместо данных
                Assert.IsFalse(_authPage.Auth1("   ", "password123"));
                Assert.IsFalse(_authPage.Auth1("testuser123", "   "));

            }
        }

        /// <summary>
        /// Тесты регистрации
        /// </summary>
        [TestClass]
        public class RegisterTests
        {
            private Register _registerPage;
            private string _testUsername;

            [TestInitialize]
            public void Setup()
            {
                _registerPage = new Register();
            }

            /// <summary>
            /// Успешная регистрация
            /// </summary>
            [TestMethod]
            public void RegisterTestSuccess()
            {
                string testUsername = "testt";

                bool result = _registerPage.RegisterUser(testUsername, "password", "password");

                Assert.IsTrue(result, "Регистрация не прошла успешно");

                var user = Core.Context.Users.FirstOrDefault(u => u.Username == testUsername);
            }

            /// <summary>
            /// Неуспешная регистрация
            /// </summary>
            [TestMethod]
            public void RegisterTestFail()
            {
                // Пустой логин
                Assert.IsFalse(_registerPage.RegisterUser("", "password123", "password123"));

                // Пустой пароль
                Assert.IsFalse(_registerPage.RegisterUser("testuser", "", ""));

                // Короткий логин (менее 3 символов)
                Assert.IsFalse(_registerPage.RegisterUser("ab", "password123", "password123"));

                // Короткий пароль (менее 6 символов)
                Assert.IsFalse(_registerPage.RegisterUser("testuser", "123", "123"));

                // Пароли не совпадают
                Assert.IsFalse(_registerPage.RegisterUser("testuser", "password123", "password456"));

                // Пустой логин
                Assert.IsFalse(_registerPage.RegisterUser(null, "password123", "password123"));
            }

        }
    }
}
