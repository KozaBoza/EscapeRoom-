using System.Windows.Controls;
using EscapeRoom.ViewModels;
using EscapeRoom.Models;
using System.Windows.Media;
using EscapeRoom.Services;
using System.Windows;
using System;

namespace EscapeRoom.Views
{
    public partial class ReviewView : UserControl
    {
        public ReviewView()
        {
            InitializeComponent();
            LoadReviews();

        }
        private void LoadReviews()
        {
            // Tutaj możesz załadować opinie z bazy danych
            // Na razie symulacja danych
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
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Błąd podczas dodawania opinii: {ex.Message}");
            }
        }

        private void OnBackButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Instance.NavigateTo(ViewType.Homepage);
        }

        private bool ValidateReviewForm()
        {
            string reviewerName = GetTextBoxValue("ReviewerNameTextBox");
            if (string.IsNullOrWhiteSpace(reviewerName))
            {
                ShowErrorMessage("Proszę podać swoje imię.");
                return false;
            }

            string reviewText = GetTextBoxValue("ReviewTextBox");
            if (string.IsNullOrWhiteSpace(reviewText))
            {
                ShowErrorMessage("Proszę napisać opinię.");
                return false;
            }

            // Sprawdzenie czy wybrano ocenę
            var ratingComboBox = this.FindName("RatingComboBox") as ComboBox;
            if (ratingComboBox?.SelectedItem == null)
            {
                ShowErrorMessage("Proszę wybrać ocenę.");
                return false;
            }

            return true;
        }

        private void SaveReview()
        {
            // Tutaj zapisałbyś opinię do bazy danych
            // Na razie tylko symulacja
        }

        private void ClearReviewForm()
        {
            (this.FindName("ReviewerNameTextBox") as TextBox)?.Clear();
            (this.FindName("ReviewTextBox") as TextBox)?.Clear();
            (this.FindName("RatingComboBox") as ComboBox)?.ClearValue(ComboBox.SelectedItemProperty);
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
    }
}