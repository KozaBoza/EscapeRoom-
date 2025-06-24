using System;
using System.ComponentModel;
using System.Windows.Input;
using EscapeRoom.Helpers;
using EscapeRoom.Models;
using EscapeRoom.Services;

namespace EscapeRoom.ViewModels
{
    public class PaymentViewModel : BaseViewModel
    {
        private int _id;
        private int _reservationId;
        private decimal _amount;
        private PaymentStatus _status;
        private PaymentMethod _method;
        private DateTime _paymentDate;
        private string _transactionId;
        private string _notes;

        public PaymentViewModel(ReservationViewModel reservationViewModel)
        {
            // Pobierz cenę z wybranego pokoju
            Amount = reservationViewModel?.RoomViewModel?.Cena ?? 0;
            ReservationId = reservationViewModel?.RezerwacjaId ?? 0; // <-- TO JEST KLUCZOWE
            PaymentDate = DateTime.Now;
            Status = PaymentStatus.Pending;
            TransactionId = "2137419" + GetRandomTransactionSuffix();

            ProcessPaymentCommand = new RelayCommand(param => ProcessPayment(), CanProcessPayment);
            CancelPaymentCommand = new RelayCommand(CancelPayment, CanCancelPayment);
        }

        public PaymentViewModel()
        {
            PaymentDate = DateTime.Now;
            Status = PaymentStatus.Pending;
            TransactionId = "2137419" + GetRandomTransactionSuffix();

            ProcessPaymentCommand = new RelayCommand(param => ProcessPayment(), CanProcessPayment);
            CancelPaymentCommand = new RelayCommand(CancelPayment, CanCancelPayment);
        }

        // Nowa metoda generująca unikalny sufiks dla numeru transakcji
        private string GetRandomTransactionSuffix()
        {
            return new Random().Next(1000, 9999).ToString();
        }

        // Modyfikacja właściwości do wyświetlania
        // Zamiast
        public string AmountText => Amount.ToString("C", new System.Globalization.CultureInfo("en-US"));

        public string PaymentDateText => PaymentDate.ToString("dd.MM.yyyy HH:mm");
        public string TransactionIdText => $"Nr transakcji: {TransactionId}";

        private void ProcessPayment()
        {
            System.Threading.Thread.Sleep(2000);
            SavePaymentToDatabase();
            ViewNavigationService.Instance.NavigateTo(ViewType.Homepage);
        }

        private async void SavePaymentToDatabase()
        {
            var dataService = new EscapeRoom.Data.DataService();
            bool result = await dataService.AddPaymentAsync(ReservationId, UserSession.CurrentUser.UzytkownikId);

            if (result)
            {
                System.Windows.MessageBox.Show(
                    "Płatność została przetworzona pomyślnie!\nTwoja rezerwacja została potwierdzona.",
                    "Sukces",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Information);

                EscapeRoom.Services.ViewNavigationService.Instance.NavigateTo(EscapeRoom.Services.ViewType.User);
            }
            else
            {
                System.Windows.MessageBox.Show(
                    "Nie udało się przetworzyć płatności. Spróbuj ponownie.",
                    "Błąd",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
        }


        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public int ReservationId
        {
            get => _reservationId;
            set => SetProperty(ref _reservationId, value);
        }

        public decimal Amount
        {
            get => _amount;
            set
            {
                if (SetProperty(ref _amount, value))
                {
                    OnPropertyChanged(nameof(AmountText));
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        public PaymentStatus Status
        {
            get => _status;
            set
            {
                if (SetProperty(ref _status, value))
                {
                    OnPropertyChanged(nameof(StatusText));
                    ((RelayCommand)ProcessPaymentCommand).RaiseCanExecuteChanged();
                    ((RelayCommand)CancelPaymentCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public PaymentMethod Method
        {
            get => _method;
            set
            {
                if (SetProperty(ref _method, value))
                    OnPropertyChanged(nameof(MethodText));
            }
        }

        public DateTime PaymentDate
        {
            get => _paymentDate;
            set
            {
                if (SetProperty(ref _paymentDate, value))
                    OnPropertyChanged(nameof(PaymentDateText));
            }
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


        public string StatusText
        {
            get
            {
                switch (Status)
                {
                    case PaymentStatus.Pending: return "Oczekująca";
                    case PaymentStatus.Completed: return "Zakończona";
                    case PaymentStatus.Failed: return "Nieudana";
                    //case PaymentStatus.Refunded: return "Zwrócona";
                    case PaymentStatus.Canceled: return "Anulowana"; //  nowy status
                    default: return "Nieznany";
                }
            }
        }

        public string MethodText
        {
            get
            {
                switch (Method)
                {
                    case PaymentMethod.CreditCard: return "Gotowka";
                    case PaymentMethod.BankTransfer: return "Przelew bankowy";
                    case PaymentMethod.Cash: return "Gotówka";
                    case PaymentMethod.OnlinePayment: return "Płatność online";
                    default: return "Nieznany";
                }
            }
        }


        public bool IsValid => Amount > 0 && ReservationId > 0;

        public bool CanBeProcessed => Status == PaymentStatus.Pending;
        public bool CanBeCanceled => Status == PaymentStatus.Pending;

        public ICommand ProcessPaymentCommand { get; }
        public ICommand CancelPaymentCommand { get; } // z RefundPaymentCommand

        private bool CanProcessPayment(object parameter) => CanBeProcessed && IsValid;

        private void CancelPayment(object parameter) // Zmieniono nazwę 
        {
            Status = PaymentStatus.Canceled;
            Notes = $"Płatność anulowana {DateTime.Now:dd.MM.yyyy HH:mm}"; // Zmieniono 
        }

        private bool CanCancelPayment(object parameter) => CanBeCanceled;

        public Payment GetPayment()
        {
            return new Payment
            {
                Id = this.Id,
                ReservationId = this.ReservationId,
                Amount = this.Amount,
                Status = this.Status,
                Method = this.Method,
                PaymentDate = this.PaymentDate,
                TransactionId = this.TransactionId,
                Notes = this.Notes
            };
        }

        public void LoadFromPayment(Payment payment)
        {
            if (payment == null) return;

            Id = payment.Id;
            ReservationId = payment.ReservationId;
            Amount = payment.Amount;
            Status = payment.Status;
            Method = payment.Method;
            PaymentDate = payment.PaymentDate;
            TransactionId = payment.TransactionId;
            Notes = payment.Notes;
        }
    }
}