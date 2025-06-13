using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using EscapeRoom.ViewModels;
using EscapeRoom.Models;
using System.Windows.Media;
using System.Windows;
using EscapeRoom.Services;

namespace EscapeRoom.Views
{ //obsluga przycisków, error message
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
            // Zarejestruj zdarzenia przycisków
            DataContext = new LoginViewModel();
        }

//        private void OnLoginButtonClick(object sender, System.Windows.RoutedEventArgs e)
//        {
//            try
//            {
//                string username = GetTextBoxValue("UsernameTextBox");
//                string password = GetPasswordBoxValue("PasswordInput");

//                if (string.IsNullOrWhiteSpace(username))
//                {
//                    ShowErrorMessage("Proszę podać nazwę użytkownika.");
//                    return;
//                }

//                if (string.IsNullOrWhiteSpace(password))
//                {
//                    ShowErrorMessage("Proszę podać hasło.");
//                    return;
//                }

////
//                if (ValidateCredentials(username, password))
//                {
//                    if (username.ToLower() == username)
//                    {
//                        System.Windows.MessageBox.Show("Zalogowano jako administrator!",
//                            "Sukces", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
//                        ViewNavigationService.Instance.NavigateTo(ViewType.AdminDashboard);
//                    }
//                    else
//                    {
//                        System.Windows.MessageBox.Show("Zalogowano pomyślnie!",
//                            "Sukces", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
//                        ViewNavigationService.Instance.NavigateTo(ViewType.User);
//                    }
//                }
//                else
//                {
//                    ShowErrorMessage("Nieprawidłowa nazwa użytkownika lub hasło.");
//                    ClearPassword();
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowErrorMessage($"Błąd podczas logowania: {ex.Message}");
//            }
//        }

        private void OnRegisterButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Funkcja rejestracji zostanie wkrótce zaimplementowana.",
                "Rejestracja", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }

        private void OnBackButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewNavigationService.Instance.NavigateTo(ViewType.Homepage);
        }

        private void PasswordInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Możesz tutaj dodać walidację hasła w czasie rzeczywistym
            //var passwordBox = sender as PasswordBox;
            //if (passwordBox != null)
            //{
            //    // Przykład: zmiana koloru obramowania w zależności od długości hasła
            //    if (passwordBox.Password.Length < 4)
            //    {
            //        passwordBox.BorderBrush = Brushes.Red;
            //    }
            //    else
            //    {
            //        passwordBox.BorderBrush = Brushes.Green;
            //    }
            //}

            var passwordBox = sender as PasswordBox;
            var vm = DataContext as LoginViewModel;
            if (vm != null && passwordBox != null)
                vm.Password = passwordBox.Password;
        }

        private bool ValidateCredentials(string username, string password)
        {
           
            return (username.ToLower() == username && password == username) ||
                   (username.ToLower() == "user" && password == "user123");
        }

        private string GetTextBoxValue(string textBoxName)
        {
            var textBox = this.FindName(textBoxName) as TextBox;
            return textBox?.Text ?? string.Empty;
        }

        private string GetPasswordBoxValue(string passwordBoxName)
        {
            var passwordBox = this.FindName(passwordBoxName) as PasswordBox;
            return passwordBox?.Password ?? string.Empty;
        }

        private void ShowErrorMessage(string message)
        {
            System.Windows.MessageBox.Show(message, "Błąd logowania",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
        }

        private void ClearPassword()
        {
            var passwordBox = this.FindName("PasswordInput") as PasswordBox;
            passwordBox?.Clear();
        }
    }
}

