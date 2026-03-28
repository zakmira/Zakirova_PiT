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
    /// Логика взаимодействия для Seats.xaml
    /// </summary>
    public partial class SeatsPage : Page
    {
            private int _sessionId;
                private int _userId;
                private List<Seats> _allSeats;
                private List<Seats> _selectedSeats;
                private decimal _ticketPrice;

                public SeatsPage(int sessionId, int userId)
                {
                    InitializeComponent();
                    _sessionId = sessionId;
                    _userId = userId;
                    _selectedSeats = new List<Seats>();

                    LoadSessionInfo();
                    LoadSeats();
                }

                private void LoadSessionInfo()
                {
                    var session = Core.Context.Sessions
                        .FirstOrDefault(s => s.SessionID == _sessionId);

                    if (session != null)
                    {
                        var hall = Core.Context.Halls
                            .FirstOrDefault(h => h.HallID == session.HallID);

                        var movie = Core.Context.Movies
                            .FirstOrDefault(m => m.MovieID == session.MovieID);

                        if (hall != null)
                        {
                            _ticketPrice = hall.Price;
                            PriceInfoText.Text = $"Цена билета: {_ticketPrice} ₽";
                            HallTypeText.Text = hall.Classification == "VIP" ? "VIP зал" : "Стандарт";
                            HallNameText.Text = hall.Name;
                        }

                        if (movie != null)
                        {
                            MovieTitleText.Text = movie.Title;
                        }

                        SessionDateText.Text = $"Дата: {session.SessionDate:dd.MM.yyyy}";
                        SessionTimeText.Text = $"Время: {session.SessionTime:hh\\:mm}";
                    }
                }

                private void LoadSeats()
                {
                    var session = Core.Context.Sessions
                        .FirstOrDefault(s => s.SessionID == _sessionId);

                    if (session == null) return;

                    var hallId = session.HallID;

                    _allSeats = Core.Context.Seats
                        .Where(s => s.HallID == hallId)
                        .OrderBy(s => s.SeatNumber.Length)
                        .ThenBy(s => s.SeatNumber)
                        .ToList();

                    var occupiedSeatIds = Core.Context.Tickets
                        .Where(t => t.SessionID == _sessionId)
                        .Select(t => t.SeatID)
                        .ToList();

                    var seatsWithStatus = _allSeats.Select(seat => new
                    {
                        Seat = seat,
                        IsOccupied = occupiedSeatIds.Contains(seat.SeatID)
                    }).ToList();

                    SeatsItems.ItemsSource = seatsWithStatus;
                }

                private void UpdateSelectionInfo()
                {
                    int count = _selectedSeats.Count;
                    decimal total = count * _ticketPrice;

                    SelectedSeatsText.Text = count > 0
                        ? $"Выбрано мест: {count}"
                        : "Места не выбраны";

                    TotalPriceText.Text = $"Итого: {total} ₽";
                }

                private void Seat_Click(object sender, RoutedEventArgs e)
                {
                    var button = sender as Button;
                    var seatData = button?.DataContext as dynamic;

                    if (seatData != null && !seatData.IsOccupied)
                    {
                        var seat = seatData.Seat as Seats;

                        if (_selectedSeats.Contains(seat))
                        {
                            _selectedSeats.Remove(seat);
                        }
                        else
                        {
                            _selectedSeats.Add(seat);
                        }

                        UpdateSelectionInfo();
                    }
                }

                private void BuyTickets_Click(object sender, RoutedEventArgs e)
                {
                    if (_selectedSeats.Count == 0)
                    {
                        MessageBox.Show("Выберите места");
                        return;
                    }

                    NavigationService.Navigate(new Order(
                        _sessionId,
                        _userId,
                        _selectedSeats,
                        _selectedSeats.Count * _ticketPrice
                    ));
                }

                private void Clear_Click(object sender, RoutedEventArgs e)
                {
                    _selectedSeats.Clear(); 
                    UpdateSelectionInfo();
        }

                private void Back_Click(object sender, RoutedEventArgs e)
                {
                    NavigationService.GoBack();
                }
            }
        }





