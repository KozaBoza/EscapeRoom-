using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using EscapeRoom.Data;
using EscapeRoom.Helpers;
using EscapeRoom.Models;
using EscapeRoom.Services;
using System.ComponentModel;

namespace EscapeRoom.ViewModels
{
    // podstrona dot. pojedynczego pokoju
    public class RoomViewModel : BaseViewModel
    {
        private Room _room;
        private bool _isSelected;

        public ObservableCollection<Room> Rooms { get; set; }

        public bool IsLoggedIn => UserSession.IsLoggedIn;

        public RoomViewModel()
        {
            _room = new Room();
            Rooms = new ObservableCollection<Room>();
            BookRoomCommand = new RelayCommand(BookRoom, CanBookRoom);
            LoadRoomsAsync();
        }

        public RoomViewModel(Room room) : this()
        {
            _room = room ?? new Room();
            OnPropertyChanged(nameof(Nazwa));
            OnPropertyChanged(nameof(Opis));
            OnPropertyChanged(nameof(Trudnosc));
            OnPropertyChanged(nameof(Cena));
            OnPropertyChanged(nameof(MaxGraczy));
            OnPropertyChanged(nameof(CzasMinut));
        }

        private async void LoadRoomsAsync()
        {
            try
            {
                DataService service = new DataService();
                var rooms = await service.GetRoomsAsync();

                if (rooms == null || rooms.Count == 0)
                {
                    MessageBox.Show("Nie wczytano danych z bazy!", "Błąd!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                foreach (var room in rooms)
                {
                    Rooms.Add(room);
                }

                _room = Rooms.FirstOrDefault(r => r.PokojId == 1) ?? new Room();

                OnPropertyChanged(nameof(Nazwa));
                OnPropertyChanged(nameof(Opis));
                OnPropertyChanged(nameof(Trudnosc));
                OnPropertyChanged(nameof(Cena));
                OnPropertyChanged(nameof(MaxGraczy));
                OnPropertyChanged(nameof(CzasMinut));

                ((RelayCommand)BookRoomCommand).RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania pokoi: {ex.Message}", "Błąd krytyczny", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public int PokojId
        {
            get => _room.PokojId;
            set
            {
                if (_room.PokojId != value)
                {
                    _room.PokojId = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Nazwa
        {
            get => _room.Nazwa;
            set
            {
                if (_room.Nazwa != value)
                {
                    _room.Nazwa = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Opis
        {
            get => _room.Opis;
            set
            {
                if (_room.Opis != value)
                {
                    _room.Opis = value;
                    OnPropertyChanged();
                }
            }
        }

        public byte Trudnosc
        {
            get => _room.Trudnosc;
            set
            {
                if (_room.Trudnosc != value)
                {
                    _room.Trudnosc = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TrudnoscText));
                }
            }
        }

        public decimal Cena
        {
            get => _room.Cena;
            set
            {
                if (_room.Cena != value)
                {
                    _room.Cena = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CenaText));
                }
            }
        }

        public byte MaxGraczy
        {
            get => _room.MaxGraczy;
            set
            {
                if (_room.MaxGraczy != value)
                {
                    _room.MaxGraczy = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(MaxGraczyText));
                }
            }
        }

        public int CzasMinut
        {
            get => _room.CzasMinut;
            set
            {
                if (_room.CzasMinut != value)
                {
                    _room.CzasMinut = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CzasText));
                }
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public string TrudnoscText => $"Poziom trudności: {Trudnosc}/5";
        public string CenaText => $"{Cena:C}";
        public string CzasText => $"{CzasMinut} minut";
        public string MaxGraczyText => $"Maksymalnie {MaxGraczy} graczy";

        public Room GetRoom() => _room;

        public ICommand BookRoomCommand { get; }

        private void BookRoom(object parameter)
        {
            // Dodaj tutaj warunek sprawdzający IsLoggedIn ponownie, dla pewności (choć CanBookRoom już to robi)
            if (!IsLoggedIn)
            {
                MessageBox.Show("Aby zarezerwować pokój, musisz być zalogowany.", "Wymagane logowanie", MessageBoxButton.OK, MessageBoxImage.Information);
                ViewNavigationService.Instance.NavigateTo(ViewType.Login); // Przekieruj do logowania
                return;
            }

            // Utwórz ReservationViewModel i przekaż mu aktualny RoomViewModel
            var reservationViewModel = new ReservationViewModel(this); // Przekazujemy 'this', czyli RoomViewModel
            ViewNavigationService.Instance.NavigateTo(ViewType.ReservationForm, reservationViewModel);
        }
        private bool CanBookRoom(object parameter)
        {
            // Przycisk "Zarezerwuj" jest aktywny tylko, jeśli użytkownik jest zalogowany
            return IsLoggedIn;
        }
    }
}
