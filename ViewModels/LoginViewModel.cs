using EscapeRoom.Data;
using EscapeRoom.Helpers;
using EscapeRoom.Services;
using EscapeRoom.Views;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using EscapeRoom.Models;

namespace EscapeRoom.ViewModels
{
    // Klasa do przechowywania danych sesji użytkownika
    // Przeniesiona na poziom namespace'u
    public static class UserSession
    {
        private static bool _isLoggedIn;
        private static User _currentUser;
        private static bool _isAdmin;

        public static event EventHandler UserSessionChanged;

        public static User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                IsAdmin = value?.Admin ?? false; // Update admin status when user changes
                OnUserSessionChanged();
            }
        }

        public static bool IsLoggedIn
        {
            get => _isLoggedIn;
            set
            {
                if (_isLoggedIn != value)
                {
                    _isLoggedIn = value;
                    if (!value) IsAdmin = false; // Reset admin status on logout
                    OnUserSessionChanged();
                }
            }
        }

        public static bool IsAdmin
        {
            get => _isAdmin;
            private set
            {
                if (_isAdmin != value)
                {
                    _isAdmin = value;
                    OnUserSessionChanged();
                }
            }
        }

        private static void OnUserSessionChanged()
        {
            UserSessionChanged?.Invoke(null, EventArgs.Empty);
        }

        public static void Logout()
        {
            CurrentUser = null;
            IsLoggedIn = false;
            IsAdmin = false;
            OnUserSessionChanged();
        }

        public static void Login(User user)
        {
            CurrentUser = user;
            IsLoggedIn = true;
            IsAdmin = user?.Admin ?? false;
            OnUserSessionChanged();
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

    //obsługa logowania
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
            !string.IsNullOrWhiteSpace(Password) &&
            !IsLoading;

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
            ViewNavigationService.Instance.NavigateTo(ViewType.Register);
        }

        private async void ExecuteForgotPassword(object parameter)
        {
            try
            {
                // Okno dialogowe do wpisania numeru telefonu
                var phoneDialog = new Window
                {
                    Title = "Przypomnienie hasła",
                    Width = 300,
                    Height = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    ResizeMode = ResizeMode.NoResize,
                    Topmost = true,
                    WindowStyle = WindowStyle.ToolWindow,
                    ShowInTaskbar = false
                };

                var phonePanel = new StackPanel { Margin = new Thickness(10) };
                phonePanel.Children.Add(new TextBlock
                {
                    Text = "Podaj swój numer telefonu:",
                    Margin = new Thickness(0, 0, 0, 10)
                });

                var phoneTextBox = new TextBox { Margin = new Thickness(0, 0, 0, 10) };
                phonePanel.Children.Add(phoneTextBox);

                var phoneButtonPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Right
                };

                var phoneOkButton = new Button
                {
                    Content = "OK",
                    Width = 80,
                    Margin = new Thickness(0, 0, 10, 0),
                    IsDefault = true
                };
                phoneOkButton.Click += (s, e) => { phoneDialog.DialogResult = true; };

                var phoneCancelButton = new Button
                {
                    Content = "Anuluj",
                    Width = 80,
                    IsCancel = true
                };
                phoneCancelButton.Click += (s, e) => { phoneDialog.DialogResult = false; };

                phoneButtonPanel.Children.Add(phoneOkButton);
                phoneButtonPanel.Children.Add(phoneCancelButton);
                phonePanel.Children.Add(phoneButtonPanel);

                phoneDialog.Content = phonePanel;

                bool? phoneResult = phoneDialog.ShowDialog();

                if (phoneResult == true && !string.IsNullOrWhiteSpace(phoneTextBox.Text))
                {
                    string phoneNumber = phoneTextBox.Text;

                    DataService service = new DataService();
                    var user = await service.GetUserByPhoneAsync(phoneNumber);

                    if (user == null)
                    {
                        MessageBox.Show("Nie znaleziono użytkownika z podanym numerem telefonu.",
                            "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Znaleziono użytkownika, wyświetl okno do zmiany hasła
                    var passwordDialog = new Window
                    {
                        Title = "Zmiana hasła",
                        Width = 350,
                        Height = 200,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        ResizeMode = ResizeMode.NoResize,
                        Topmost = true,
                        WindowStyle = WindowStyle.ToolWindow,
                        ShowInTaskbar = false
                    };

                    var passwordPanel = new StackPanel { Margin = new Thickness(10) };
                    passwordPanel.Children.Add(new TextBlock
                    {
                        Text = $"Użytkownik: {user.Imie} {user.Nazwisko}",
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 0, 0, 10)
                    });

                    passwordPanel.Children.Add(new TextBlock
                    {
                        Text = "Nowe hasło:",
                        Margin = new Thickness(0, 0, 0, 5)
                    });

                    var newPasswordBox = new PasswordBox { Margin = new Thickness(0, 0, 0, 10) };
                    passwordPanel.Children.Add(newPasswordBox);

                    passwordPanel.Children.Add(new TextBlock
                    {
                        Text = "Potwierdź nowe hasło:",
                        Margin = new Thickness(0, 0, 0, 5)
                    });

                    var confirmPasswordBox = new PasswordBox { Margin = new Thickness(0, 0, 0, 10) };
                    passwordPanel.Children.Add(confirmPasswordBox);

                    var passwordErrorText = new TextBlock
                    {
                        Foreground = Brushes.Red,
                        Margin = new Thickness(0, 0, 0, 10),
                        Visibility = Visibility.Collapsed
                    };
                    passwordPanel.Children.Add(passwordErrorText);

                    var passwordButtonPanel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Right
                    };

                    var passwordOkButton = new Button
                    {
                        Content = "Zmień hasło",
                        Width = 100,
                        Margin = new Thickness(0, 0, 10, 0),
                        IsDefault = true
                    };

                    passwordOkButton.Click += (s, e) =>
                    {
                        if (string.IsNullOrWhiteSpace(newPasswordBox.Password))
                        {
                            passwordErrorText.Text = "Hasło nie może być puste.";
                            passwordErrorText.Visibility = Visibility.Visible;
                            return;
                        }

                        if (newPasswordBox.Password != confirmPasswordBox.Password)
                        {
                            passwordErrorText.Text = "Hasła nie są zgodne.";
                            passwordErrorText.Visibility = Visibility.Visible;
                            return;
                        }

                        passwordDialog.DialogResult = true;
                    };

                    var passwordCancelButton = new Button
                    {
                        Content = "Anuluj",
                        Width = 80,
                        IsCancel = true
                    };
                    passwordCancelButton.Click += (s, e) => { passwordDialog.DialogResult = false; };

                    passwordButtonPanel.Children.Add(passwordOkButton);
                    passwordButtonPanel.Children.Add(passwordCancelButton);
                    passwordPanel.Children.Add(passwordButtonPanel);

                    passwordDialog.Content = passwordPanel;

                    bool? passwordResult = passwordDialog.ShowDialog();

                    if (passwordResult == true)
                    {
                        // Aktualizuj hasło użytkownika w bazie danych
                        bool updated = await UpdateUserPasswordAsync(service, user.UzytkownikId, newPasswordBox.Password);

                        if (updated)
                        {
                            MessageBox.Show("Hasło zostało pomyślnie zmienione. Możesz teraz zalogować się używając nowego hasła.",
                                "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Wystąpił błąd podczas zmiany hasła. Spróbuj ponownie później.",
                                "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas zmiany hasła: {ex.Message}",
                    "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Metoda do aktualizacji hasła użytkownika w bazie danych
        private async Task<bool> UpdateUserPasswordAsync(DataService service, int userId, string newPassword)
        {
            try
            {
                string hashedPassword = GeneratePBKDF2Hash(newPassword);

                using (MySqlConnection conn = new MySqlConnection(service.GetConnectionString()))
                {
                    await conn.OpenAsync();
                    MySqlCommand cmd = new MySqlCommand(
                        "UPDATE uzytkownicy SET haslo_hash = @haslo_hash WHERE uzytkownik_id = @uzytkownik_id", conn);
                    cmd.Parameters.AddWithValue("@haslo_hash", hashedPassword);
                    cmd.Parameters.AddWithValue("@uzytkownik_id", userId);

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas aktualizacji hasła: {ex.Message}",
                    "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
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

    }
}