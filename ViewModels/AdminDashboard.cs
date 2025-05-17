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

        // Services would be injected here
        // private readonly RoomService _roomService;
        // private readonly ReservationService _reservationService;
        // private readonly UserService _userService;
        // private readonly PaymentService _paymentService;

        public AdminDashboard()
        {
            // Initialize collections
            Rooms = new ObservableCollection<Room>();
            RecentReservations = new ObservableCollection<Reservation>();
            RecentUsers = new ObservableCollection<User>();

            // Initialize services
            // _roomService = new RoomService(connectionString);
            // _reservationService = new ReservationService(connectionString);
            // _userService = new UserService(connectionString);
            // _paymentService = new PaymentService(connectionString);

            // Load dashboard data
            // LoadDashboardDataAsync();
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

        // Commands
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

                // Load rooms data
                // var allRooms = await _roomService.GetAllRoomsAsync(includeInactive: true);
                // Rooms = new ObservableCollection<Room>(allRooms);
                // TotalRooms = allRooms.Count;
                // ActiveRooms = allRooms.Count(r => r.IsActive);

                // Load reservations data
                // var reservationsStatus = await _reservationService.GetReservationStatusCountsAsync();
                // PendingReservations = reservationsStatus.PendingCount;
                // ConfirmedReservations = reservationsStatus.ConfirmedCount;
                // var recent = await _reservationService.GetRecentReservationsAsync(5);
                // RecentReservations = new ObservableCollection<Reservation>(recent);

                // Load users data
                // var users = await _userService.GetUsersCountAsync();
                // TotalUsers = users;
                // var recentUsers = await _userService.GetRecentUsersAsync(5);
                // RecentUsers = new ObservableCollection<User>(recentUsers);

                // Load revenue data
                // TotalRevenue = await _paymentService.GetTotalRevenueAsync();

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
            // Navigation logic here
            // Could use a navigation service or event aggregator
            StatusMessage = "Navigating to Rooms...";
            // NavigationService.Navigate(new Uri("/Views/AdminRoomsView.xaml", UriKind.Relative));
        }

        private void NavigateToReservations()
        {
            // Navigation logic here
            StatusMessage = "Navigating to Reservations...";
            // NavigationService.Navigate(new Uri("/Views/AdminReservationsView.xaml", UriKind.Relative));
        }

        private void NavigateToUsers()
        {
            // Navigation logic here
            StatusMessage = "Navigating to Users...";
            // NavigationService.Navigate(new Uri("/Views/AdminUsersView.xaml", UriKind.Relative));
        }

        private void NavigateToPayments()
        {
            // Navigation logic here
            StatusMessage = "Navigating to Payments...";
            // NavigationService.Navigate(new Uri("/Views/AdminPaymentsView.xaml", UriKind.Relative));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Note: The RelayCommand class should be in a separate file to avoid duplication across ViewModels
}