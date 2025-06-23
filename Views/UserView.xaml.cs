using EscapeRoom.Data;
using EscapeRoom.Models;
using EscapeRoom.Services;
using EscapeRoom.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace EscapeRoom.Views
{ //obsługa przycisków 
    public partial class UserView : UserControl
    {
        public UserView()
        {
            InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            // Ustaw DataContext na nowy UserViewModel, który pobierze dane z UserSession
            this.DataContext = new UserViewModel();
        }

        private void OnEditProfileButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                EnableProfileEditing(true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Błąd podczas edycji profilu: {ex.Message}");
            }
        }

        private void OnSaveProfileButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (ValidateProfileData())
                {
                    SaveProfileChanges();
                    EnableProfileEditing(false);
                    System.Windows.MessageBox.Show("Profil został zaktualizowany.",
                        "Sukces", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Błąd podczas zapisywania profilu: {ex.Message}");
            }
        }

        private void OnCancelEditButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            EnableProfileEditing(false);
            LoadUserData(); 
        }

        private void OnViewReservationsButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Wyświetlanie historii rezerwacji użytkownika...",
                "Moje rezerwacje", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }

        private void OnLogoutButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            var result = System.Windows.MessageBox.Show("Czy na pewno chcesz się wylogować?",
                "Wylogowanie", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                ViewNavigationService.Instance.NavigateTo(ViewType.Homepage);
            }
        }
        private void EnableProfileEditing(bool enable)
        {
            (this.FindName("NameTextBox") as TextBox)?.SetValue(TextBox.IsReadOnlyProperty, !enable);
            (this.FindName("EmailTextBox") as TextBox)?.SetValue(TextBox.IsReadOnlyProperty, !enable);
            (this.FindName("PhoneTextBox") as TextBox)?.SetValue(TextBox.IsReadOnlyProperty, !enable);

            // Pokaż/ukryj odpowiednie przyciski
            (this.FindName("EditProfileButton") as Button)?.SetValue(Button.VisibilityProperty,
                enable ? Visibility.Collapsed : Visibility.Visible);
            (this.FindName("SaveProfileButton") as Button)?.SetValue(Button.VisibilityProperty,
                enable ? Visibility.Visible : Visibility.Collapsed);
            (this.FindName("CancelEditButton") as Button)?.SetValue(Button.VisibilityProperty,
                enable ? Visibility.Visible : Visibility.Collapsed);
        }

        private bool ValidateProfileData()
        {
            string name = GetTextBoxValue("NameTextBox");
            if (string.IsNullOrWhiteSpace(name))
            {
                ShowErrorMessage("Proszę podać imię i nazwisko.");
                return false;
            }

            string email = GetTextBoxValue("EmailTextBox");
            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
            {
                ShowErrorMessage("Proszę podać prawidłowy adres email.");
                return false;
            }

            return true;
        }

        private async void SaveProfileChanges()
        {
            try
            {
                var viewModel = (UserViewModel)DataContext;
                var dataService = new DataService();

                var success = await dataService.UpdateUserAsync(viewModel.GetUser());

                if (success)
                {
                    // Aktualizuj dane w sesji
                    UserSession.CurrentUser = viewModel.GetUser();

                    System.Windows.MessageBox.Show(
                        "Dane zostały pomyślnie zaktualizowane.",
                        "Sukces",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Information);
                }
                else
                {
                    System.Windows.MessageBox.Show(
                        "Nie udało się zaktualizować danych. Spróbuj ponownie.",
                        "Błąd",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Wystąpił błąd podczas aktualizacji danych: {ex.Message}",
                    "Błąd",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
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