using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using EscapeRoom.Helpers;
using EscapeRoom.Models;

namespace EscapeRoom.ViewModels
{
    // podstrona dot. pojedynczego pokoju
    public class RoomViewModel : BaseViewModel
    {
        private Room _room;
        private bool _isSelected;

        public ObservableCollection<Room> Rooms { get; set; }

        public RoomViewModel()
        {

        }

        public RoomViewModel(Room room) : this()
        {
            _room = room ?? new Room();
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
            // rezerwacja
        }

        private bool CanBookRoom(object parameter) => true;
    }
}
