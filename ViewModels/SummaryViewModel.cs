using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EscapeRoom.Helpers;
using EscapeRoom.Models;
using EscapeRoom.Services;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace EscapeRoom.ViewModels
{
    public class SummaryViewModel : INotifyPropertyChanged
    {
        private Reservation _reservation;
        private Room _room;
        private User _user;
        private bool _isBusy;
        private string _statusMessage;
        private bool _isReservationConfirmed;
        private string _paymentMethod;
        private string _cardNumber;
        private string _cardHolderName;
        private string _expiryDate;
        private string _cvv;
        private ObservableCollection<string> _paymentMethods;

        public SummaryViewModel()
        {
            PaymentMethods = new ObservableCollection<string>
            {
                "Credit Card",
                "Debit Card",
                "PayPal",
                "Pay at Location"
            };
            PaymentMethod = PaymentMethods[0]; 
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

        public Room Room
        {
            get => _room;
            set
            {
                if (_room != value)
                {
                    _room = value;
                    OnPropertyChanged();
                }
            }
        }

        public User User
        {
            get => _user;
            set
            {
                if (_user != value)
                {
                    _user = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                }
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage != value)
                {
                    _statusMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsReservationConfirmed
        {
            get => _isReservationConfirmed;
            set
            {
                if (_isReservationConfirmed != value)
                {
                    _isReservationConfirmed = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PaymentMethod
        {
            get => _paymentMethod;
            set
            {
                if (_paymentMethod != value)
                {
                    _paymentMethod = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsCardPayment));
                    OnPropertyChanged(nameof(IsPayPal));
                    OnPropertyChanged(nameof(IsPayAtLocation));
                }
            }
        }

        public ObservableCollection<string> PaymentMethods
        {
            get => _paymentMethods;
            set
            {
                if (_paymentMethods != value)
                {
                    _paymentMethods = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CardNumber
        {
            get => _cardNumber;
            set
            {
                if (_cardNumber != value)
                {
                    _cardNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CardHolderName
        {
            get => _cardHolderName;
            set
            {
                if (_cardHolderName != value)
                {
                    _cardHolderName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ExpiryDate
        {
            get => _expiryDate;
            set
            {
                if (_expiryDate != value)
                {
                    _expiryDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CVV
        {
            get => _cvv;
            set
            {
                if (_cvv != value)
                {
                    _cvv = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsCardPayment => PaymentMethod == "Credit Card" || PaymentMethod == "Debit Card";
        public bool IsPayPal => PaymentMethod == "PayPal";
        public bool IsPayAtLocation => PaymentMethod == "Pay at Location";
        private ICommand _processPaymentCommand;
        public ICommand ProcessPaymentCommand
        {
            get
            {
                return _processPaymentCommand ?? (_processPaymentCommand = new RelayCommand(_ => ProcessPayment(), _ => CanProcessPayment()));
            }
        }

        private ICommand _cancelReservationCommand;
        public ICommand CancelReservationCommand
        {
            get
            {
                return _cancelReservationCommand ?? (_cancelReservationCommand = new RelayCommand(_ => CancelReservation(), _ => CanCancelReservation()));
            }
        }

        private ICommand _navigateToHomeCommand;
        public ICommand NavigateToHomeCommand
        {
            get
            {
                return _navigateToHomeCommand ?? (_navigateToHomeCommand = new RelayCommand(_ => NavigateToHome(), _ => true));
            }
        }

        private ICommand _printConfirmationCommand;
        public ICommand PrintConfirmationCommand
        {
            get
            {
                return _printConfirmationCommand ?? (_printConfirmationCommand = new RelayCommand(_ => PrintConfirmation(), _ => IsReservationConfirmed));
            }
        }


        private bool CanProcessPayment()
        {
            if (IsBusy || Reservation == null)
                return false;

            if (IsPayAtLocation)
                return true;

            if (IsCardPayment)
            {
                return !string.IsNullOrWhiteSpace(CardNumber) &&
                       !string.IsNullOrWhiteSpace(CardHolderName) &&
                       !string.IsNullOrWhiteSpace(ExpiryDate) &&
                       !string.IsNullOrWhiteSpace(CVV);
            }

            if (IsPayPal)
            {
                return true;
            }

            return false;
        }

        private async void ProcessPayment()
        {
            if (!CanProcessPayment())
                return;

            try
            {
                IsBusy = true;
                StatusMessage = "Processing payment...";
                var payment = new Payment
                {
                    ReservationId = Reservation.Id,
                    Amount = Reservation.TotalPrice,
                    PaymentMethod = PaymentMethod,
                    PaymentDate = DateTime.Now,
                    Status = "Pending"
                };
                bool paymentSuccess = true;

                if (paymentSuccess)
                {
                    payment.Status = "Completed";
                    Reservation.Status = "Confirmed";
                    IsReservationConfirmed = true;
                    StatusMessage = "Payment processed successfully! Your reservation is confirmed.";
                }
                else
                {
                    StatusMessage = "Payment processing failed. Please try again or choose a different payment method.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error processing payment: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanCancelReservation()
        {
            return !IsBusy && Reservation != null && !IsReservationConfirmed;
        }

        private async void CancelReservation()
        {
            if (!CanCancelReservation())
                return;

            try
            {
                IsBusy = true;
                StatusMessage = "Cancelling reservation...";
                StatusMessage = "Reservation cancelled successfully.";
                NavigateToHome();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error cancelling reservation: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void NavigateToHome()
        {
            StatusMessage = "Navigating to Home...";
        }

        private void PrintConfirmation()
        {
            try
            {
                IsBusy = true;
                StatusMessage = "Preparing confirmation for printing...";
                StatusMessage = "Confirmation sent to printer.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error printing confirmation: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}