using System;
using System.Windows.Input;
using EscapeRoom.Data;
using EscapeRoom.Helpers;
using EscapeRoom.Models;
using EscapeRoom.Services;

namespace EscapeRoom.ViewModels
{
    public class ReservationViewModel : BaseViewModel
    { //ZEBY BYLO MVVP TRZEBA PRZENIESC TE FUNKCJE TO MODELS
        private Reservation _reservation;
        private UserViewModel _userViewModel;
        private RoomViewModel _roomViewModel;

        public ReservationViewModel()
        {
            _reservation = new Reservation();
            ConfirmReservationCommand = new RelayCommand(ConfirmReservation, CanConfirmReservation);
            CancelReservationCommand = new RelayCommand(CancelReservation, CanCancelReservation);
        }

        public ReservationViewModel(Reservation reservation) : this()
        {
            _reservation = reservation ?? new Reservation();
            LoadAssociatedDataAsync();
        }

        public ReservationViewModel(RoomViewModel roomViewModel) : this()
        {
            RoomViewModel = roomViewModel; // Ustawienie właściwości RoomViewModel
                                           // Ustaw PokojId z wybranego pokoju
            _reservation.PokojId = roomViewModel.PokojId;
            // Tutaj możesz też zainicjować inne dane rezerwacji
            _reservation.DataUtworzenia = DateTime.Now; // Ustaw domyślną datę utworzenia
            Status = ReservationStatus.zarezerwowana; // Domyślny status po utworzeniu
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

        // Obliczone właściwości
        public bool IsValid =>
            LiczbaOsob > 0 &&
            (RoomViewModel?.MaxGraczy ?? 0) >= LiczbaOsob;

        public string StatusText
        {
            get
            {
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

        private void ConfirmReservation(object parameter)
        {
            if (IsValid)
            {
                Status = ReservationStatus.zarezerwowana;
                ViewNavigationService.Instance.NavigateTo(ViewType.Payment);
                //przenoszenie
            }
        }

        private bool CanConfirmReservation(object parameter) => IsValid;

        private void CancelReservation(object parameter)
        {
            Status = ReservationStatus.odwolana;
            // logika anulowania
        }

        private bool CanCancelReservation(object parameter) => CanBeCancelled;

    }
}
