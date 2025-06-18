using EscapeRoom.Data;
using EscapeRoom.Helpers;
using EscapeRoom.Models;
using EscapeRoom.Services;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using Org.BouncyCastle.Utilities;
using System.Transactions;

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

        // Nowe właściwości dla PaymentView
        private string _amountText;
        private string _methodText;
        private string _paymentStatusText; // Zmieniona nazwa, aby nie kolidować z Reservation.Status
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
            InitializePaymentDetails();
        }

        public ReservationViewModel(RoomViewModel roomViewModel) : this()
        {
            RoomViewModel = roomViewModel; // Ustawienie właściwości RoomViewModel
            // Ustaw PokojId z wybranego pokoju
            _reservation.PokojId = roomViewModel.PokojId;
            // Tutaj możesz też zainicjować inne dane rezerwacji
            _reservation.DataUtworzenia = DateTime.Now; // Ustaw domyślną datę utworzenia
            _reservation.DataRozpoczecia = DateTime.Today; // Ustaw domyślną datę rezerwacji na dzisiaj
            _reservation.Status = ReservationStatus.zarezerwowana; // Domyślny status po utworzeniu

            // Ustaw UzytkownikId z sesji w momencie tworzenia ViewModelu, jeśli użytkownik jest zalogowany
            _reservation.UzytkownikId = UserSession.CurrentUser?.UzytkownikId ?? 0;

            // Sprawdź dostępność pokoju na dzisiaj
            CheckRoomAvailabilityAsync();
            InitializePaymentDetails();
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
            InitializePaymentDetails();
        }

        private void InitializePaymentDetails()
        {
            AmountText = RoomViewModel?.Cena.ToString("C") ?? "N/A"; // Formatowanie jako waluta
            MethodText = "Karta kredytowa (symulacja)"; // Przykładowa metoda
            PaymentStatusText = "Oczekuje na płatność"; // Początkowy status płatności
            PaymentDateText = "Nieopłacono";
            TransactionId = "N/A";
            Notes = "Kliknij 'PRZETWORZENIE' aby zasymulować płatność.";
            CanBeProcessed = true; // Na początku płatność może być przetworzona
            CanBeRefunded = false; // Na początku nie można zwrócić płatności
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
        public ICommand ProcessPaymentCommand { get; }

        private void CheckRoomAvailability(object parameter)
        {
            CheckRoomAvailabilityAsync();
        }

        // Nowe właściwości dla PaymentView
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

        public string PaymentStatusText // Zmieniona nazwa, aby uniknąć kolizji
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
            if (UserSession.CurrentUser?.UzytkownikId == 0)
            {
                MessageBox.Show("Musisz być zalogowany, aby dokonać rezerwacji.",
                    "Logowanie wymagane", MessageBoxButton.OK, MessageBoxImage.Warning);
                ViewNavigationService.Instance.NavigateTo(ViewType.Login);
                return;
            }

            if (!IsValid)
            {
                MessageBox.Show("Proszę wypełnić wszystkie wymagane pola poprawnie i upewnić się, że pokój jest dostępny.",
                   "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _reservation.UzytkownikId = UserSession.CurrentUser.UzytkownikId;

                bool isAvailable = await _dataService.IsRoomAvailableAsync(PokojId, DataRozpoczecia);
                if (!isAvailable)
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

                    // Poniższa linia jest kluczowa: przekazujemy ten sam ViewModel do PaymentView
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

        // ZMODYFIKOWANA METODA: ProcessPayment
        public async Task ProcessPayment()
        {
            try
            {
                if (_reservation.RezerwacjaId == 0)
                {
                    MessageBox.Show("Błąd: ID rezerwacji nie zostało przypisane. Nie można przetworzyć płatności.", "Błąd płatności", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                decimal amountToPay = RoomViewModel?.Cena ?? 0;
                if (amountToPay <= 0)
                {
                    MessageBox.Show("Błąd: Nie można określić kwoty do zapłaty.", "Błąd płatności", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Symulacja zapisania danych płatności (bez faktycznej tabeli)
                bool paymentSuccess = await _dataService.AddPaymentAsync(_reservation.RezerwacjaId, amountToPay, DateTime.Now);

                if (paymentSuccess)
                {
                    // Zmiana statusu rezerwacji w bazie na "zrealizowana"
                    bool statusUpdateSuccess = await _dataService.UpdateReservationStatusAsync(_reservation.RezerwacjaId, ReservationStatus.zrealizowana);

                    if (statusUpdateSuccess)
                    {
                        // Zmiana statusu pokoju na "zarezerwowany" (lub "zajęty", "zrealizowany" - jakakolwiek konwencja)
                        // Używamy "zarezerwowany" aby wskazać, że pokój jest zajęty przez opłaconą rezerwację.
                        await _dataService.UpdateRoomStatusAsync(_reservation.PokojId, "zarezerwowany"); // Przykładowy status

                        Status = ReservationStatus.zrealizowana; // Aktualizacja w ViewModelu
                        PaymentStatusText = "Zrealizowana"; // Aktualizacja statusu płatności w PaymentView
                        PaymentDateText = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                        TransactionId = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(); // Generowanie prostego ID transakcji
                        Notes = "Płatność zakończona sukcesem. Dziękujemy!";
                        CanBeProcessed = false; // Płatność już przetworzona
                        CanBeRefunded = true; // Teraz można zwrócić

                        MessageBox.Show("Płatność i rezerwacja zostały pomyślnie zrealizowane!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                        ViewNavigationService.Instance.NavigateTo(ViewType.User); // Przejście do panelu użytkownika
                    }
                    else
                    {
                        MessageBox.Show("Płatność została przetworzona, ale nie udało się zaktualizować statusu rezerwacji.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Nie udało się przetworzyć płatności. Spróbuj ponownie.", "Błąd Płatności", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas przetwarzania płatności: {ex.Message}", "Błąd Płatności", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanProcessPayment(object parameter)
        {
            // Płatność możliwa, jeśli rezerwacja jest wstępnie utworzona, walidacja jest poprawna,
            // i jeszcze nie została przetworzona.
            return _reservation.RezerwacjaId > 0 && IsValid && CanBeProcessed;
        }

        // NOWA METODA: RefundPayment
        private async void RefundPayment(object parameter)
        {
            try
            {
                if (_reservation.RezerwacjaId == 0)
                {
                    MessageBox.Show("Błąd: ID rezerwacji nie zostało przypisane. Nie można przetworzyć zwrotu.", "Błąd zwrotu", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Symulacja procesu zwrotu (bez faktycznego zwrotu pieniędzy)
                // W prawdziwej aplikacji byłaby tu integracja z bramką płatności.
                bool refundSuccess = await _dataService.UpdateReservationStatusAsync(_reservation.RezerwacjaId, ReservationStatus.odwolana); // Zmieniamy status na anulowany

                if (refundSuccess)
                {
                    // Ustaw status pokoju z powrotem na "wolny"
                    await _dataService.UpdateRoomStatusAsync(_reservation.PokojId, "wolny");

                    Status = ReservationStatus.odwolana; // Aktualizacja w ViewModelu
                    PaymentStatusText = "Zwrócona";
                    PaymentDateText = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                    TransactionId = "ZWRÓCONO: " + TransactionId; // Zmiana ID na oznaczenie zwrotu
                    Notes = "Płatność została zwrócona, rezerwacja anulowana.";
                    CanBeProcessed = false;
                    CanBeRefunded = false; // Nie można zwrócić drugi raz

                    MessageBox.Show("Zwrot płatności i anulowanie rezerwacji zakończone pomyślnie.", "Zwrot", MessageBoxButton.OK, MessageBoxImage.Information);
                    ViewNavigationService.Instance.NavigateTo(ViewType.User); // Przejście do panelu użytkownika
                }
                else
                {
                    MessageBox.Show("Nie udało się przetworzyć zwrotu. Spróbuj ponownie.", "Błąd Zwrotu", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas przetwarzania zwrotu: {ex.Message}", "Błąd Zwrotu", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanRefundPayment(object parameter)
        {
            // Zwrot możliwy, jeśli płatność została przetworzona i nie jest już zwrócona/anulowana
            return Status == ReservationStatus.zrealizowana && CanBeRefunded;
        }


        private void CancelReservation(object parameter)
        {
            // Możesz zapytać użytkownika o potwierdzenie
            if (MessageBox.Show("Czy na pewno chcesz anulować tę rezerwację? Spowoduje to również anulowanie ewentualnej płatności.", "Potwierdź anulowanie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    // Opcjonalnie: Jeśli płatność została przetworzona, spróbuj ją zwrócić
                    if (Status == ReservationStatus.zrealizowana)
                    {
                        RefundPayment(null); // Wywołaj funkcję zwrotu
                        return; // RefundPayment zajmie się nawigacją
                    }
                    else
                    {
                        // Jeśli nie ma płatności, tylko anuluj rezerwację w bazie
                        _dataService.UpdateReservationStatusAsync(_reservation.RezerwacjaId, ReservationStatus.odwolana);
                        // Ustaw status pokoju z powrotem na "wolny" (jeśli był zarezerwowany)
                        _dataService.UpdateRoomStatusAsync(_reservation.PokojId, "wolny");
                    }

                    Status = ReservationStatus.odwolana;
                    MessageBox.Show("Rezerwacja została anulowana.", "Anulowano", MessageBoxButton.OK, MessageBoxImage.Information);
                    ViewNavigationService.Instance.NavigateTo(ViewType.Room); // Możesz nawigować do innej strony, np. listy rezerwacji użytkownika
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Wystąpił błąd podczas anulowania rezerwacji: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool CanCancelReservation(object parameter) => CanBeCancelled;

    }
}
