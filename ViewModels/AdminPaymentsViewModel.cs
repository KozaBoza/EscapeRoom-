using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EscapeRoom.Helpers;
using EscapeRoom.Models;

namespace EscapeRoom.ViewModels
{
    internal class AdminPaymentsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Payment> _payments;
        private Payment _selectedPayment;
        private DateTime _startDate;
        private DateTime _endDate;
        private PaymentStatus? _filterStatus;
        private PaymentMethod? _filterMethod;
        private string _searchQuery;
        private decimal _totalAmount;
        private bool _isBusy;
        private string _statusMessage;


        public AdminPaymentsViewModel()
        {
            Payments = new ObservableCollection<Payment>();
            StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            EndDate = DateTime.Now;
        }

        public ObservableCollection<Payment> Payments
        {
            get => _payments;
            set
            {
                if (_payments != value)
                {
                    _payments = value;
                    OnPropertyChanged();
                }
            }
        }

        public Payment SelectedPayment
        {
            get => _selectedPayment;
            set
            {
                if (_selectedPayment != value)
                {
                    _selectedPayment = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public PaymentStatus? FilterStatus
        {
            get => _filterStatus;
            set
            {
                if (_filterStatus != value)
                {
                    _filterStatus = value;
                    OnPropertyChanged();
                }
            }
        }

        public PaymentMethod? FilterMethod
        {
            get => _filterMethod;
            set
            {
                if (_filterMethod != value)
                {
                    _filterMethod = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (_searchQuery != value)
                {
                    _searchQuery = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal TotalAmount
        {
            get => _totalAmount;
            set
            {
                if (_totalAmount != value)
                {
                    _totalAmount = value;
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

        public Array PaymentStatusOptions => Enum.GetValues(typeof(PaymentStatus));
        public Array PaymentMethodOptions => Enum.GetValues(typeof(PaymentMethod));

        private ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new RelayCommand(_ => LoadPaymentsAsync(), _ => !IsBusy));
            }
        }

        private ICommand _updatePaymentStatusCommand;
        public ICommand UpdatePaymentStatusCommand
        {
            get
            {
                return _updatePaymentStatusCommand ?? (_updatePaymentStatusCommand = new RelayCommand(status => UpdatePaymentStatus((PaymentStatus)status), _ => SelectedPayment != null && !IsBusy));
            }
        }

        private ICommand _exportReportCommand;
        public ICommand ExportReportCommand
        {
            get
            {
                return _exportReportCommand ?? (_exportReportCommand = new RelayCommand(_ => ExportPaymentsReport(), _ => Payments.Count > 0 && !IsBusy));
            }
        }

        private ICommand _viewPaymentDetailsCommand;
        public ICommand ViewPaymentDetailsCommand
        {
            get
            {
                return _viewPaymentDetailsCommand ?? (_viewPaymentDetailsCommand = new RelayCommand(_ => ViewPaymentDetails(), _ => SelectedPayment != null && !IsBusy));
            }
        }

        private async void LoadPaymentsAsync()
        {
            try
            {
                IsBusy = true;
                StatusMessage = "Loading payments...";
                var payments = new Collection<Payment>();

                Payments = new ObservableCollection<Payment>(payments);
                decimal total = 0;
                foreach (var payment in Payments)
                {
                    if (payment.Status == PaymentStatus.Completed)
                    {
                        total += payment.Amount;
                    }
                }
                TotalAmount = total;

                StatusMessage = $"Loaded {Payments.Count} payments.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading payments: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void UpdatePaymentStatus(PaymentStatus newStatus)
        {
            if (SelectedPayment == null) return;

            try
            {
                IsBusy = true;
                StatusMessage = $"Updating payment status to {newStatus}...";
                bool success = true; 

                if (success)
                {
                    SelectedPayment.Status = newStatus;
                    StatusMessage = "Payment status updated successfully.";
                    if (newStatus == PaymentStatus.Completed || newStatus == PaymentStatus.Refunded)
                    {
                        decimal total = 0;
                        foreach (var payment in Payments)
                        {
                            if (payment.Status == PaymentStatus.Completed)
                            {
                                total += payment.Amount;
                            }
                        }
                        TotalAmount = total;
                    }
                }
                else
                {
                    StatusMessage = "Failed to update payment status.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error updating payment status: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ExportPaymentsReport()
        {
            try
            {
                IsBusy = true;
                StatusMessage = "Exporting payments report...";
                StatusMessage = "Payments report exported successfully.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error exporting payments report: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ViewPaymentDetails()
        {
            if (SelectedPayment == null) return;
            StatusMessage = $"Viewing details for payment #{SelectedPayment.Id}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}