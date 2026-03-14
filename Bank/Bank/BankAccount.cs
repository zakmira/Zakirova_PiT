using System;

namespace BankAccountNS
{
    /// <summary>
    /// Bank account demo class.
    /// </summary>

    public class BankAccount
    {
        /// <summary>
        /// Две константы для сообщений об ошибках в области видимости класса.
        /// </summary>
        public const string DebitAmountExceedsBalanceMessage = "Debit amount exceeds balance";
        public const string DebitAmountLessThanZeroMessage = "Debit amount is less than zero";

        /// <summary>
        /// Еще одна аналогичная константа.
        /// </summary>
        public const string CreditAmountLessThanZeroMessage = "Credit amount is less than zero";


        private readonly string m_customerName;
        private double m_balance;

        /// <summary>
        /// Приватный конструктор по умолчанию.
        /// </summary>
        private BankAccount() { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="BankAccount"/>.
        /// </summary>
        
        /// <param name="customerName">
        /// Имя владельца счета.
        /// </param>
        
        /// <param name="balance">
        /// Начальный баланс счета.
        /// </param>
        public BankAccount(string customerName, double balance)
        {
            m_customerName = customerName;
            m_balance = balance;
        }

        /// <summary>
        /// Получает имя владельца счета.
        /// </summary>
        
        /// <value>
        /// Имя клиента в виде строки.
        /// </value>
        public string CustomerName
        {
            get { return m_customerName; }
        }

        /// <summary>
        /// Получает текущий баланс счета.
        /// </summary>
        
        /// <value>
        /// Текущая сумма на счете.
        /// </value>
        public double Balance
        {
            get { return m_balance; }
        }

        /// <summary>
        /// Списывает указанную сумму со счета (дебетование).
        /// </summary>
        
        /// <param name="amount">
        /// Сумма, которую необходимо списать.
        /// </param>
        
        /// <exception cref="ArgumentOutOfRangeException">
        /// Возникает, если сумма больше текущего баланса или меньше нуля.
        /// </exception>
        public void Debit(double amount)
        {
            if (amount > m_balance)
            {
                throw new System.ArgumentOutOfRangeException("amount", amount, DebitAmountExceedsBalanceMessage);
            }

            if (amount < 0)
            {
                throw new System.ArgumentOutOfRangeException("amount", amount, DebitAmountLessThanZeroMessage);
            }

            m_balance -= amount;
        }

        /// <summary>
        /// Зачисляет указанную сумму на счет (кредитование).
        /// </summary>

        /// <param name="amount">
        /// Сумма, которую необходимо зачислить.
        /// </param>

        /// <exception cref="ArgumentOutOfRangeException">
        /// Возникает, если сумма меньше нуля.
        /// </exception>
        public void Credit(double amount)
        {
            if (amount < 0)
            {
                throw new System.ArgumentOutOfRangeException("amount", amount, CreditAmountLessThanZeroMessage);
            }

            m_balance += amount;
        }


        /// <summary>
        /// Точка входа в консольное приложение.
        /// </summary>

        /// <remarks>
        /// Создает экземпляр счета, performs операции и выводит баланс.
        /// </remarks>
        public static void Main()
        {
            BankAccount ba = new BankAccount("Mr. Roman Abramovich", 11.99);
            ba.Credit(5.77);
            ba.Debit(11.22);
            Console.WriteLine("Current balance is ${0}", ba.Balance);
            Console.ReadLine();
        }
    }
}