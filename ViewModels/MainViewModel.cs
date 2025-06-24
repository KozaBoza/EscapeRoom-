using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EscapeRoom.Helpers;
using EscapeRoom.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using EscapeRoom.Views;

namespace EscapeRoom.ViewModels
{
    public class MainViewModel : BaseViewModel, IDisposable
    {
        private UserViewModel _currentUser;
        private RoomViewModel _selectedRoom;
        private ReservationViewModel _currentReservation;
        private string _currentView;
        private string _currentUserName;

        public MainViewModel()
        {
            Rooms = new ObservableCollection<RoomViewModel>();
            Reservations = new ObservableCollection<ReservationViewModel>();
            Reviews = new ObservableCollection<ReviewViewModel>();
            Payments = new ObservableCollection<PaymentViewModel>();

            LoginCommand = new RelayCommand(Login, CanLogin);
            LogoutCommand = new RelayCommand(Logout, CanLogout);
            RegisterCommand = new RelayCommand(Register, CanRegister);
            NavigateCommand = new RelayCommand(Navigate);
            RefreshDataCommand = new RelayCommand(RefreshData);

            CurrentView = "Rooms"; // default view

            // Subscribe to UserSession changes
            UserSession.UserSessionChanged += UserSession_UserSessionChanged;
        }

        private void UserSession_UserSessionChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(IsLoggedIn));
            OnPropertyChanged(nameof(IsLogoutVisible));
            OnPropertyChanged(nameof(IsAdmin));
        }

        public bool IsLoggedIn => UserSession.IsLoggedIn;
        public bool IsLogoutVisible => !IsLoggedIn;
        public bool IsAdmin => UserSession.IsAdmin;

        public UserViewModel CurrentUser
        {
            get => _currentUser;
            set
            {
                if (SetProperty(ref _currentUser, value))
                {
                    CurrentUserName = value?.Email;
                    OnPropertyChanged(nameof(IsLoggedIn));
                }
            }
        }

        public RoomViewModel SelectedRoom
        {
            get => _selectedRoom;
            set => SetProperty(ref _selectedRoom, value);
        }

        public ReservationViewModel CurrentReservation
        {
            get => _currentReservation;
            set => SetProperty(ref _currentReservation, value);
        }

        public string CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public ObservableCollection<RoomViewModel> Rooms { get; set; }
        public ObservableCollection<ReservationViewModel> Reservations { get; set; }
        public ObservableCollection<ReviewViewModel> Reviews { get; set; }
        public ObservableCollection<PaymentViewModel> Payments { get; set; }

        public ICommand LoginCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand NavigateCommand { get; }
        public ICommand RefreshDataCommand { get; }

        public string CurrentUserName
        {
            get => _currentUserName;
            set => SetProperty(ref _currentUserName, value);
        }

        private void Login(object parameter)
        {
            if (!IsLoggedIn)
            {
                CurrentView = "Login";
            }
        }

        private bool CanLogin(object parameter) => !IsLoggedIn;

        private void Logout(object parameter)
        {
            UserSession.Logout();
            CurrentUser = null;
            ClearSessionData();
            CurrentView = "Login";
        }

        private bool CanLogout(object parameter) => IsLoggedIn;

        private void Register(object parameter)
        {
            if (!IsLoggedIn)
            {
                CurrentView = "Register";
            }
        }

        private bool CanRegister(object parameter) => !IsLoggedIn;

        private void Navigate(object parameter)
        {
            if (parameter is string viewName)
            {
                CurrentView = viewName;
            }
        }

        private void RefreshData(object parameter)
        {
            if (CurrentUser == null) return;
            Reservations.Clear();
            Payments.Clear();
            Reviews.Clear();
        }

        public void CreateReservation(RoomViewModel room, DateTime startTime, byte numberOfPeople)
        {
            if (!IsLoggedIn || room == null) return;

            var reservation = new Reservation
            {
                UzytkownikId = CurrentUser.UzytkownikId,
                PokojId = room.PokojId,
                DataRozpoczecia = startTime,
                LiczbaOsob = numberOfPeople,
                Status = ReservationStatus.zarezerwowana,
                DataUtworzenia = DateTime.Now
            };

            CurrentReservation = new ReservationViewModel(reservation)
            {
                UserViewModel = CurrentUser,
                RoomViewModel = room
            };

            Reservations.Add(CurrentReservation);
        }

        public void ProcessPayment(ReservationViewModel reservation, PaymentMethod method, decimal amount)
        {
            if (reservation == null) return;

            var payment = new PaymentViewModel
            {
                ReservationId = reservation.RezerwacjaId,
                Amount = amount,
                Method = method,
                Status = PaymentStatus.Pending,
                PaymentDate = DateTime.Now
            };

            Payments.Add(payment);
        }

        private void ClearSessionData()
        {
            Reservations.Clear();
            Payments.Clear();
            Reviews.Clear();
        }

        public void Dispose()
        {
            UserSession.UserSessionChanged -= UserSession_UserSessionChanged;
        }

        public void OnLoginSuccess(User user)
        {
            UserSession.Login(user);
            CurrentUser = new UserViewModel(user);
            CurrentView = "Rooms";
        }
    }
}