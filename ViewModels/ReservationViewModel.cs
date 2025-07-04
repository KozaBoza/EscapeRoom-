﻿using EscapeRoom.Data;
using EscapeRoom.Helpers;
using EscapeRoom.Models;
using EscapeRoom.Services;
using System;
using System.Threading.Tasks;
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

        private string _amountText;
        private string _methodText;
        private string _paymentStatusText;
        private string _paymentDateText;
        private string _transactionId;
        private string _notes;
        private bool _canBeProcessed;
        private bool _canBeRefunded;

        public ReservationViewModel()
        {
            _dataService = new DataService();
            _reservation = new Reservation();
            _reservation.DataRozpoczecia = DateTime.Today;
            _reservation.DataUtworzenia = DateTime.Now;
            _reservation.Status = ReservationStatus.zarezerwowana;

            InitializeCommands();
            InitializePaymentDetails();
        }

        public ReservationViewModel(Reservation reservation) : this()
        {
            _reservation = reservation ?? new Reservation();
            if (_reservation.DataRozpoczecia == default(DateTime))
            {
                _reservation.DataRozpoczecia = DateTime.Today;
            }
            LoadAssociatedDataAsync();
        }

        private void InitializeCommands()
        {
            ConfirmReservationCommand = new RelayCommand(ConfirmReservation, CanConfirmReservation);
            CancelReservationCommand = new RelayCommand(CancelReservation, CanCancelReservation);
            ProcessPaymentCommand = new RelayCommand(async param => await ProcessPayment(), CanProcessPayment);
            RefundPaymentCommand = new RelayCommand(async param => await RefundPayment(), CanRefundPayment);
            CheckRoomAvailabilityCommand = new RelayCommand(async param => await CheckRoomAvailabilityAsync());
        }

        public string NazwaPokoju => _reservation.Pokoj?.Nazwa ?? "Brak nazwy";
        public byte Trudnosc => _reservation.Pokoj?.Trudnosc ?? 0;
        public decimal Cena => _reservation.Pokoj?.Cena ?? 0m;
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
                    CheckRoomAvailabilityAsync();
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
            get => _reservation?.Status ?? ReservationStatus.zarezerwowana;
            set
            {
                if (_reservation != null && _reservation.Status != value)
                {
                    _reservation.Status = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(StatusDisplay));
                    ((RelayCommand)RefundPaymentCommand)?.RaiseCanExecuteChanged();
                    ((RelayCommand)ProcessPaymentCommand)?.RaiseCanExecuteChanged();
                }
            }
        }
        public string StatusDisplay
        {
            get
            {
                if (_reservation.DataRozpoczecia.Date < DateTime.Now.Date)
                    return "Odbyte";
                return _reservation.Status.ToString();
            }
        }

        public ReservationViewModel(RoomViewModel roomViewModel) : this()
        {
            RoomViewModel = roomViewModel;
            _reservation.PokojId = roomViewModel.PokojId;
            _reservation.UzytkownikId = UserSession.CurrentUser?.UzytkownikId ?? 0;
            CheckRoomAvailabilityAsync();
            InitializePaymentDetails();
        }

        private async void LoadAssociatedDataAsync()
        {
            if (_reservation.PokojId > 0)
            {
                var room = await _dataService.GetRoomByIdAsync(_reservation.PokojId);
                if (room != null)
                {
                    RoomViewModel = new RoomViewModel(room);
                }
            }

            InitializePaymentDetails();
            OnPropertyChanged(nameof(IsValid));
        }

        private void InitializePaymentDetails()
        {
            AmountText = RoomViewModel?.Cena.ToString("C", new System.Globalization.CultureInfo("en-US")) ?? "N/A";
            MethodText = "Karta kredytowa (symulacja)";
            PaymentStatusText = "Oczekuje na płatność";
            PaymentDateText = "Nieopłacono";
            TransactionId = "N/A";
            Notes = "Kliknij 'PRZETWORZENIE' aby zasymulować płatność.";
            CanBeProcessed = true;
            CanBeRefunded = false;
        }

        // Properties...
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
                    InitializePaymentDetails();
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
                    ((RelayCommand)ConfirmReservationCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public string RoomAvailabilityMessage
        {
            get => _roomAvailabilityMessage;
            set => SetProperty(ref _roomAvailabilityMessage, value);
        }

        public bool IsValid =>
            LiczbaOsob > 0 &&
            (RoomViewModel?.MaxGraczy ?? 0) >= LiczbaOsob &&
            DataRozpoczecia >= DateTime.Today &&
            RoomViewModel != null &&
            IsRoomAvailable;

        public Room Pokoj => _reservation.Pokoj;

        public Reservation GetReservation() => _reservation;

        // Commands
        public ICommand ConfirmReservationCommand { get; private set; }
        public ICommand CancelReservationCommand { get; private set; }
        public ICommand ProcessPaymentCommand { get; private set; }
        public ICommand RefundPaymentCommand { get; private set; }
        public ICommand CheckRoomAvailabilityCommand { get; private set; }

        // Payment properties
        public string AmountText
        {
            get => _amountText;
            set => SetProperty(ref _amountText, value);
        }

        public string MethodText
        {
            get => _methodText;
            set => SetProperty(ref _methodText, value);
        }

        public string PaymentStatusText
        {
            get => _paymentStatusText;
            set => SetProperty(ref _paymentStatusText, value);
        }

        public string PaymentDateText
        {
            get => _paymentDateText;
            set => SetProperty(ref _paymentDateText, value);
        }

        public string TransactionId
        {
            get => _transactionId;
            set => SetProperty(ref _transactionId, value);
        }

        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        public bool CanBeProcessed
        {
            get => _canBeProcessed;
            set => SetProperty(ref _canBeProcessed, value);
        }

        public bool CanBeRefunded
        {
            get => _canBeRefunded;
            set => SetProperty(ref _canBeRefunded, value);
        }

        // Methods
        private async Task CheckRoomAvailabilityAsync()
        {
            if (PokojId <= 0 || DataRozpoczecia == default(DateTime) || RoomViewModel == null)
            {
                IsRoomAvailable = false;
                RoomAvailabilityMessage = "Wybierz pokój i datę, aby sprawdzić dostępność.";
                return;
            }

            try
            {
                System.Diagnostics.Debug.WriteLine($"Sprawdzanie dostępności pokoju: {PokojId} na datę: {DataRozpoczecia}");

                IsRoomAvailable = await _dataService.IsRoomAvailableAsync(PokojId, DataRozpoczecia);

                if (IsRoomAvailable)
                {
                    RoomAvailabilityMessage = "Pokój dostępny w wybranym terminie.";
                    System.Diagnostics.Debug.WriteLine("Pokój dostępny");
                }
                else
                {
                    RoomAvailabilityMessage = "Pokój niedostępny w wybranym terminie - wybierz inną datę lub pokój.";
                    System.Diagnostics.Debug.WriteLine("Pokój niedostępny");
                }
            }
            catch (Exception ex)
            {
                IsRoomAvailable = false;
                RoomAvailabilityMessage = $"Błąd sprawdzania dostępności: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Błąd sprawdzania dostępności: {ex.Message}");
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                MessageBox.Show($"Wystąpił błąd podczas sprawdzania dostępności: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ConfirmReservation(object parameter)
        {
            if (UserSession.CurrentUser?.UzytkownikId == 0)
            {
                MessageBox.Show("Musisz być zalogowany, aby dokonać rezerwacji.",
                    "Logowanie wymagane", MessageBoxButton.OK, MessageBoxImage.Warning);
                ViewNavigationService.Instance.NavigateTo(ViewType.Login);
                return;
            }

            if (!IsValid)
            {
                MessageBox.Show("Proszę wypełnić wszystkie wymagane pola poprawnie i upewnić się, że pokój jest dostępny. Max liczba osób to: " + (RoomViewModel?.MaxGraczy ?? 0) + ", a data musi być z dzisiaj lub przyszłości.",
                   "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _reservation.UzytkownikId = UserSession.CurrentUser.UzytkownikId;
                _reservation.DataUtworzenia = DateTime.Now;

                bool isAvailableNow = await _dataService.IsRoomAvailableAsync(PokojId, DataRozpoczecia);
                if (!isAvailableNow)
                {
                    MessageBox.Show("Ten pokój został właśnie zarezerwowany przez kogoś innego lub jest niedostępny. Wybierz inną datę.",
                        "Pokój niedostępny", MessageBoxButton.OK, MessageBoxImage.Warning);
                    await CheckRoomAvailabilityAsync();
                    return;
                }

                bool success = await _dataService.AddReservationAsync(_reservation);

                if (success)
                {
                    MessageBox.Show("Rezerwacja wstępnie utworzona. Przekierowanie do płatności.",
                           "Przejdź do płatności", MessageBoxButton.OK, MessageBoxImage.Information);
                    ViewNavigationService.Instance.NavigateTo(ViewType.Payment, this);
                }
                else
                {
                    MessageBox.Show("Nie udało się utworzyć rezerwacji. Spróbuj ponownie później.",
                        "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas tworzenia rezerwacji: {ex.Message}",
                    "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanConfirmReservation(object parameter) => IsValid;

        private void CancelReservation(object parameter)
        {
            var result = MessageBox.Show(
                "Czy na pewno chcesz anulować rezerwację?",
                "Anulowanie rezerwacji",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ViewNavigationService.Instance.NavigateTo(ViewType.Room);
            }
        }

        private bool CanCancelReservation(object parameter) => true;

        private async Task ProcessPayment()
        {
            try
            {
                if (_reservation.RezerwacjaId == 0)
                {
                    MessageBox.Show("Rezerwacja nie została jeszcze zapisana.",
                        "Błąd płatności", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                decimal amountToPay = RoomViewModel?.Cena ?? 0;
                if (amountToPay <= 0)
                {
                    MessageBox.Show("Błąd: Nie można określić kwoty do zapłaty.",
                        "Błąd płatności", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (MessageBox.Show($"Czy chcesz potwierdzić płatność na kwotę {amountToPay:C}?",
                    "Potwierdzenie płatności", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                {
                    return;
                }

                bool paymentSuccess = await _dataService.AddPaymentAsync(
                    _reservation.RezerwacjaId,
                    UserSession.CurrentUser.UzytkownikId);

                if (paymentSuccess)
                {
                    bool statusUpdateSuccess = await _dataService.UpdateReservationStatusAsync(
                        _reservation.RezerwacjaId,
                        ReservationStatus.oplacona);

                    if (statusUpdateSuccess)
                    {
                        Status = ReservationStatus.oplacona;
                        PaymentDateText = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                        TransactionId = $"TR{DateTime.Now:yyyyMMdd}{new Random().Next(1000, 9999)}";
                        PaymentStatusText = "Opłacono";
                        Notes = "Płatność zatwierdzona pomyślnie";
                        CanBeProcessed = false;
                        CanBeRefunded = true;

                        MessageBox.Show("Płatność została zrealizowana pomyślnie!",
                            "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                        ViewNavigationService.Instance.NavigateTo(ViewType.User);
                    }
  else
                    {
                        MessageBox.Show("Płatność została przetworzona, ale nie udało się zaktualizować statusu rezerwacji.",
                            "Ostrzeżenie", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Nie udało się przetworzyć płatności. Spróbuj ponownie.",
                        "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas przetwarzania płatności: {ex.Message}",
                    "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanProcessPayment(object parameter)
        {
            var canProcess = _reservation.RezerwacjaId > 0 &&
                            IsValid &&
                            CanBeProcessed &&
                            RoomViewModel != null;

            System.Diagnostics.Debug.WriteLine($"CanProcessPayment: " +
                $"\nRezerwacjaId > 0: {_reservation.RezerwacjaId > 0}" +
                $"\nIsValid: {IsValid}" +
                $"\nCanBeProcessed: {CanBeProcessed}" +
                $"\nRoomViewModel != null: {RoomViewModel != null}" +
                $"\nFinal result: {canProcess}");

            return canProcess;
        }
        private async Task RefundPayment()
        {
            try
            {
                if (_reservation.RezerwacjaId == 0)
                {
                    MessageBox.Show("Błąd: ID rezerwacji nie zostało przypisane. Nie można przetworzyć zwrotu.",
                        "Błąd zwrotu", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (MessageBox.Show("Czy na pewno chcesz dokonać zwrotu płatności i anulować rezerwację?",
                    "Potwierdź zwrot", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                {
                    return;
                }

                bool refundSuccess = await _dataService.UpdateReservationStatusAsync(
                    _reservation.RezerwacjaId,
                    ReservationStatus.odwolana);

                if (refundSuccess)
                {
                    await _dataService.UpdateRoomStatusAsync(_reservation.PokojId, "wolny");

                    PaymentStatusText = "Zwrócona";
                    PaymentDateText = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                    TransactionId = "ZWRÓCONO: " + TransactionId;
                    Notes = "Płatność została zwrócona, rezerwacja anulowana.";
                    CanBeProcessed = false;
                    CanBeRefunded = false;

                    MessageBox.Show("Zwrot płatności i anulowanie rezerwacji zakończone pomyślnie.",
                        "Zwrot", MessageBoxButton.OK, MessageBoxImage.Information);
                    ViewNavigationService.Instance.NavigateTo(ViewType.User);
                }
                else
                {
                    MessageBox.Show("Nie udało się przetworzyć zwrotu. Spróbuj ponownie.",
                        "Błąd Zwrotu", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas przetwarzania zwrotu: {ex.Message}",
                    "Błąd Zwrotu", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

            private bool CanRefundPayment(object parameter)
        {
            return true;
        }
    }
}