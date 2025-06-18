using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using EscapeRoom.Data;
using EscapeRoom.Helpers;
using EscapeRoom.Models;
//using EscapeRoom.Models.Enums;
using EscapeRoom.Services;

namespace EscapeRoom.ViewModels
{
    public class AdminDashboardViewModel : BaseViewModel
    {
        private ObservableCollection<Room> _rooms;
        private ObservableCollection<Reservation> _recentReservations;
        private ObservableCollection<User> _recentUsers;

        private int _totalRooms;
        private int _activeRooms;
        private int _pendingReservations;
        private int _confirmedReservations;
        private int _totalUsers;
        private decimal _totalRevenue;

        private bool _isBusy;
        private string _statusMessage;

        private readonly DataService _dataService = new DataService();


        public AdminDashboardViewModel()
        {
            _rooms = new ObservableCollection<Room>();
            _recentReservations = new ObservableCollection<Reservation>();
            _recentUsers = new ObservableCollection<User>();

            LoadDataCommand = new RelayCommand(async _ => await LoadDataAsync(), _ => !IsBusy);
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

        public int TotalRooms
        {
            get => _totalRooms;
            set => SetProperty(ref _totalRooms, value);
        }

        public int ActiveRooms
        {
            get => _activeRooms;
            set => SetProperty(ref _activeRooms, value);
        }

        public int PendingReservations
        {
            get => _pendingReservations;
            set => SetProperty(ref _pendingReservations, value);
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

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (SetProperty(ref _isBusy, value))
                    LoadDataCommand.RaiseCanExecuteChanged();
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public RelayCommand LoadDataCommand { get; }

        private async Task LoadDataAsync()
        {
            try //symulator ładowania -- informacje dot. liczby pokoi etc.
            {
                IsBusy = true;
                StatusMessage = "Ładowanie danych...";
                await Task.Delay(300); // opcjonalne opóźnienie wizualne

                Rooms = new ObservableCollection<Room>(await _dataService.GetRoomsAsync());
                TotalRooms = Rooms.Count;
                ActiveRooms = await _dataService.GetActiveRoomsCountAsync();

                RecentReservations = new ObservableCollection<Reservation>(await _dataService.GetRecentReservationsAsync(5));
                PendingReservations = await _dataService.GetPendingReservationsCountAsync();
                ConfirmedReservations = await _dataService.GetConfirmedReservationsCountAsync();

                RecentUsers = new ObservableCollection<User>(await _dataService.GetRecentUsersAsync(5));
                TotalUsers = await _dataService.GetTotalUsersAsync();
                TotalRevenue = await _dataService.GetTotalRevenueAsync();

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
    }
}
