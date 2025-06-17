using EscapeRoom.Data;
using EscapeRoom.Helpers;
using EscapeRoom.Models;
using EscapeRoom.Services;
using System;
using System.Windows;
using System.Windows.Input;

namespace EscapeRoom.ViewModels
{
    public class ReservationViewModel : BaseViewModel
    {
        private Reservation _reservation;
        private UserViewModel _userViewModel;
        private RoomViewModel _roomViewModel;
        private bool _isRoomAvailable = true;
        private string _roomAvailabilityMessage = string.Empty;
        private readonly DataService _dataService;

        public ReservationViewModel()
        {
            _dataService = new DataService();
            _reservation = new Reservation();
            _reservation.DataRozpoczecia = DateTime.Today;
            ConfirmReservationCommand = new RelayCommand(ConfirmReservation, CanConfirmReservation);
            CancelReservationCommand = new RelayCommand(CancelReservation, CanCancelReservation);
        }

        public ReservationViewModel(Reservation reservation) : this()
        {
            _reservation = reservation ?? new Reservation();
            // Jeśli data nie jest ustawiona, ustaw na dzisiaj
            if (_reservation.DataRozpoczecia == default(DateTime))
            {
                _reservation.DataRozpoczecia = DateTime.Today;
            }
            LoadAssociatedDataAsync();
        }

        public ReservationViewModel(RoomViewModel roomViewModel) : this()
        {
            RoomViewModel = roomViewModel; // Ustawienie właściwości RoomViewModel
            // Ustaw PokojId z wybranego pokoju
            _reservation.PokojId = roomViewModel.PokojId;
            // Tutaj możesz też zainicjować inne dane rezerwacji
            _reservation.DataUtworzenia = DateTime.Now; // Ustaw domyślną datę utworzenia
            _reservation.DataRozpoczecia = DateTime.Today; // Ustaw domyślną datę rezerwacji na dzisiaj
            Status = ReservationStatus.zarezerwowana; // Domyślny status po utworzeniu

            // Sprawdź dostępność pokoju na dzisiaj
            CheckRoomAvailabilityAsync();
        }

        private async void LoadAssociatedDataAsync()
        {
            DataService service = new DataService();
            if (_reservation.PokojId > 0)
            {
                // Pobierz pokój na podstawie PokojId rezerwacji
                var room = await service.GetRoomByIdAsync(_reservation.PokojId);
                if (room != null)
                {
                    RoomViewModel = new RoomViewModel(room); // Utwórz RoomViewModel z załadowanego pokoju
                }
            }
            // TODO: Analogicznie załaduj UserViewModel jeśli _reservation.UzytkownikId > 0
        }

        public int RezerwacjaId
        {
            get => _reservation.RezerwacjaId;
            set
            {
                if (_reservation.RezerwacjaId != value)
                {
                    _reservation.RezerwacjaId = value;
                    OnPropertyChanged();
                }
            }
        }

        public int UzytkownikId
        {
            get => _reservation.UzytkownikId;
            set
            {
                if (_reservation.UzytkownikId != value)
                {
                    _reservation.UzytkownikId = value;
                    OnPropertyChanged();
                }
            }
        }

        public int PokojId
        {
            get => _reservation.PokojId;
            set
            {
                if (_reservation.PokojId != value)
                {
                    _reservation.PokojId = value;
                    OnPropertyChanged();
                }
            }
        }

        public byte LiczbaOsob
        {
            get => _reservation.LiczbaOsob;
            set
            {
                if (_reservation.LiczbaOsob != value)
                {
                    _reservation.LiczbaOsob = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        public ReservationStatus Status
        {
            get => _reservation.Status;
            set
            {
                if (_reservation.Status != value)
                {
                    _reservation.Status = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(StatusText));
                    OnPropertyChanged(nameof(CanBeCancelled));
                }
            }
        }

        public DateTime DataRozpoczecia
        {
            get => _reservation.DataRozpoczecia;
            set
            {
                if (_reservation.DataRozpoczecia != value)
                {
                    _reservation.DataRozpoczecia = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsValid));

                    // Sprawdź dostępność pokoju po zmianie daty
                    CheckRoomAvailabilityAsync();
                }
            }
        }

        public DateTime DataUtworzenia
        {
            get => _reservation.DataUtworzenia;
            set
            {
                if (_reservation.DataUtworzenia != value)
                {
                    _reservation.DataUtworzenia = value;
                    OnPropertyChanged();
                }
            }
        }

        public UserViewModel UserViewModel
        {
            get => _userViewModel;
            set => SetProperty(ref _userViewModel, value);
        }

