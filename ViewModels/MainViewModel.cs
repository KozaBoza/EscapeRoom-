using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EscapeRoom.ViewModels
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private object _currentView;
        private readonly HomeViewModel _homeViewModel;
        private readonly RoomListViewModel _roomListViewModel;
        private readonly LoginViewModel _loginViewModel;
        private readonly AccountViewModel _accountViewModel;
        private readonly ContactViewModel _contactViewModel;
        private readonly AdminDashboard _adminDashboard;
        private bool _isUserLoggedIn;
        private bool _isAdmin;
        private string _currentUsername;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsUserLoggedIn
        {
            get => _isUserLoggedIn;
            set
            {
                if (_isUserLoggedIn != value)
                {
                    _isUserLoggedIn = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsUserLoggedOut));
                }
            }
        }

        public bool IsUserLoggedOut => !IsUserLoggedIn;

        public bool IsAdmin
        {
            get => _isAdmin;
            set
            {
                if (_isAdmin != value)
                {
                    _isAdmin = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CurrentUsername
        {
            get => _currentUsername;
            set
            {
                if (_currentUsername != value)
                {
                    _currentUsername = value;
                    OnPropertyChanged();
                }
            }
        }

        //
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateRoomsCommand { get; }
        public ICommand NavigateLoginCommand { get; }
        public ICommand NavigateAccountCommand { get; }
        public ICommand NavigateContactCommand { get; }
        public ICommand NavigateAdminCommand { get; }
        public ICommand LogoutCommand { get; }

        public MainViewModel()
        {
            _homeViewModel = new HomeViewModel();
            _roomListViewModel = new RoomListViewModel();
            _loginViewModel = new LoginViewModel();
            _accountViewModel = new AccountViewModel();
            _contactViewModel = new ContactViewModel();
            _adminDashboard = new AdminDashboard();

            CurrentView = _homeViewModel;

            NavigateHomeCommand = new RelayCommand(param => NavigateHome());
            NavigateRoomsCommand = new RelayCommand(param => NavigateRooms());
            NavigateLoginCommand = new RelayCommand(param => NavigateLogin());
            NavigateAccountCommand = new RelayCommand(param => NavigateAccount(), param => IsUserLoggedIn);
            NavigateContactCommand = new RelayCommand(param => NavigateContact());
            NavigateAdminCommand = new RelayCommand(param => NavigateAdmin(), param => IsAdmin);
            LogoutCommand = new RelayCommand(param => Logout(), param => IsUserLoggedIn);

            _loginViewModel.LoginSuccessful += OnLoginSuccessful;
        }

        private void NavigateHome()
        {
            CurrentView = _homeViewModel;
        }

        private void NavigateRooms()
        {
            CurrentView = _roomListViewModel;
        }

        private void NavigateLogin()
        {
            CurrentView = _loginViewModel;
        }

        private void NavigateAccount()
        {
            CurrentView = _accountViewModel;
        }

        private void NavigateContact()
        {
            CurrentView = _contactViewModel;
        }

        private void NavigateAdmin()
        {
            CurrentView = _adminDashboard;
        }

        private void Logout()
        {
            IsUserLoggedIn = false;
            IsAdmin = false;
            CurrentUsername = string.Empty;
            CurrentView = _homeViewModel;
        }

        private void OnLoginSuccessful(object sender, LoginEventArgs e)
        {
            IsUserLoggedIn = true;
            IsAdmin = e.IsAdmin;
            CurrentUsername = e.Username;

            if (IsAdmin)
            {
                NavigateAdmin();
            }
            else
            {
                NavigateHome();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
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