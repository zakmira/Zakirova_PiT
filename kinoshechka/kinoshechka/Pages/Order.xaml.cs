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

namespace kinoshechka.Pages
{
    /// <summary>
    /// Логика взаимодействия для Order.xaml
    /// </summary>
    public partial class Order : Page
    {
        private int _sessionId;
        private int _userId;
        private List<kinoshechka.Seats> _selectedSeats;
        private decimal _totalPrice;

        public Order()
        {
            InitializeComponent();
        }

        public Order(int sessionId, int userId, List<kinoshechka.Seats> selectedSeats, decimal totalPrice) : this()
        {
            _sessionId = sessionId;
            _userId = userId;
            _selectedSeats = selectedSeats;
            _totalPrice = totalPrice;

            LoadOrderInfo();
        }

        private void LoadOrderInfo()
        {
            try
            {
                var sessionData = Core.Context.Sessions
                    .FirstOrDefault(s => s.SessionID == _sessionId);

                if (sessionData == null)
                {
                    MessageBox.Show("Сеанс не найден!");
                    return;
                }

                var movie = Core.Context.Movies
                    .FirstOrDefault(m => m.MovieID == sessionData.MovieID);

                var hall = Core.Context.Halls
                    .FirstOrDefault(h => h.HallID == sessionData.HallID);

                if (movie != null)
                {
                    MovieTitleText.Text = movie.Title;
                }
                else
                {
                    MovieTitleText.Text = "Неизвестно";
                }

                if (hall != null)
                {
                    HallNameText.Text = hall.Name;
                }
                else
                {
                    HallNameText.Text = "Неизвестно";
                }

                SessionDateText.Text = sessionData.SessionDate.ToString("dd.MM.yyyy");
                SessionTimeText.Text = sessionData.SessionTime.ToString("hh\\:mm");

                if (_selectedSeats != null && _selectedSeats.Count > 0)
                {
                    var seatNumbers = string.Join(", ", _selectedSeats.Select(s => s.SeatNumber));
                    SelectedSeatsText.Text = seatNumbers;
                }
                else
                {
                    SelectedSeatsText.Text = "Не выбраны";
                }

                TotalPriceText.Text = _totalPrice.ToString("F2") + " ₽";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message + "\n\n" + ex.StackTrace);
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedSeats == null || _selectedSeats.Count == 0)
                {
                    MessageBox.Show("Нет выбранных мест!");
                    return;
                }

                var ticketPrice = Core.Context.Tickets
                    .Where(t => t.SessionID == _sessionId)
                    .Select(t => t.Price)
                    .FirstOrDefault();

                if (ticketPrice == 0)
                {
                    ticketPrice = 350m;
                }

                foreach (var seat in _selectedSeats)
                {
                    var ticket = new Tickets
                    {
                        UserID = _userId,
                        SessionID = _sessionId,
                        SeatID = seat.SeatID,
                        Price = ticketPrice,
                        PurchaseDate = DateTime.Now
                    };

                    Core.Context.Tickets.Add(ticket);
                }

                Core.Context.SaveChanges();

                MessageBox.Show("Успешно!");

                NavigationService.Navigate(new MainPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message + "\n\n" + ex.StackTrace);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
