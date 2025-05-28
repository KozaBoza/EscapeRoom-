using System.Windows.Controls;
using EscapeRoom.ViewModels;
using EscapeRoom.Models;
using EscapeRoom.Services;
using System.Windows;
using System;

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
            // 
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
                NavigationService.Instance.NavigateTo(ViewType.Homepage);
            }
        }

        private void CheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                //???newsletter
                string checkBoxName = checkBox.Name;
                bool isChecked = checkBox.IsChecked == true;

                switch (checkBoxName)
                {
                    case "NewsletterCheckBox":
                        HandleNewsletterSubscription(isChecked);
                        break;
                    case "NotificationsCheckBox":
                        HandleNotificationPreference(isChecked);
                        break;
                    default:
                        //checkboxy
                        break;
                }
            }
        }

        private void HandleNewsletterSubscription(bool subscribe)
        {
            string message = subscribe ?
                "Zostałeś zapisany do newslettera." :
                "Zostałeś wypisany z newslettera.";

            System.Windows.MessageBox.Show(message, "Newsletter",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }

        private void HandleNotificationPreference(bool enable)
        {
            string message = enable ?
                "Powiadomienia zostały włączone." :
                "Powiadomienia zostały wyłączone.";

            System.Windows.MessageBox.Show(message, "Powiadomienia",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
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

        private void SaveProfileChanges()
        {
           //symulacja poki co
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