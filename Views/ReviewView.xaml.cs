using System.Windows.Controls;
using EscapeRoom.ViewModels;
using EscapeRoom.Models;
using System.Windows.Media;
using EscapeRoom.Services;
using System.Windows;
using System;
using System.Collections.ObjectModel;

namespace EscapeRoom.Views
{
    public partial class ReviewView : UserControl
    {
        private ReviewViewModel _viewModel;
        public ReviewView()
        {
            InitializeComponent();
            _viewModel = new ReviewViewModel();
            this.DataContext = _viewModel;
            LoadReviewsForCurrentRoom();

        }

        public ReviewView(Room selectedRoom)
        {
            InitializeComponent();
            _viewModel = new ReviewViewModel(selectedRoom);
            this.DataContext = _viewModel;
            LoadReviewsForCurrentRoom();
        }

        private async void LoadReviewsForCurrentRoom()
        {
            try
            {
                await _viewModel.LoadReviewsForRoomAsync(); //baza danych
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Błąd podczas ładowania recenzji: {ex.Message}");
            }
        }

        private void OnAddReviewButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (ValidateReviewForm())
                {
                    SaveReview();
                    ClearReviewForm();
                    System.Windows.MessageBox.Show("Dziękujemy za opinię!",
                        "Sukces", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

                    // Odśwież listę recenzji po dodaniu nowej
                    LoadReviewsForCurrentRoom();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Błąd podczas dodawania opinii: {ex.Message}");
            }
        }

        private void OnBackButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewNavigationService.Instance.NavigateTo(ViewType.Homepage);
        }

        private bool ValidateReviewForm()
        {
            // Sprawdź czy użytkownik jest zalogowany
            if (!UserSession.IsLoggedIn)
            {
                ShowErrorMessage("Musisz być zalogowany, aby dodać opinię.");
                return false;
            }

            string reviewText = GetTextBoxValue("ReviewTextBox");
            if (string.IsNullOrWhiteSpace(reviewText))
            {
                ShowErrorMessage("Proszę napisać opinię.");
                return false;
            }

            return true;
        }

        private async void SaveReview()
        {
            try
            {
                var newReview = new Review
                {
                    UzytkownikId = UserSession.CurrentUser.UzytkownikId,
                    PokojId = _viewModel.CurrentRoom.PokojId,
                    Komentarz = GetTextBoxValue("ReviewTextBox"),
                    DataUtworzenia = DateTime.Now
                };

                await _viewModel.AddReviewAsync(newReview);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Błąd podczas zapisywania opinii: {ex.Message}");
            }
        }

        private void ClearReviewForm()
        {
            (this.FindName("ReviewerNameTextBox") as TextBox)?.Clear();
            (this.FindName("ReviewTextBox") as TextBox)?.Clear();
        }

        private string GetTextBoxValue(string textBoxName)
        {
            var textBox = this.FindName(textBoxName) as TextBox;
            return textBox?.Text ?? string.Empty;
        }

        private void ShowErrorMessage(string message)
        {
            System.Windows.MessageBox.Show(message, "Błąd",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
        }


        private async void OnDeleteReviewClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var review = button?.DataContext as ReviewViewModel;

                if (review != null)
                {
                    var result = MessageBox.Show(
                        "Czy na pewno chcesz usunąć tę recenzję?",
                        "Potwierdzenie",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        await _viewModel.DeleteReviewAsync(review.Id); //baza danych
                        LoadReviewsForCurrentRoom(); // Odśwież listę
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Błąd podczas usuwania recenzji: {ex.Message}");
            }
        }


    }
}