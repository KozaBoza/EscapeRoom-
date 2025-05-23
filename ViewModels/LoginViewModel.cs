using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using EscapeRoom.Helpers;

namespace EscapeRoom.ViewModels
{
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
                await Task.Delay(1000); // symulacja autoryzacji

                if (Username == "admin" && Password == "admin")
                {
                    OnLoginSuccessful(new LoginEventArgs(Username, true));
                }
                else if (Username == "user" && Password == "user")
                {
                    OnLoginSuccessful(new LoginEventArgs(Username, false));
                }
                else
                {
                    ErrorMessage = "Nieprawidłowa nazwa użytkownika lub hasło.";
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
            // Możesz tutaj przekierować do widoku rejestracji
        }

        private void ExecuteForgotPassword(object parameter)
        {
            //haslo przypomnienie
        }

        protected virtual void OnLoginSuccessful(LoginEventArgs e)
        {
            LoginSuccessful?.Invoke(this, e);
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
