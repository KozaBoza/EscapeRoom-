using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

//OBSŁUGA PŁATNOŚCI (potem wszystko opisze dokladniej) !!!BD MUSIALA TO POZMIENAIC BO TO SA VIEWMODELE
namespace EscapeRoom.Models
{
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }

    public enum PaymentMethod
    {
        CreditCard,
        BankTransfer,
        Cash,
        OnlinePayment
    }

    public class Payment : INotifyPropertyChanged
    {
        private int _id;
        private int _reservationId;
        private decimal _amount;
        private PaymentStatus _status;
        private PaymentMethod _method;
        private DateTime _paymentDate;
        private string _transactionId;
        private string _notes;

       
        private Reservation _reservation;

        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        public int ReservationId
        {
            get => _reservationId;
            set
            {
                if (_reservationId != value)
                {
                    _reservationId = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal Amount
        {
            get => _amount;
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    OnPropertyChanged();
                }
            }
        }

        public PaymentStatus Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        public PaymentMethod Method
        {
            get => _method;
            set
            {
                if (_method != value)
                {
                    _method = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime PaymentDate
        {
            get => _paymentDate;
            set
            {
                if (_paymentDate != value)
                {
                    _paymentDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public string TransactionId
        {
            get => _transactionId;
            set
            {
                if (_transactionId != value)
                {
                    _transactionId = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Notes
        {
            get => _notes;
            set
            {
                if (_notes != value)
                {
                    _notes = value;
                    OnPropertyChanged();
                }
            }
        }

        public Reservation Reservation
        {
            get => _reservation;
            set
            {
                if (_reservation != value)
                {
                    _reservation = value;
                    OnPropertyChanged();
                }
            }
        }

        //konstruktor jkbc
        public Payment()
        {
            PaymentDate = DateTime.Now;
            Status = PaymentStatus.Pending;
            Method = PaymentMethod.CreditCard;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}