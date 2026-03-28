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
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        private int? _currentUserId;

        public Profile()
        {
            InitializeComponent();
            LoadProfileData();
        }

        private void LoadProfileData()
        {
            if (Core.CurrentUserId == null)
            {
                MessageBox.Show("Требуется авторизация");

                NavigationService.Navigate(new Auth());
                return;
            }

            try
            {
                int userId = Core.CurrentUserId.Value;

                var user = Core.Context.Users
                    .FirstOrDefault(u => u.UserID == userId);

                if (user != null)
                {
                    UserNameText.Text = user.Username;


                    var tickets = Core.Context.Tickets
                        .Where(t => t.UserID == userId)
                        .Join(
                            Core.Context.Sessions,
                            ticket => ticket.SessionID,
                            session => session.SessionID,
                            (ticket, session) => new { Ticket = ticket, Session = session }
                        )
                        .Join(
                            Core.Context.Movies,
                            temp => temp.Session.MovieID,
                            movie => movie.MovieID,
                            (temp, movie) => new { temp.Ticket, temp.Session, Movie = movie }
                        )
                        .Join(
                            Core.Context.Halls,
                            temp => temp.Session.HallID,
                            hall => hall.HallID,
                            (temp, hall) => new { temp.Ticket, temp.Session, temp.Movie, Hall = hall }
                        )
                        .Join(
                            Core.Context.Seats,
                            temp => temp.Ticket.SeatID,
                            seat => seat.SeatID,
                            (temp, seat) => new
                            {
                                temp.Ticket,
                                temp.Session,
                                temp.Movie,
                                temp.Hall,
                                Seat = seat
                            }
                        )
                        .OrderByDescending(t => t.Session.SessionDate)
                        .Select(t => new
                        {
                            TicketID = t.Ticket.TicketID,
                            MovieTitle = t.Movie.Title,
                            CoverImage = t.Movie.CoverImage,
                            SessionDate = t.Session.SessionDate,
                            SessionTime = t.Session.SessionTime,
                            HallName = t.Hall.Name,
                            SeatNumber = t.Seat.SeatNumber,
                            Price = t.Ticket.Price,
                            PurchaseDate = t.Ticket.PurchaseDate
                        })
                        .ToList();

                    TotalTicketsText.Text = tickets.Count.ToString() + " шт.";

                    TicketsItems.ItemsSource = tickets;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }
    }
}




