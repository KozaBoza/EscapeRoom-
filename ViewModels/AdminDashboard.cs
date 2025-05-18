using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using EscapeRoom.Helpers;
using EscapeRoom.Models;

namespace EscapeRoom.ViewModels
{
    internal class AdminDashboard : INotifyPropertyChanged
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


        public AdminDashboard()
        {
            // Initialize collections
            Rooms = new ObservableCollection<Room>();
            RecentReservations = new ObservableCollection<Reservation>();
            RecentUsers = new ObservableCollection<User>();

        }

        public ObservableCollection<Room> Rooms
        {
            get => _rooms;
            set
            {
                if (_rooms != value)
                {
                    _rooms = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Reservation> RecentReservations
        {
            get => _recentReservations;
            set
            {
                if (_recentReservations != value)
                {
                    _recentReservations = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<User> RecentUsers
        {
            get => _recentUsers;
            set
            {
                if (_recentUsers != value)
                {
                    _recentUsers = value;
                    OnPropertyChanged();
                }
            }
        }

        public int TotalRooms
        {
            get => _totalRooms;
            set
            {
                if (_totalRooms != value)
                {
                    _totalRooms = value;
                    OnPropertyChanged();
                }
            }
        }

        public int ActiveRooms
        {
            get => _activeRooms;
            set
            {
                if (_activeRooms != value)
                {
                    _activeRooms = value;
                    OnPropertyChanged();
                }
            }
        }

        public int PendingReservations
        {
            get => _pendingReservations;
            set
            {
                if (_pendingReservations != value)
                {
                    _pendingReservations = value;
                    OnPropertyChanged();
                }
            }
        }

        public int ConfirmedReservations
        {
            get => _confirmedReservations;
            set
            {
                if (_confirmedReservations != value)
                {
                    _confirmedReservations = value;
                    OnPropertyChanged();
                }
            }
        }

        public int TotalUsers
        {
            get => _totalUsers;
            set
            {
                if (_totalUsers != value)
                {
                    _totalUsers = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal TotalRevenue
        {
            get => _totalRevenue;
            set
            {
                if (_totalRevenue != value)
                {
                    _totalRevenue = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                }
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage != value)
                {
                    _statusMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        private ICommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                return _refreshCommand ?? (_refreshCommand = new RelayCommand(_ => LoadDashboardDataAsync(), _ => !IsBusy));
            }
        }

        private ICommand _navigateToRoomsCommand;
        public ICommand NavigateToRoomsCommand
        {
            get
            {
                return _navigateToRoomsCommand ?? (_navigateToRoomsCommand = new RelayCommand(_ => NavigateToRooms(), _ => !IsBusy));
            }
        }

        private ICommand _navigateToReservationsCommand;
        public ICommand NavigateToReservationsCommand
        {
            get
            {
                return _navigateToReservationsCommand ?? (_navigateToReservationsCommand = new RelayCommand(_ => NavigateToReservations(), _ => !IsBusy));
            }
        }

        private ICommand _navigateToUsersCommand;
        public ICommand NavigateToUsersCommand
        {
            get
            {
                return _navigateToUsersCommand ?? (_navigateToUsersCommand = new RelayCommand(_ => NavigateToUsers(), _ => !IsBusy));
            }
        }

        private ICommand _navigateToPaymentsCommand;
        public ICommand NavigateToPaymentsCommand
        {
            get
            {
                return _navigateToPaymentsCommand ?? (_navigateToPaymentsCommand = new RelayCommand(_ => NavigateToPayments(), _ => !IsBusy));
            }
        }

        private async void LoadDashboardDataAsync()
        {
            try
            {
                IsBusy = true;
                StatusMessage = "Loading dashboard data...";
                StatusMessage = "Dashboard data loaded successfully.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading dashboard data: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void NavigateToRooms()
        {
            StatusMessage = "Navigating to Rooms...";
        }

        private void NavigateToReservations()
        {
            StatusMessage = "Navigating to Reservations...";
 }

        private void NavigateToUsers()
        {
            StatusMessage = "Navigating to Users...";
        }

        private void NavigateToPayments()
        {
            StatusMessage = "Navigating to Payments...";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}