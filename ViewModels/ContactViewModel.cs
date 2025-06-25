using System;
using System.Text.RegularExpressions;
using System.Windows.Input;
using EscapeRoom.Helpers;
using EscapeRoom.Models;
using System.Windows;
using EscapeRoom.Data;



namespace EscapeRoom.ViewModels
{ //opcjonalnie: social media
    public class ContactViewModel : BaseViewModel
    {
        private Contact _contact;
        private string _errorMessage;
        private string _statusMessage;
        private bool _canSend;
        public bool IsUserLoggedIn => UserSession.CurrentUser == null;
        public string LoginMessage => "Musisz być zalogowany aby wysłać wiadomość";

        private DataService _dataService;


        public ContactViewModel()
        {
            _contact = new Contact();
            _dataService = new DataService();

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

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public bool CanSend
        {
            get => _canSend;
            set
            {
                _canSend = value;
                OnPropertyChanged();
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
            !string.IsNullOrWhiteSpace(Message) &&
            !IsUserLoggedIn;

        public ICommand SubmitCommand { get; }

        private async void SubmitContact(object parameter)
        {
            if (!CanSubmit)
            {
                ErrorMessage = "Uzupełnij wszystkie pola poprawnie.";
                return;
            }

            try
            {
                SubmittedAt = DateTime.Now;

                int? userId = UserSession.CurrentUser?.UzytkownikId;

                bool result = await _dataService.SaveContactMessageAsync(_contact.Message, userId);

                if (result)
                {
                    StatusMessage = "Wiadomość została pomyślnie wysłana.";
                    ErrorMessage = string.Empty;
                    CanSend = false;
                }
                else
                {
                    ErrorMessage = "Nie udało się zapisać wiadomości.";
                    StatusMessage = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Wystąpił błąd podczas wysyłania wiadomości.";
                StatusMessage = string.Empty;
                Console.WriteLine(ex.Message); // ewentualnie logowanie
            }
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
