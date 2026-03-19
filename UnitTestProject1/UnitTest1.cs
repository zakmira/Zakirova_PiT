using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Практическая_работа_4_Закирова.Pages;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            int res = 2 + 2;
            Assert.AreEqual(res, 4);
            Assert.AreNotEqual(res, 5);
            Assert.IsFalse(res > 5);
            Assert.IsTrue(res < 5);
        }


        /// <summary>
        /// Тест первой функции с корректными данными
        /// </summary>
        [TestMethod]
        public void CalculateFormula1_ValidData_ReturnsResult()
        {
            // Arrange
            double x = 1.0;
            double y = 8.0;  
            double z = 0.0;
            double expected = 2.0; 

            // Act
            double result = Page1.Calc(x, y, z);

            // Assert
            Assert.AreEqual(expected, result, 0.1);  
        }

        /// <summary>
        /// Тест первой функции, когда x = 0
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CalculateFormula1_XIsZero_ThrowsException()
        {
            // Arrange
            double x = 0.0;
            double y = 2.0;
            double z = 3.0;

            // Act
            Page1.Calc(x, y, z);
        }

        /// <summary>
        /// Тест первой функции, когда x = y
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CalculateFormula1_YEqualsX_ThrowsException()
        {
            // Arrange
            double x = 2.0;
            double y = 2.0;
            double z = 3.0;

            // Act
            Page1.Calc(x, y, z);
        }

        /// <summary>
        /// Тест первой ветви второй функции: x > |p|
        /// </summary>
        [TestMethod]
        public void CalculateFormula2_FirstBranch()
        {
            // Arrange
            double x = 5.0;
            double p = 2.0;
            double fx = Math.Exp(x); // e^x

            double expected = 2 * Math.Pow(fx, 3) + 3 * Math.Pow(p, 2);

            // Act
            double result = Page2.CalcFormula2(x, p, fx);

            // Assert
            Assert.AreEqual(expected, result, 0.0001);
        }

        /// <summary>
        /// Тест второй ветви второй функции: 3 < x < |p|
        /// </summary>
        [TestMethod]
        public void CalculateFormula2_SecondBranch()
        {
            // Arrange
            double x = 4.0;
            double p = 10.0;
            double fx = Math.Pow(x, 2);

            double expected = Math.Abs(fx - p);

            // Act
            double result = Page2.CalcFormula2(x, p, fx);

            // Assert
            Assert.AreEqual(expected, result, 0.0001);
        }

        /// <summary>
        /// Тест третьей ветви второй функции: x = |p|
        /// </summary>
        [TestMethod]
        public void CalculateFormula2_ThirdBranch()
        {
            // Arrange
            double x = 3.0;
            double p = -3.0; // |p| = 3
            double fx = Math.Sinh(x); // sh(3)

            // l = (sh(3) - (-3))²
            double expected = Math.Pow(fx - p, 2);

            // Act
            double result = Page2.CalcFormula2(x, p, fx);

            // Assert
            Assert.AreEqual(expected, result, 0.0001);
        }

        /// <summary>
        /// Тест второй функции, когда ни одна ветвь не подходит
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CalculateFormula2_NoBranchMatches()
        {
            // Arrange
            double x = 2.0; // x <= 3 и x < |p|
            double p = 5.0;
            double fx = Math.Exp(x);

            // Act
            Page2.CalcFormula2(x, p, fx);
        }

        /// <summary>
        /// Тест третьей функции при x = 0
        /// </summary>
        [TestMethod]
        public void CalculateFormula3_XisZero_ReturnsResult()
        {
            // Arrange
            double b = 2.3;
            double x = 0.0;

            double expected = Math.Sqrt(0 + Math.Exp(0.82));

            // Act
            double result = Page3.CalcFormula3(b, x);

            // Assert
            Assert.AreEqual(expected, result, 0.0001);
        }

        /// <summary>
        /// Тест третьей функции при x > 0
        /// </summary>
        [TestMethod]
        public void CalculateFormula3_PositiveX_ReturnsResult()
        {
            // Arrange
            double b = 2.3;
            double x = 4.0;

            double expected = 0.0025 * b * Math.Pow(x, 3) + Math.Sqrt(x + Math.Exp(0.82));

            // Act
            double result = Page3.CalcFormula3(b, x);

            // Assert
            Assert.AreEqual(expected, result, 0.0001);
        }

        /// <summary>
        /// Тест третьей функции при x < 0
        /// </summary>
        [TestMethod]
        public void CalculateFormula3_NegativeX_ReturnsResult()
        {
            // Arrange
            double b = 2.3;
            double x = -1.0;

            double expected = 0.0025 * b * Math.Pow(x, 3) + Math.Sqrt(x + Math.Exp(0.82));

            // Act
            double result = Page3.CalcFormula3(b, x);

            // Assert
            Assert.AreEqual(expected, result, 0.0001);
        }

        /// <summary>
        /// Тест третьей функции с отрицательным b
        /// </summary>
        [TestMethod]
        public void CalculateFormula3_NegativeB_ReturnsResult()
        {
            // Arrange
            double b = -2.3;
            double x = 2.0;
            double expected = 0.0025 * b * Math.Pow(x, 3) + Math.Sqrt(x + Math.Exp(0.82));

            // Act
            double result = Page3.CalcFormula3(b, x);

            // Assert
            Assert.AreEqual(expected, result, 0.0001);
        }

        /// <summary>
        /// Тест проверки области определения третьей функции (вне допустимых значений)
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CalculateFormula3_OutOfDomain_ThrowsException()
        {
            // Arrange
            double b = 2.3;
            double x = -Math.Exp(0.82) - 1.0; // x + e^0.82 < 0

            // Act
            Page3.CalcFormula3(b, x);
        }
    }
}
