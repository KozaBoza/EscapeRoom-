using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EscapeRoom.Helpers;
using EscapeRoom.Models;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace EscapeRoom.ViewModels
{
    public class RoomViewModel:BaseViewModel
    {
        private Room _room;
        private bool _isSelected;

        public RoomViewModel()
        {
            _room = new Room();
            BookRoomCommand = new RelayCommand(BookRoom, CanBookRoom);
        }

        public RoomViewModel(Room room) : this()
        {
            _room = room ?? new Room();
        }

        public int PokojId
        {
            get => _room.PokojId;
            set => SetProperty(ref _room.PokojId, value);
        }

        public string Nazwa
        {
            get => _room.Nazwa;
            set => SetProperty(ref _room.Nazwa, value);
        }

        public string Opis
        {
            get => _room.Opis;
            set => SetProperty(ref _room.Opis, value);
        }

        public byte Trudnosc
        {
            get => _room.Trudnosc;
            set => SetProperty(ref _room.Trudnosc, value);
        }

        public decimal Cena
        {
            get => _room.Cena;
            set => SetProperty(ref _room.Cena, value);
        }

        public byte MaxGraczy
        {
            get => _room.MaxGraczy;
            set => SetProperty(ref _room.MaxGraczy, value);
        }

        public int CzasMinut
        {
            get => _room.CzasMinut;
            set => SetProperty(ref _room.CzasMinut, value);
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
            // 
        }

        private bool CanBookRoom(object parameter) => true;
    }
}
