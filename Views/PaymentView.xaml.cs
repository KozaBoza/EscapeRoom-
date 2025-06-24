using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using EscapeRoom.ViewModels;
using EscapeRoom.Models;
using EscapeRoom.Services;
using System.Windows;

//obsługa przycisków etc
namespace EscapeRoom.Views
{
    public partial class PaymentView : UserControl
    {
        public PaymentView()
        {
            InitializeComponent();

            // Pobierz parametr nawigacji
            var parameter = ViewNavigationService.Instance.GetNavigationParameter();
            if (parameter is ReservationViewModel reservationViewModel)
            {
                this.DataContext = new PaymentViewModel(reservationViewModel);
            }
            else
            {
                this.DataContext = new PaymentViewModel();
            }
        }

        private void OnPayButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (ValidatePaymentForm())
                {
                    var result = System.Windows.MessageBox.Show(
                        "Czy potwierdzasz płatność?",
                        "Potwierdzenie płatności",
                        System.Windows.MessageBoxButton.YesNo,
                        System.Windows.MessageBoxImage.Question);

                    if (result == System.Windows.MessageBoxResult.Yes)
                    {
                        ProcessPayment();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Błąd podczas przetwarzania płatności: {ex.Message}");
            }
        }

        private void OnBackButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewNavigationService.Instance.NavigateTo(ViewType.ReservationForm);
        }

        private bool ValidatePaymentForm()
        {
            string cardNumber = GetTextBoxValue("CardNumberTextBox");
            if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length < 16)
            {
                ShowErrorMessage("Proszę podać prawidłowy numer karty (16 cyfr).");
                return false;
            }

            string expiryDate = GetTextBoxValue("ExpiryDateTextBox");
            if (string.IsNullOrWhiteSpace(expiryDate))
            {
                ShowErrorMessage("Proszę podać datę ważności karty.");
                return false;
            }

            string cvv = GetTextBoxValue("CvvTextBox");
            if (string.IsNullOrWhiteSpace(cvv) || cvv.Length != 3)
            {
                ShowErrorMessage("Proszę podać prawidłowy kod CVV (3 cyfry).");
                return false;
            }

            string cardHolder = GetTextBoxValue("CardHolderTextBox");
            if (string.IsNullOrWhiteSpace(cardHolder))
            {
                ShowErrorMessage("Proszę podać imię i nazwisko posiadacza karty.");
                return false;
            }

            return true;
        }

        private void ProcessPayment()
        {
            System.Threading.Thread.Sleep(2000);
            System.Windows.MessageBox.Show(
                "Płatność została przetworzona pomyślnie!\nTwoja rezerwacja została potwierdzona.",
                "Sukces",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
            ViewNavigationService.Instance.NavigateTo(ViewType.Homepage);
        }

        private string GetTextBoxValue(string textBoxName)
        {
            var textBox = this.FindName(textBoxName) as TextBox;
            return textBox?.Text ?? string.Empty;
        }

        private void ShowErrorMessage(string message)
        {
            System.Windows.MessageBox.Show(message, "Błąd",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
        }
        private void OnProcessPaymentButtonClick(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ReservationViewModel;
            if (vm != null && vm.ProcessPaymentCommand.CanExecute(null))
            {
                vm.ProcessPaymentCommand.Execute(null);
            }
        }



    }


}