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
    /// Логика взаимодействия для Film.xaml
    /// </summary>
    public partial class Film : Page
    {
        private int _movieId;

        public Film()
        {
            InitializeComponent();
        }

        public Film(int movieId) : this()
        {
            _movieId = movieId;
            LoadFilmData();
        }

        private void LoadFilmData()
        {
            try
            {
                var movie = Core.Context.Movies
                    .FirstOrDefault(m => m.MovieID == _movieId);

                if (movie == null)
                {
                    MessageBox.Show("Фильм не найден");
                    NavigationService.Navigate(new MainPage());
                    return;
                }

                TitleText.Text = movie.Title;
                RatingText.Text = movie.Rating.HasValue ? movie.Rating.Value.ToString("F1") : "Нет рейтинга";
                AgeRatingText.Text = movie.AgeRating;
                ReleaseDateText.Text = movie.ReleaseDate.HasValue ? movie.ReleaseDate.Value.ToString("dd.MM.yyyy") : "Дата неизвестна";
                DescriptionText.Text = movie.ShortDescription ?? "Описание отсутствует";

                if (!string.IsNullOrEmpty(movie.CoverImage))
                {
                    try
                    {
                        string imagePath = movie.CoverImage;

                        if (!imagePath.StartsWith("pack://") && !imagePath.StartsWith("http"))
                        {
                            imagePath = $"pack://application:,,,/{imagePath}";
                        }

                        Image.Source = new System.Windows.Media.Imaging.BitmapImage(
                            new Uri(imagePath, UriKind.Absolute));
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            Image.Source = new System.Windows.Media.Imaging.BitmapImage(
                                new Uri(movie.CoverImage, UriKind.RelativeOrAbsolute));
                        }
                        catch
                        {
                            Image.Source = null;
                        }
                    }
                }

                try
                {
                    var genres = Core.Context.MovieGenres
                        .Where(mg => mg.MovieID == _movieId)
                        .Join(
                            Core.Context.Genres,
                            mg => mg.GenreID,
                            g => g.GenreID,
                            (mg, g) => g.Name
                        )
                        .ToList();

                    GenresText.Text = genres.Any() ? string.Join(", ", genres) : "Не указано";
                }
                catch
                {
                    GenresText.Text = "Не указано";
                }

                var sessions = Core.Context.Sessions
                    .Where(s => s.MovieID == _movieId)
                    .Join(
                        Core.Context.Halls,
                        session => session.HallID,
                        hall => hall.HallID,
                        (session, hall) => new
                        {
                            SessionID = session.SessionID,
                            SessionDate = session.SessionDate,
                            SessionTime = session.SessionTime,
                            HallName = hall.Name,
                            HallClassification = hall.Classification
                        }
                    )
                    .OrderBy(s => s.SessionDate)
                    .ThenBy(s => s.SessionTime)
                    .ToList();

                SessionsItems.ItemsSource = sessions;
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

        public void SelectSeats_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var session = button?.DataContext as dynamic;

            if (session != null)
            {
                if (Core.CurrentUserId == null)
                {
                    MessageBox.Show("Для покупки билета требуется авторизация");

                    NavigationService.Navigate(new Auth());
                    return;
                }

                NavigationService.Navigate(new SeatsPage(session.SessionID, Core.CurrentUserId.Value));
            }
        }
    }
       
}

    


       