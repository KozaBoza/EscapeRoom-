using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EscapeRoom.Helpers;
using EscapeRoom.Models;
using System.Windows;
using System.Windows.Controls; // Potrzebne do UserControl
using System.Windows.Media;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using EscapeRoom.Views; 

namespace EscapeRoom.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private UserViewModel _currentUser;
        private RoomViewModel _selectedRoom;
        private ReservationViewModel _currentReservation;
        private string _currentView;
        private bool _isLoggedIn;
        private string _currentUserName; // email
        private bool _isLogoutVisible;


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

            CurrentView = "Rooms"; // domyslny
            IsLoggedIn = false; // Na początku wylogowany
            IsLogoutVisible = true; //
            //Messenger.Instance.Subscribe<NavigationMessage>(OnNavigationMessageReceived);
        }

        public UserViewModel CurrentUser
        {
            get => _currentUser;
            set
            {
                if (SetProperty(ref _currentUser, value))
                {
                    
                    CurrentUserName = value?.Email; //email
                    IsLoggedIn = (value != null);
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
            get => _isLoggedIn;
            set
            {
                if (SetProperty(ref _isLoggedIn, value))
                {
                    OnPropertyChanged(nameof(CanLogout));
                    OnPropertyChanged(nameof(CanLogin));
                    OnPropertyChanged(nameof(CanRegister));
                    IsLogoutVisible = !value;
                }
            }
        }

        public ObservableCollection<RoomViewModel> Rooms { get; set; }
        public ObservableCollection<ReservationViewModel> Reservations { get; set; }
        public ObservableCollection<ReviewViewModel> Reviews { get; set; }
        public ObservableCollection<PaymentViewModel> Payments { get; set; }

        // Komendy
        public ICommand LoginCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand RegisterCommand { get; } // Pozostaje tutaj, jeśli używasz go w MainView.xaml
        public ICommand NavigateCommand { get; }
        public ICommand RefreshDataCommand { get; }



        public string CurrentUserName
        {
            get => _currentUserName;
            set => SetProperty(ref _currentUserName, value);
        }

        private async void Login(object parameter)
        {
            // Przejście do widoku logowania, jeśli nie jesteś zalogowany
            if (!IsLoggedIn)
            {
                CurrentView = "Login"; // Ustawia CurrentView na "Login"
            }
        }

        private bool CanLogin(object parameter) => !IsLoggedIn;

        public bool IsLogoutVisible // Ta właściwość kontroluje widoczność zarówno przycisków logowania, jak i wylogowania
        {
            get => _isLogoutVisible;
            set => SetProperty(ref _isLogoutVisible, value);
        }

        private void Logout(object parameter)
        {
            CurrentUser = null;
            IsLoggedIn = false;
            ClearSessionData();
            CurrentView = "Login"; // Przejście do widoku logowania po wylogowaniu
        }

        private bool CanLogout(object parameter) => IsLoggedIn;

        private void Register(object parameter)
        {
            // przejscie do widoku rejestracji
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

        //private void OnNavigationMessageReceived(NavigationMessage message)
        //{
        //    if (message?.TargetView != null)
        //    {
        //        CurrentView = message.TargetView;
        //    }
        //}

        private void RefreshData(object parameter)
        {
            
            if (CurrentUser == null) return;
            Reservations.Clear();
            Payments.Clear();
            Reviews.Clear();
           
        }

        //metody 
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
            // 
        }
      
    }
}

