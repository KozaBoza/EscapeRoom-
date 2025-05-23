using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EscapeRoom.Helpers;
using EscapeRoom.Models;


namespace EscapeRoom.ViewModels
{
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
            set => SetProperty(ref _user.UzytkownikId, value);
        }

        public string Email
        {
            get => _user.Email;
            set
            {
                if (SetProperty(ref _user.Email, value))
                    OnPropertyChanged(nameof(IsValid));
            }
        }

        public string HasloHash
        {
            get => _user.HasloHash;
            set => SetProperty(ref _user.HasloHash, value);
        }

        public string Imie
        {
            get => _user.Imie;
            set
            {
                if (SetProperty(ref _user.Imie, value))
                    OnPropertyChanged(nameof(IsValid));
            }
        }

        public string Nazwisko
        {
            get => _user.Nazwisko;
            set
            {
                if (SetProperty(ref _user.Nazwisko, value))
                    OnPropertyChanged(nameof(IsValid));
            }
        }

        public string Telefon
        {
            get => _user.Telefon;
            set => SetProperty(ref _user.Telefon, value);
        }

        public DateTime DataRejestracji
        {
            get => _user.DataRejestracji;
            set => SetProperty(ref _user.DataRejestracji, value);
        }

        public bool Admin
        {
            get => _user.Admin;
            set => SetProperty(ref _user.Admin, value);
        }

        public string PasswordConfirm
        {
            get => _passwordConfirm;
            set
            {
                if (SetProperty(ref _passwordConfirm, value))
                    OnPropertyChanged(nameof(IsValid));
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

        //komendy
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand EditCommand { get; }

        private void Save(object parameter)
        {
            if (IsValid)
            {
                IsEditing = false;
                //logika zapisu do bazy danych
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
}