        public RoomViewModel RoomViewModel
        {
            get => _roomViewModel;
            set
            {
                if (SetProperty(ref _roomViewModel, value))
                {
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        public bool IsRoomAvailable
        {
            get => _isRoomAvailable;
            set
            {
                if (SetProperty(ref _isRoomAvailable, value))
                {
                    OnPropertyChanged(nameof(IsValid));
                    OnPropertyChanged(nameof(RoomAvailabilityMessage));
                }
            }
        }

        public string RoomAvailabilityMessage
        {
            get => _roomAvailabilityMessage;
            set => SetProperty(ref _roomAvailabilityMessage, value);
        }

        // Obliczone właściwości
        public bool IsValid =>
            LiczbaOsob > 0 &&
            (RoomViewModel?.MaxGraczy ?? 0) >= LiczbaOsob &&
            DataRozpoczecia >= DateTime.Today;

        public string StatusText
        {
            get
            {
                if (!IsRoomAvailable)
                {
                    return "Pokój niedostępny w wybranym terminie";
                }

                switch (Status)
                {
                    case ReservationStatus.zarezerwowana:
                        return "Zarezerwowana";
                    case ReservationStatus.odwolana:
                        return "Odwołana";
                    case ReservationStatus.zrealizowana:
                        return "Zrealizowana";
                    default:
                        return "Nieznany";
                }
            }
        }

        public bool CanBeCancelled => Status == ReservationStatus.zarezerwowana;
        public Reservation GetReservation() => _reservation;

        //komendy
        public ICommand ConfirmReservationCommand { get; }
        public ICommand CancelReservationCommand { get; }
        public ICommand CheckRoomAvailabilityCommand { get; }

        private void CheckRoomAvailability(object parameter)
        {
            CheckRoomAvailabilityAsync();
        }

        private async System.Threading.Tasks.Task CheckRoomAvailabilityAsync()
        {
            if (PokojId <= 0 || DataRozpoczecia == default(DateTime))
            {
                IsRoomAvailable = false;
                RoomAvailabilityMessage = "Nie wybrano pokoju lub daty";
                return;
            }

            try
            {
                // Dodaj logowanie do debugowania
                System.Diagnostics.Debug.WriteLine($"Sprawdzanie dostępności pokoju: {PokojId} na datę: {DataRozpoczecia}");

                IsRoomAvailable = await _dataService.IsRoomAvailableAsync(PokojId, DataRozpoczecia);

                if (IsRoomAvailable)
                {
                    RoomAvailabilityMessage = "Pokój dostępny w wybranym terminie";
                    System.Diagnostics.Debug.WriteLine("Pokój dostępny");
                }
                else
                {
                    RoomAvailabilityMessage = "Pokój niedostępny w wybranym terminie - wybierz inną datę";
                    System.Diagnostics.Debug.WriteLine("Pokój niedostępny");
                }

                OnPropertyChanged(nameof(StatusText));
            }
            catch (Exception ex)
            {
                IsRoomAvailable = false;
                RoomAvailabilityMessage = "Błąd sprawdzania dostępności";
                System.Diagnostics.Debug.WriteLine($"Błąd sprawdzania dostępności: {ex.Message}");
                // Zapisz pełny stack trace do debugowania
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        private async void ConfirmReservation(object parameter)
        {
            if (IsValid)
            {
                try
                {
                    // Pobierz aktualnego użytkownika z sesji
                    _reservation.UzytkownikId = UserSession.CurrentUser?.UzytkownikId ?? 0;

                    if (_reservation.UzytkownikId == 0)
                    {
                        MessageBox.Show("Musisz być zalogowany, aby dokonać rezerwacji.",
                            "Logowanie wymagane", MessageBoxButton.OK, MessageBoxImage.Warning);
                        ViewNavigationService.Instance.NavigateTo(ViewType.Login);
                        return;
                    }

                    // Sprawdź ponownie dostępność pokoju przed potwierdzeniem
                    bool isAvailable = await _dataService.IsRoomAvailableAsync(PokojId, DataRozpoczecia);
                    if (!isAvailable)
                    {
                        MessageBox.Show("Ten pokój został właśnie zarezerwowany przez kogoś innego. Wybierz inną datę.",
                            "Pokój niedostępny", MessageBoxButton.OK, MessageBoxImage.Warning);
                        await CheckRoomAvailabilityAsync();
                        return;
                    }

                    // Zapisz rezerwację do bazy danych
                    bool success = await _dataService.AddReservationAsync(_reservation);

                    if (success)
                    {
                        Status = ReservationStatus.zarezerwowana;
                        MessageBox.Show("Rezerwacja potwierdzona pomyślnie.",
                            "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                        ViewNavigationService.Instance.NavigateTo(ViewType.Payment);
                    }
                    else
                    {
                        MessageBox.Show("Nie udało się potwierdzić rezerwacji. Spróbuj ponownie później.",
                            "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Wystąpił błąd podczas potwierdzania rezerwacji: {ex.Message}",
                        "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool CanConfirmReservation(object parameter) => IsValid;

        private void CancelReservation(object parameter)
        {
            Status = ReservationStatus.odwolana;
            ViewNavigationService.Instance.NavigateTo(ViewType.Room);
        }

        private bool CanCancelReservation(object parameter) => CanBeCancelled;

    }
}
