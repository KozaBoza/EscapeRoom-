using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
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
            try
            {
                IsBusy = true;
                StatusMessage = "Ładowanie danych...";

                // Symulacja opóźnienia ładowania danych
                await Task.Delay(1500);

                // Tutaj powinna być logika pobierania danych z serwisów/bazy
                // Poniżej przykładowe dane symulacyjne:

                Rooms = new ObservableCollection<Room>
                {
                    new Room { PokojId = 1, Nazwa = "Pokój Tajemnic", MaxGraczy = 6 },
                    new Room { PokojId = 2, Nazwa = "Pokój Detektywa", MaxGraczy = 4 }
                };
                TotalRooms = Rooms.Count;
                ActiveRooms = 2;

                RecentReservations = new ObservableCollection<Reservation>
                {
                    new Reservation { RezerwacjaId = 101, Status = ReservationStatus.zarezerwowana, LiczbaOsob = 4 },
                    new Reservation { RezerwacjaId = 102, Status = ReservationStatus.zrealizowana, LiczbaOsob = 5 }
                };
                PendingReservations = 1;
                ConfirmedReservations = 1;

                RecentUsers = new ObservableCollection<User>
                {
                    new User { UzytkownikId = 1, Imie = "Jan", Nazwisko = "Kowalski" },
                    new User { UzytkownikId = 2, Imie = "Anna", Nazwisko = "Nowak" }
                };
                TotalUsers = 50;

                TotalRevenue = 12345.67m;

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
