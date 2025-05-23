using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EscapeRoom.Helpers;
using EscapeRoom.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Input;


namespace EscapeRoom.ViewModels
{
    public class PaymentViewModel : BaseViewModel
    {
        private Payment _payment;

        public PaymentViewModel()
        {
            _payment = new Payment();
            ProcessPaymentCommand = new RelayCommand(ProcessPayment, CanProcessPayment);
            RefundPaymentCommand = new RelayCommand(RefundPayment, CanRefundPayment);
        }

        public PaymentViewModel(Payment payment) : this()
        {
            _payment = payment ?? new Payment();
        }

        public int Id
        {
            get => GetFieldValue<int>("_id");
            set => SetFieldValue(value, "_id");
        }

        public int ReservationId
        {
            get => GetFieldValue<int>("_reservationId");
            set => SetFieldValue(value, "_reservationId");
        }

        public decimal Amount
        {
            get => GetFieldValue<decimal>("_amount");
            set
            {
                if (SetFieldValue(value, "_amount"))
                {
                    OnPropertyChanged(nameof(AmountText));
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        public PaymentStatus Status
        {
            get => GetFieldValue<PaymentStatus>("_status");
            set
            {
                if (SetFieldValue(value, "_status"))
                    OnPropertyChanged(nameof(StatusText));
            }
        }

        public PaymentMethod Method
        {
            get => GetFieldValue<PaymentMethod>("_method");
            set
            {
                if (SetFieldValue(value, "_method"))
                    OnPropertyChanged(nameof(MethodText));
            }
        }

        public DateTime PaymentDate
        {
            get => GetFieldValue<DateTime>("_paymentDate");
            set
            {
                if (SetFieldValue(value, "_paymentDate"))
                    OnPropertyChanged(nameof(PaymentDateText));
            }
        }

        public string TransactionId
        {
            get => GetFieldValue<string>("_transactionId");
            set => SetFieldValue(value, "_transactionId");
        }

        public string Notes
        {
            get => GetFieldValue<string>("_notes");
            set => SetFieldValue(value, "_notes");
        }

        // Obliczone właściwości
        public string AmountText => Amount.ToString("C");

        public string StatusText => Status switch
        {
            PaymentStatus.Pending => "Oczekująca",
            PaymentStatus.Completed => "Zakończona",
            PaymentStatus.Failed => "Nieudana",
            PaymentStatus.Refunded => "Zwrócona",
            _ => "Nieznany"
        };

        public string MethodText => Method switch
        {
            PaymentMethod.CreditCard => "Karta kredytowa",
            PaymentMethod.BankTransfer => "Przelew bankowy",
            PaymentMethod.Cash => "Gotówka",
            PaymentMethod.OnlinePayment => "Płatność online",
            _ => "Nieznany"
        };

        public string PaymentDateText => PaymentDate.ToString("dd.MM.yyyy HH:mm");

        public bool IsValid => Amount > 0 && ReservationId > 0;

        public bool CanBeRefunded => Status == PaymentStatus.Completed;
        public bool CanBeProcessed => Status == PaymentStatus.Pending;

        //komendy
        public ICommand ProcessPaymentCommand { get; }
        public ICommand RefundPaymentCommand { get; }

        private void ProcessPayment(object parameter)
        {
            Status = PaymentStatus.Completed;
            PaymentDate = DateTime.Now;
            TransactionId = Guid.NewGuid().ToString("N")[..8].ToUpper();
            //logika przetwarzania płatności
        }

        private bool CanProcessPayment(object parameter) => CanBeProcessed && IsValid;

        private void RefundPayment(object parameter)
        {
            Status = PaymentStatus.Refunded;
            Notes = $"Zwrot wykonany {DateTime.Now:dd.MM.yyyy HH:mm}";
            //logika zwrotu płatności
        }

        private bool CanRefundPayment(object parameter) => CanBeRefunded;

        //do pracy z prywatymi polami przez refleksję
        private T GetFieldValue<T>(string fieldName)
        {
            var field = typeof(Payment).GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return field != null ? (T)field.GetValue(_payment) : default(T);
        }

        private bool SetFieldValue<T>(T value, string fieldName)
        {
            var field = typeof(Payment).GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                var currentValue = (T)field.GetValue(_payment);
                if (!Equals(currentValue, value))
                {
                    field.SetValue(_payment, value);
                    return true;
                }
            }
            return false;
        }
    }
}
