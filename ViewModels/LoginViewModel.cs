using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EscapeRoom.Data;
using EscapeRoom.Helpers;
using Org.BouncyCastle.Crypto.Generators;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using EscapeRoom.Views;

namespace EscapeRoom.ViewModels
{ //obsługa logowania
    public class LoginViewModel : BaseViewModel
    {
        private string _username;
        private string _password;
        private string _errorMessage;
        private bool _isLoading;

        public string Username
        {
            get => _username;
            set
            {
                if (SetProperty(ref _username, value))
                    OnPropertyChanged(nameof(CanLogin));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (SetProperty(ref _password, value))
                    OnPropertyChanged(nameof(CanLogin));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (SetProperty(ref _isLoading, value))
                    OnPropertyChanged(nameof(CanLogin));
            }
        }

        public bool CanLogin =>
            !string.IsNullOrWhiteSpace(Username) &&
            !string.IsNullOrWhiteSpace(Password); /*&&
            !IsLoading;*/

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand ForgotPasswordCommand { get; }

        public event EventHandler<LoginEventArgs> LoginSuccessful;

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(ExecuteLogin, _ => CanLogin);
            RegisterCommand = new RelayCommand(ExecuteRegister);
            ForgotPasswordCommand = new RelayCommand(ExecuteForgotPassword);
        }

        private async void ExecuteLogin(object parameter)
        {
            ErrorMessage = string.Empty;
            IsLoading = true;

            try
            {
                DataService service = new DataService();
                var user = await service.GetUserByEmailAsync(Username);

                if (user == null)
                {
                    ErrorMessage = "Nie znaleziono użytkownika.";
                    return;
                }

                // Sprawdź hash hasła
                if (!VerifyPBKDF2Hash(Password, user.HasloHash))
                {
                    ErrorMessage = "Nieprawidłowe hasło.";
                    return;
                }

                // Sukces: logowanie udane - zapisz dane użytkownika globalnie
                UserSession.CurrentUser = user;
                UserSession.IsLoggedIn = true;

                OnLoginSuccessful(new LoginEventArgs(user.Email, user.Admin));
                MessageBox.Show($"Zalogowano jako {user.Imie} {user.Nazwisko}", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

                //Admin
                if (user.Admin)
                {
                    //  główne okno
                    var mainWindow = Application.Current.Windows
                        .OfType<Window>()
                        .FirstOrDefault(w => w is MainWindow) as MainWindow;

                    if (mainWindow != null)
                    {
                        mainWindow.MainContentControl.Content = new AdminDashboardView();
                    }

                    Application.Current.Windows
                        .OfType<Window>()
                        .FirstOrDefault(w => w != mainWindow && w.IsActive)
                        ?.Close();
                }
                // Użytkownik
                else
                {
                    // Znajdź główne okno
                    var mainWindow = Application.Current.Windows
                        .OfType<Window>()
                        .FirstOrDefault(w => w is MainWindow) as MainWindow;

                    if (mainWindow != null)
                    {
                        mainWindow.MainContentControl.Content = new UserView();
                    }

                    // Zamknij okno logowania, jeśli jest osobne
                    Application.Current.Windows
                        .OfType<Window>()
                        .FirstOrDefault(w => w != mainWindow && w.IsActive)
                        ?.Close();

                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Błąd logowania: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ExecuteRegister(object parameter)
        {
            //opcja: przekierować do widoku rejestracji
        }

        private async void ExecuteForgotPassword(object parameter)
        {
            // pytanie o numer telefonu
            string phoneNumber = Microsoft.VisualBasic.Interaction.InputBox(
                "Podaj swój numer telefonu:",
                "Przypomnienie hasła",
                "");

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return; //  anulowanie
            }

            try
            {
                DataService service = new DataService();
                var user = await service.GetUserByPhoneAsync(phoneNumber); //tutaj trzeba z baza dostosować

                if (user == null)
                {
                    MessageBox.Show("Nie znaleziono użytkownika z podanym numerem telefonu.",
                        "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Tutaj możesz dodać logikę wysyłania nowego hasła SMS/email
                MessageBox.Show($"Nowe hasło zostało wysłane na numer {phoneNumber}",
                    "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

                // Przykład: wygeneruj tymczasowe hasło i zaktualizuj w bazie
                // string tempPassword = GenerateTemporaryPassword();
                // await service.UpdateUserPasswordAsync(user.UzytkownikId, GeneratePBKDF2Hash(tempPassword));

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas przypominania hasła: {ex.Message}",
                    "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected virtual void OnLoginSuccessful(LoginEventArgs e)
        {
            LoginSuccessful?.Invoke(this, e);
        }

        //Hashowanie hasła przy logowaniu
        private string GeneratePBKDF2Hash(string password)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, 16, 10000))
            {
                byte[] salt = deriveBytes.Salt;
                byte[] key = deriveBytes.GetBytes(32);
                return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(key)}";
            }
        }

        private bool VerifyPBKDF2Hash(string password, string hash)
        {
            var parts = hash.Split(':');
            if (parts.Length != 2) return false;
            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] key = Convert.FromBase64String(parts[1]);
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] testKey = deriveBytes.GetBytes(32);
                return testKey.SequenceEqual(key);
            }
        }
    }

    public class LoginEventArgs : EventArgs
    {
        public string Username { get; }
        public bool IsAdmin { get; }

        public LoginEventArgs(string username, bool isAdmin)
        {
            Username = username;
            IsAdmin = isAdmin;
        }
    }

    // Klasa do przechowywania danych sesji użytkownika
    public static class UserSession
    {
        public static EscapeRoom.Models.User CurrentUser { get; set; }
        public static bool IsLoggedIn { get; set; } = false;

        public static void Logout()
        {
            CurrentUser = null;
            IsLoggedIn = false;
        }
    }
}