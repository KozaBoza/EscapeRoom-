using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Collections.ObjectModel;
using EscapeRoom.Models;
using System.Threading.Tasks;
using EscapeRoom.Helpers;
using EscapeRoom.Services;
using System.Windows.Controls;


namespace EscapeRoom.ViewModels
{
    internal class LoginViewModel : INotifyPropertyChanged
    { //logowanie- sciagnelam z jakiego innego gith
        private string _username;
        private string _password;
        private string _errorMessage;
        private bool _isLoading;

        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanLogin));
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanLogin));
                }
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanLogin));
                }
            }
        }

        public bool CanLogin => !string.IsNullOrWhiteSpace(Username) &&
                                !string.IsNullOrWhiteSpace(Password) &&
                                !IsLoading;

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand ForgotPasswordCommand { get; }
        public event EventHandler<LoginEventArgs> LoginSuccessful;

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(ExecuteLogin, param => CanLogin);
            RegisterCommand = new RelayCommand(ExecuteRegister);
            ForgotPasswordCommand = new RelayCommand(ExecuteForgotPassword);
        }

        private async void ExecuteLogin(object parameter)
        {
            ErrorMessage = string.Empty;
            IsLoading = true;

            try
            {
                await Task.Delay(1000);
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
                    ErrorMessage = "Invalid username or password";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Login failed: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ExecuteRegister(object parameter)
        {
        }

        private void ExecuteForgotPassword(object parameter)
        {
        }

        protected virtual void OnLoginSuccessful(LoginEventArgs e)
        {
            LoginSuccessful?.Invoke(this, e);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}