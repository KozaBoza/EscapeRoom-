using System;
using System.Text.RegularExpressions;
using System.Windows.Input;
using EscapeRoom.Helpers;
using EscapeRoom.Models;
using System.Windows;



namespace EscapeRoom.ViewModels
{ //opcjonalnie: social media
    public class ContactViewModel : BaseViewModel
    {
        private Contact _contact;
        private string _errorMessage;
        private string _statusMessage;
        private bool _canSend;
        public bool IsUserLoggedIn => UserSession.CurrentUser == null;


        public ContactViewModel()
        {
            _contact = new Contact();
            SubmitCommand = new RelayCommand(SubmitContact, CanSubmitContact);
            if (UserSession.CurrentUser != null)
            {
                Name = $"{UserSession.CurrentUser.Imie} {UserSession.CurrentUser.Nazwisko}";
                Email = UserSession.CurrentUser.Email;
            }
        }

        public ContactViewModel(Contact contact) : this()
        {
            _contact = contact ?? new Contact();
        }

        public string Name
        {
            get => _contact.Name;
            set
            {
                if (_contact.Name != value)
                {
                    _contact.Name = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanSubmit));
                }
            }
        }

        public string Email
        {
            get => _contact.Email;
            set
            {
                if (_contact.Email != value)
                {
                    _contact.Email = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanSubmit));
                }
            }
        }

        public string Message
        {
            get => _contact.Message;
            set
            {
                if (_contact.Message != value)
                {
                    _contact.Message = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanSubmit));
                }
            }
        }


        public DateTime SubmittedAt
        {
            get => GetFieldValue<DateTime>("_submittedAt");
            set => SetFieldValue(value, "_submittedAt");
        }

        public bool CanSubmit =>
            !string.IsNullOrWhiteSpace(Name) &&
            IsValidEmail(Email) &&
            !string.IsNullOrWhiteSpace(Message);

        public ICommand SubmitCommand { get; }

        private void SubmitContact(object parameter)
        {
            SubmittedAt = DateTime.Now;
            // tutaj można dodać logikę: zapis do bazy, wysyłanie maila...
        }

        private bool CanSubmitContact(object parameter) => CanSubmit;

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private T GetFieldValue<T>(string fieldName)
        {
            var field = typeof(Contact).GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return field != null ? (T)field.GetValue(_contact) : default(T);
        }

        private bool SetFieldValue<T>(T value, string fieldName)
        {
            var field = typeof(Contact).GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                var currentValue = (T)field.GetValue(_contact);
                if (!Equals(currentValue, value))
                {
                    field.SetValue(_contact, value);
                    return true;
                }
            }
            return false;
        }

        public Contact GetContact() => _contact;
    }
}
