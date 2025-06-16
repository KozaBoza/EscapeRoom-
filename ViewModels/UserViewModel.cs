using System;
using System.Windows.Input;
using EscapeRoom.Helpers;
using EscapeRoom.Models;

namespace EscapeRoom.ViewModels
{ // podstrona dot. pojedynczego użytkownika
    public class UserViewModel : BaseViewModel
    {
        private User _user;
        private string _passwordConfirm;
        private bool _isEditing;

        public UserViewModel()
        {
            _user = new User();
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
            EditCommand = new RelayCommand(Edit);
        }

        public UserViewModel(User user) : this()
        {
            _user = user ?? new User();
        }

        public int UzytkownikId
        {
            get => _user.UzytkownikId;
            set
            {
                if (_user.UzytkownikId != value)
                {
                    _user.UzytkownikId = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Email
        {
            get => _user.Email;
            set
            {
                if (_user.Email != value)
                {
                    _user.Email = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        public string HasloHash
        {
            get => _user.HasloHash;
            set
            {
                if (_user.HasloHash != value)
                {
                    _user.HasloHash = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Imie
        {
            get => _user.Imie;
            set
            {
                if (_user.Imie != value)
                {
                    _user.Imie = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        public string Nazwisko
        {
            get => _user.Nazwisko;
            set
            {
                if (_user.Nazwisko != value)
                {
                    _user.Nazwisko = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        public string Telefon
        {
            get => _user.Telefon;
            set
            {
                if (_user.Telefon != value)
                {
                    _user.Telefon = value;
                    OnPropertyChanged();
                }
            }
        }


        public bool Admin
        {
            get => _user.Admin;
            set
            {
                if (_user.Admin != value)
                {
                    _user.Admin = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PasswordConfirm
        {
            get => _passwordConfirm;
            set
            {
                if (_passwordConfirm != value)
                {
                    _passwordConfirm = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        public bool IsEditing
        {
            get => _isEditing;
            set => SetProperty(ref _isEditing, value);
        }

        public bool IsValid =>
            !string.IsNullOrWhiteSpace(Email) &&
            !string.IsNullOrWhiteSpace(Imie) &&
            !string.IsNullOrWhiteSpace(Nazwisko) &&
            Email.Contains("@");

        public User GetUser() => _user;

        // komendy
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand EditCommand { get; }

        private void Save(object parameter)
        {
            if (IsValid)
            {
                IsEditing = false;
                // logika zapisu do bazy danych DODAĆ
            }
        }

        private bool CanSave(object parameter) => IsValid && IsEditing;

        private void Cancel(object parameter)
        {
            IsEditing = false;
        }

        private void Edit(object parameter)
        {
            IsEditing = true;
        }
    }
}
