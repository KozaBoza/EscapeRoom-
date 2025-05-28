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

namespace EscapeRoom.Views
{ //obsłużyc przyciski i poprawić errormessage etc
    public partial class ContactView : UserControl
    {
        public ContactView()
        {
            InitializeComponent();
        }
        private void OnSendButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
              
                string name = GetTextBoxValue("NameTextBox");
                string email = GetTextBoxValue("EmailTextBox");
                string message = GetTextBoxValue("MessageTextBox");

                if (string.IsNullOrWhiteSpace(name))
                {
                    ShowErrorMessage("Proszę podać imię i nazwisko.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
                {
                    ShowErrorMessage("Proszę podać prawidłowy adres email.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(message))
                {
                    ShowErrorMessage("Proszę wpisać wiadomość.");
                    return;
                }

                System.Windows.MessageBox.Show("Wiadomość została wysłana pomyślnie! Odpowiemy w ciągu 24 godzin.",
                    "Sukces", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                ClearForm();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Błąd podczas wysyłania wiadomości: {ex.Message}");
            }
        }

        private void OnBackButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Instance.NavigateTo(ViewType.Homepage);
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

        private void ClearForm()
        {
            (this.FindName("NameTextBox") as TextBox)?.Clear();
            (this.FindName("EmailTextBox") as TextBox)?.Clear();
            (this.FindName("MessageTextBox") as TextBox)?.Clear();
        }
    }
}