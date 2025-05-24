using System;
using System.Windows.Input;
using EscapeRoom.Helpers;
using EscapeRoom.Models;

namespace EscapeRoom.ViewModels
{
    public class ReservationViewModel : BaseViewModel
    {
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
                    OnPropertyChanged(nameof(DataRozpoczeciaText));
                    OnPropertyChanged(nameof(CanBeCancelled));
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
            DataRozpoczecia > DateTime.Now &&
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

        public string DataRozpoczeciaText => DataRozpoczecia.ToString("dd.MM.yyyy HH:mm");

        public bool CanBeCancelled => Status == ReservationStatus.zarezerwowana &&
                                      DataRozpoczecia > DateTime.Now.AddHours(2);

        public Reservation GetReservation() => _reservation;

        //komendy
        public ICommand ConfirmReservationCommand { get; }
        public ICommand CancelReservationCommand { get; }

        private void ConfirmReservation(object parameter)
        {
            if (IsValid)
            {
                Status = ReservationStatus.zarezerwowana;
                // logika potwierdzenia
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
