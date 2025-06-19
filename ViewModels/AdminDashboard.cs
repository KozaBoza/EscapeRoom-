using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq; 
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using EscapeRoom.Data;
using EscapeRoom.Helpers;
using EscapeRoom.Models;
using EscapeRoom.Services; 

namespace EscapeRoom.ViewModels
{
    public class AdminDashboardViewModel : BaseViewModel
    {
        private ObservableCollection<Room> _rooms;
        private ObservableCollection<Reservation> _recentReservations;
        private ObservableCollection<User> _recentUsers;
        private ObservableCollection<Message> _recentMessages; //wiadomosci

        private int _totalRooms;
        //private int _activeRooms; 
        private int _pendingReservations;
        private int _confirmedReservations;
        private int _totalUsers;
        private decimal _totalRevenue;
        private bool _hasPendingReservations; 

        private bool _isBusy;
        private string _statusMessage;

        private readonly DataService _dataService; 


        public AdminDashboardViewModel()
        {
            _dataService = new DataService(); 

            _rooms = new ObservableCollection<Room>();
            _recentReservations = new ObservableCollection<Reservation>();
            _recentUsers = new ObservableCollection<User>();
            _recentMessages = new ObservableCollection<Message>(); 

            LoadDataCommand = new RelayCommand(async _ => await LoadDataAsync(), _ => !IsBusy);
            ApprovePendingReservationsCommand = new RelayCommand(async _ => await ApprovePendingReservationsAsync(), _ => !IsBusy && HasPendingReservations); // Nowa komenda
        }

        public ObservableCollection<Room> Rooms
        {
            get => _rooms;
            set => SetProperty(ref _rooms, value);
        }

        public ObservableCollection<Reservation> RecentReservations
        {
            get => _recentReservations;
            set => SetProperty(ref _recentReservations, value);
        }

        public ObservableCollection<User> RecentUsers
        {
            get => _recentUsers;
            set => SetProperty(ref _recentUsers, value);
        }

        public ObservableCollection<Message> RecentMessages //dla wiadomosci
        {
            get => _recentMessages;
            set => SetProperty(ref _recentMessages, value);
        }

        public int TotalRooms
        {
            get => _totalRooms;
            set => SetProperty(ref _totalRooms, value);
        }

        public int PendingReservations
        {
            get => _pendingReservations;
            set
            {
                if (SetProperty(ref _pendingReservations, value))
                {
                    HasPendingReservations = value > 0; 
                }
            }
        }

        public int ConfirmedReservations
        {
            get => _confirmedReservations;
            set => SetProperty(ref _confirmedReservations, value);
        }

        public int TotalUsers
        {
            get => _totalUsers;
            set => SetProperty(ref _totalUsers, value);
        }

        public decimal TotalRevenue
        {
            get => _totalRevenue;
            set => SetProperty(ref _totalRevenue, value);
        }

        public bool HasPendingReservations //  do kontrolowania widoczności przycisku
        {
            get => _hasPendingReservations;
            set
            {
                if (SetProperty(ref _hasPendingReservations, value))
                {
                    // gdy zmienia się dostępność oczekujących rezerwacji
                    ((RelayCommand)ApprovePendingReservationsCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (SetProperty(ref _isBusy, value))
                {
                    LoadDataCommand.RaiseCanExecuteChanged();
                    ((RelayCommand)ApprovePendingReservationsCommand).RaiseCanExecuteChanged(); // Odśwież też komendę zatwierdzania
                }
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public RelayCommand LoadDataCommand { get; }
        public RelayCommand ApprovePendingReservationsCommand { get; } // Nowa komenda

        private async Task LoadDataAsync()
        {
            try
            {
                IsBusy = true;
                StatusMessage = "Ładowanie danych...";

                Rooms = new ObservableCollection<Room>(await _dataService.GetRoomsAsync());
                TotalRooms = Rooms.Count;

                RecentReservations = new ObservableCollection<Reservation>(await _dataService.GetRecentReservationsAsync(5));
                PendingReservations = await _dataService.GetPendingReservationsCountAsync();
                ConfirmedReservations = await _dataService.GetConfirmedReservationsCountAsync();

                RecentUsers = new ObservableCollection<User>(await _dataService.GetRecentUsersAsync(5));
                TotalUsers = await _dataService.GetTotalUsersAsync();
                TotalRevenue = await _dataService.GetTotalRevenueAsync();

                RecentMessages = new ObservableCollection<Message>(await _dataService.GetRecentMessagesAsync(5)); // Pobierz 5 najnowszych wiadomości

                StatusMessage = "Dane załadowane pomyślnie.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Błąd ładowania danych: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ApprovePendingReservationsAsync()
        {
            try
            {
                IsBusy = true;
                StatusMessage = "Zatwierdzanie oczekujących rezerwacji...";

                await _dataService.ApproveAllPendingReservationsAsync(); 
                await LoadDataAsync();
                StatusMessage = "Wszystkie oczekujące rezerwacje zostały zatwierdzone.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Błąd podczas zatwierdzania rezerwacji: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}