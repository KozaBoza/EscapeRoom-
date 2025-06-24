using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using EscapeRoom.Data;
using EscapeRoom.Helpers;
using EscapeRoom.Models;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using EscapeRoom.Services;

namespace EscapeRoom.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private string _telefon;
        private string _email;
        private string _password;
        private string _confirmPassword;
        private string _firstName;
        private string _lastName;
        private string _errorMessage;
        private bool _isLoading;

        public string Telefon
        {
            get => _telefon;
            set
            {
                if (SetProperty(ref _telefon, value))
                    OnPropertyChanged(nameof(CanRegister));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (SetProperty(ref _email, value))
                    OnPropertyChanged(nameof(CanRegister));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (SetProperty(ref _password, value))
                    OnPropertyChanged(nameof(CanRegister));
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (SetProperty(ref _confirmPassword, value))
                    OnPropertyChanged(nameof(CanRegister));
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (SetProperty(ref _firstName, value))
                    OnPropertyChanged(nameof(CanRegister));
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (SetProperty(ref _lastName, value))
                    OnPropertyChanged(nameof(CanRegister));
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
                    OnPropertyChanged(nameof(CanRegister));
            }
        }

        public ICommand RegisterCommand { get; }
        public ICommand GoToLoginCommand { get; }

        public RegisterViewModel()
        {
            // inicjalizacja pól, aby uniknąć null
            Telefon = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            ErrorMessage = string.Empty;


            RegisterCommand = new RelayCommand(async (param) => await Register(), (param) => CanRegister);
            GoToLoginCommand = new RelayCommand(GoToLogin);
        }

        private async Task Register()
        {
            ErrorMessage = string.Empty;
            IsLoading = true;

            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Hasła nie są zgodne";
                IsLoading = false;
                return;
            }

            if (!IsValidEmail(Email))
            {
                ErrorMessage = "Niepoprawny format";
                IsLoading = false;
                return;
            }

            //sprawdzanie
            var dataService = new DataService();
            var existingUserByUsername = await dataService.GetUserByEmailAsync(Email);
            if (existingUserByUsername != null)
            {
                ErrorMessage = "Użytkownik o tej nazwie już istnieje.";
                IsLoading = false;
                return;
            }

            var existingUserByEmail = await dataService.GetUserByEmailAsync(Email);
            if (existingUserByEmail != null)
            {
                ErrorMessage = "Użytkownik o tym adresie e-mail już istnieje.";
                IsLoading = false;
                return;
            }

            try
            {
               var newUser = new User
                {
                    Email = Email,
                    Imie = FirstName,
                    Nazwisko = LastName,
                    Telefon = Telefon,
                   HasloHash = GeneratePBKDF2Hash(Password),
                    DataRejestracji = DateTime.Now,
                    Admin = false //domyslnie nie
                };

                var success = await dataService.AddUserAsync(newUser);

                if (success)
                {
                    //informacja o pomyślnym utworzeniu konta
                    System.Windows.MessageBox.Show(
                        "Konto zostało pomyślnie utworzone. Możesz teraz się zalogować.",
                        "Rejestracja zakończona",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Information);

                    //przekierowanie do widoku logowania
                    ViewNavigationService.Instance.NavigateTo(ViewType.Login);
                }
                else
                {
                    ErrorMessage = "Rejestracja nie powiodła się. Spróbuj ponownie.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Wystąpił błąd podczas rejestracji: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            } 

        }


        private bool CanRegister =>
            !string.IsNullOrWhiteSpace(Telefon) &&
            !string.IsNullOrWhiteSpace(Email) &&
            !string.IsNullOrWhiteSpace(Password) &&
            !string.IsNullOrWhiteSpace(ConfirmPassword) &&
            !string.IsNullOrWhiteSpace(FirstName) &&
            !string.IsNullOrWhiteSpace(LastName) &&
            Password == ConfirmPassword &&
            IsValidEmail(Email) &&
            !IsLoading;

        private void GoToLogin(object parameter)
        {
            // Przekierowanie do widoku logowania bez wyświetlania komunikatu
            ViewNavigationService.Instance.NavigateTo(ViewType.Login);
        }

        private string GeneratePBKDF2Hash(string password)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, 16, 10000))
            {
                byte[] salt = deriveBytes.Salt;
                byte[] key = deriveBytes.GetBytes(32);
                return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(key)}";
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
    }
}