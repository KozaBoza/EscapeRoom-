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

namespace EscapeRoom.ViewModels
{ //widok główny
    public class MainViewModel : BaseViewModel
    {
        private UserViewModel _currentUser;
        private RoomViewModel _selectedRoom;
        private ReservationViewModel _currentReservation;
        private string _currentView;
        private bool _isLoggedIn;

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

            CurrentView = "Rooms";
            LoadSampleData();
        }

        public ObservableCollection<RoomViewModel> Rooms { get; }
        public ObservableCollection<ReservationViewModel> Reservations { get; }
        public ObservableCollection<ReviewViewModel> Reviews { get; }
        public ObservableCollection<PaymentViewModel> Payments { get; }

        public UserViewModel CurrentUser
        {
            get => _currentUser;
            set
            {
                if (SetProperty(ref _currentUser, value))
                {
                    OnPropertyChanged(nameof(IsLoggedIn));
                    OnPropertyChanged(nameof(CurrentUserName));
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

        public bool IsLoggedIn
        {
            get => _isLoggedIn && CurrentUser != null;
            set => SetProperty(ref _isLoggedIn, value);
        }

        public string CurrentUserName => CurrentUser != null ?
            $"{CurrentUser.Imie} {CurrentUser.Nazwisko}" : "Gość";

        // komendy
        public ICommand LoginCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand NavigateCommand { get; }
        public ICommand RefreshDataCommand { get; }

        // metody komend
        private void Login(object parameter)
        {
            // logika logowania - tutaj powinna być integracja z bazą danych
            if (parameter is UserViewModel loginUser)
            {
                CurrentUser = loginUser;
                IsLoggedIn = true;
                LoadUserData();
            }
        }

        private bool CanLogin(object parameter) => !IsLoggedIn;

        private void Logout(object parameter)
        {
            CurrentUser = null;
            IsLoggedIn = false;
            CurrentView = "Rooms";
        }

        private bool CanLogout(object parameter) => IsLoggedIn;

        private bool _isLogoutVisible = true;
        public bool IsLogoutVisible
        {
            get => _isLogoutVisible;
            set
            {
                _isLogoutVisible = value;
                OnPropertyChanged(nameof(IsLogoutVisible));
            }
        }


        private void Register(object parameter)
        {
            //logika rejestracji nowego użytkownika
            if (parameter is UserViewModel newUser && newUser.IsValid)
            {
                //logika zapisu do bazy danych
                CurrentUser = newUser;
                IsLoggedIn = true;
            }
        }

        private bool CanRegister(object parameter) =>
            parameter is UserViewModel user && user.IsValid;

        private void Navigate(object parameter)
        {
            if (parameter is string viewName)
            {
                CurrentView = viewName;
            }
        }

        private void RefreshData(object parameter)
        {
            LoadSampleData();
            if (IsLoggedIn)
            {
                LoadUserData();
            }
        }

        // metody pomocnicze
        private void LoadSampleData()
        {
            //zaladowanie przykładowych pokoi
            Rooms.Clear();
            //var sampleRooms = new[]
            // {
            //przykaldy
            //};

            //foreach (var room in sampleRooms)
            {
            //    Rooms.Add(new RoomViewModel(room));
            }
        }

        private void LoadUserData()
        {
            if (CurrentUser == null) return;
            Reservations.Clear();
            Payments.Clear();
            Reviews.Clear();
            //dodac logike
        }

        //metody biznesowe
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
    }
}