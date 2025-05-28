using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using EscapeRoom.ViewModels;
using EscapeRoom.Models;
using System.Windows.Media;
using EscapeRoom.Services;
using System.Windows;

namespace EscapeRoom.Views
{ //obsługa przycisków
    public partial class ReservationFormView:UserControl
    {
        public ReservationFormView()
        {
            InitializeComponent();
            InitializeDatePicker();
        }
        private void InitializeDatePicker()
        {
            var datePicker = this.FindName("ReservationDatePicker") as DatePicker;
            if (datePicker != null)
            {
                datePicker.DisplayDateStart = DateTime.Today;
            }
        }

        private void OnSubmitButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (ValidateReservationForm())
                {
                    var result = System.Windows.MessageBox.Show(
                        "Czy chcesz przejść do płatności?",
                        "Potwierdzenie rezerwacji",
                        System.Windows.MessageBoxButton.YesNo,
                        System.Windows.MessageBoxImage.Question);

                    if (result == System.Windows.MessageBoxResult.Yes)
                    {
                        NavigationService.Instance.NavigateTo(ViewType.Payment);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Błąd podczas przetwarzania rezerwacji: {ex.Message}");
            }
        }

        private void OnBackButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Instance.NavigateTo(ViewType.Homepage);
        }

        private bool ValidateReservationForm()
        {
            //walidacje
            string name = GetTextBoxValue("NameTextBox");
            if (string.IsNullOrWhiteSpace(name))
            {
                ShowErrorMessage("Proszę podać imię i nazwisko.");
                return false;
            }
            string email = GetTextBoxValue("EmailTextBox");
            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
            {
                ShowErrorMessage("Proszę podać prawidłowy adres email.");
                return false;
            }

            string phone = GetTextBoxValue("PhoneTextBox");
            if (string.IsNullOrWhiteSpace(phone))
            {
                ShowErrorMessage("Proszę podać numer telefonu.");
                return false;
            }

            var datePicker = this.FindName("ReservationDatePicker") as DatePicker;
            if (datePicker?.SelectedDate == null || datePicker.SelectedDate < DateTime.Today)
            {
                ShowErrorMessage("Proszę wybrać prawidłową datę rezerwacji.");
                return false;
            }

            string peopleCount = GetTextBoxValue("PeopleCountTextBox");
            if (!int.TryParse(peopleCount, out int count) || count < 1 || count > 8)
            {
                ShowErrorMessage("Liczba osób musi być z zakresu 1-8.");
                return false;
            }

            var roomComboBox = this.FindName("RoomComboBox") as ComboBox;
            if (roomComboBox?.SelectedItem == null)
            {
                ShowErrorMessage("Proszę wybrać pokój.");
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private string GetTextBoxValue(string textBoxName)
        {
            var textBox = this.FindName(textBoxName) as TextBox;
            return textBox?.Text ?? string.Empty;
        }

        private void ShowErrorMessage(string message)
        {
            System.Windows.MessageBox.Show(message, "Błąd rezerwacji",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
        }
    }
}
