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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            LoadMovies();
        }

        private void LoadMovies()
        {
            try
            {
                var movies = Core.Context.Movies.ToList();
                FilmsItems.ItemsSource = movies;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки фильмов: {ex.Message}");
            }
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            if (Core.CurrentUserId == null)
            {
                MessageBox.Show("Пожалуйста, войдите в аккаунт");

                NavigationService.Navigate(new Auth());
                return;
            }

            NavigationService.Navigate(new Pages.Profile());
        }

        private void Auth_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.Auth());
        }

        private void OpenFilm_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var movie = button?.DataContext as Movies;

            if (movie != null)
            {
                NavigationService.Navigate(new Pages.Film(movie.MovieID));
            }
        }

        public void SearchMovies(string searchText)
        {
            var movies = Core.Context.Movies.ToList();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                movies = movies
                    .Where(m => m.Title.ToLower().Contains(searchText.ToLower()))
                    .ToList();
            }

            FilmsItems.ItemsSource = movies;
        }

        public void SortMovies(string sortBy, bool ascending = true)
        {
            IQueryable<Movies> query = Core.Context.Movies;
            IEnumerable<Movies> sortedMovies;

            switch (sortBy.ToLower())
            {
                case "название":
                    sortedMovies = ascending
                        ? query.OrderBy(m => m.Title)
                        : query.OrderByDescending(m => m.Title);
                    break;
                case "рейтинг":
                    sortedMovies = ascending
                        ? query.OrderBy(m => m.Rating)
                        : query.OrderByDescending(m => m.Rating);
                    break;
                case "дата":
                    sortedMovies = ascending
                        ? query.OrderBy(m => m.ReleaseDate)
                        : query.OrderByDescending(m => m.ReleaseDate);
                    break;
                default:
                    sortedMovies = query;
                    break;
            }

            FilmsItems.ItemsSource = sortedMovies.ToList();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchMovies(SearchBox.Text);
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SortComboBox.SelectedItem is ComboBoxItem item)
            {
                var tag = item.Tag?.ToString()?.Split('|');
                if (tag != null && tag.Length == 2)
                {
                    string sortBy = tag[0];
                    bool ascending = bool.Parse(tag[1]);
                    SortMovies(sortBy, ascending);
                }
            }
        }
    }
}
