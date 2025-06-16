using EscapeRoom.Data;
using EscapeRoom.Helpers;
using EscapeRoom.Models;
using System;
using System.Windows.Input;
using System.Threading.Tasks;

namespace EscapeRoom.ViewModels
{ // podstrona dot. pojedynczego użytkownika
    public class UserViewModel : BaseViewModel
    {
        private User _user;
        private string _passwordConfirm;
        private bool _isEditing;
        private DataService _dataService;

        public UserViewModel()
        {
            // Pobierz dane zalogowanego użytkownika z UserSession
            _user = UserSession.CurrentUser ?? new User();
            _dataService = new DataService();

            SaveCommand = new RelayCommand(SaveAsync, CanSave);
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

        private async void SaveAsync(object parameter)
        {
            if (IsValid)
            {
                try
                {
                    // Implementacja zapisu do bazy danych
                    // Przykład, jak można to zrobić:
                    bool success = await UpdateUserInDatabase();

                    if (success)
                    {
                        // Aktualizuj UserSession, aby odzwierciedlał zmiany
                        UserSession.CurrentUser = _user;

                        IsEditing = false;
                        System.Windows.MessageBox.Show("Dane zostały pomyślnie zaktualizowane.",
                            "Sukces", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Nie udało się zaktualizować danych. Spróbuj ponownie.",
                            "Błąd", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Wystąpił błąd: {ex.Message}",
                        "Błąd", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
        }

        private async Task<bool> UpdateUserInDatabase()
        {
            // Przykładowa implementacja - do uzupełnienia zgodnie z logiką DataService
            // Ta metoda powinna wywołać odpowiednią metodę z DataService, która zaktualizuje
            // dane użytkownika w bazie danych
            try
            {
                using (var conn = new MySql.Data.MySqlClient.MySqlConnection(_dataService.GetConnectionString()))
                {
                    await conn.OpenAsync();
                    var cmd = new MySql.Data.MySqlClient.MySqlCommand(
                        "UPDATE uzytkownicy SET email = @email, imie = @imie, nazwisko = @nazwisko, " +
                        "telefon = @telefon, admin = @admin WHERE uzytkownik_id = @id", conn);

                    cmd.Parameters.AddWithValue("@email", Email);
                    cmd.Parameters.AddWithValue("@imie", Imie);
                    cmd.Parameters.AddWithValue("@nazwisko", Nazwisko);
                    cmd.Parameters.AddWithValue("@telefon", Telefon ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@admin", Admin);
                    cmd.Parameters.AddWithValue("@id", UzytkownikId);

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        private bool CanSave(object parameter) => IsValid && IsEditing;

        private void Cancel(object parameter)
        {
            // Przywróć oryginalne dane użytkownika
            if (UserSession.CurrentUser != null)
            {
                _user = new User
                {
                    UzytkownikId = UserSession.CurrentUser.UzytkownikId,
                    Email = UserSession.CurrentUser.Email,
                    HasloHash = UserSession.CurrentUser.HasloHash,
                    Imie = UserSession.CurrentUser.Imie,
                    Nazwisko = UserSession.CurrentUser.Nazwisko,
                    Telefon = UserSession.CurrentUser.Telefon,
                    Admin = UserSession.CurrentUser.Admin,
                    DataRejestracji = UserSession.CurrentUser.DataRejestracji
                };

                // Powiadom widok o zmianach
                OnPropertyChanged(nameof(UzytkownikId));
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(Imie));
                OnPropertyChanged(nameof(Nazwisko));
                OnPropertyChanged(nameof(Telefon));
                OnPropertyChanged(nameof(Admin));
            }

            IsEditing = false;
        }

        private void Edit(object parameter)
        {
            IsEditing = true;
        }
    }
}
