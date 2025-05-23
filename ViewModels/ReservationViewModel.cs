using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EscapeRoom.Helpers;
using EscapeRoom.Models;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Windows;

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
            set => SetProperty(ref _reservation.RezerwacjaId, value);
        }

        public int UzytkownikId
        {
            get => _reservation.UzytkownikId;
            set => SetProperty(ref _reservation.UzytkownikId, value);
        }

        public int PokojId
        {
            get => _reservation.PokojId;
            set => SetProperty(ref _reservation.PokojId, value);
        }

        public DateTime DataRozpoczecia
        {
            get => _reservation.DataRozpoczecia;
            set
            {
                if (SetProperty(ref _reservation.DataRozpoczecia, value))
                {
                    OnPropertyChanged(nameof(IsValid));
                    OnPropertyChanged(nameof(DataRozpoczeciaText));
                }
            }
        }

        public byte LiczbaOsob
        {
            get => _reservation.LiczbaOsob;
            set
            {
                if (SetProperty(ref _reservation.LiczbaOsob, value))
                    OnPropertyChanged(nameof(IsValid));
            }
        }

        public ReservationStatus Status
        {
            get => _reservation.Status;
            set
            {
                if (SetProperty(ref _reservation.Status, value))
                    OnPropertyChanged(nameof(StatusText));
            }
        }

        public DateTime DataUtworzenia
        {
            get => _reservation.DataUtworzenia;
            set => SetProperty(ref _reservation.DataUtworzenia, value);
        }

        public UserViewModel UserViewModel
        {
            get => _userViewModel;
            set => SetProperty(ref _userViewModel, value);
        }

        public RoomViewModel RoomViewModel
        {
            get => _roomViewModel;
            set => SetProperty(ref _roomViewModel, value);
        }

        // Obliczone właściwości
        public bool IsValid =>
            DataRozpoczecia > DateTime.Now &&
            LiczbaOsob > 0 &&
            (RoomViewModel?.MaxGraczy ?? 0) >= LiczbaOsob;

        public string StatusText => Status switch
        {
            ReservationStatus.zarezerwowana => "Zarezerwowana",
            ReservationStatus.odwolana => "Odwołana",
            ReservationStatus.zrealizowana => "Zrealizowana",
            _ => "Nieznany"
        };

        public string DataRozpoczeciaText => DataRozpoczecia.ToString("dd.MM.yyyy HH:mm");

        public bool CanBeCancelled => Status == ReservationStatus.zarezerwowana &&
                                     DataRozpoczecia > DateTime.Now.AddHours(2);

        public Reservation GetReservation() => _reservation;

        public ICommand ConfirmReservationCommand { get; }
        public ICommand CancelReservationCommand { get; }

        private void ConfirmReservation(object parameter)
        {
            if (IsValid)
            {
                Status = ReservationStatus.zarezerwowana;
                //logika
            }
        }

        private bool CanConfirmReservation(object parameter) => IsValid;

        private void CancelReservation(object parameter)
        {
            Status = ReservationStatus.odwolana;
            //logika
        }

        private bool CanCancelReservation(object parameter) => CanBeCancelled;
    }
}
}