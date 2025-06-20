﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using EscapeRoom.Data;
using EscapeRoom.Helpers;
using EscapeRoom.Models;
using EscapeRoom.Services;
using System.ComponentModel;
using System.Threading.Tasks; // Added for async operations

namespace EscapeRoom.ViewModels
{
    // podstrona dot. listy pokojów
    public class RoomViewModel : BaseViewModel
    {
     
        private RoomViewModel _selectedRoom;

      
        public ObservableCollection<RoomViewModel> Rooms { get; set; }

        public bool IsLoggedIn => UserSession.IsLoggedIn;

        public RoomViewModel()
        {
            Rooms = new ObservableCollection<RoomViewModel>();
            BookRoomCommand = new RelayCommand(BookRoom, CanBookRoom);
            ShowReviewsCommand = new RelayCommand(ShowReviews); 
            LoadRoomsAsync();
        }


        public RoomViewModel SelectedRoom
        {
            get => _selectedRoom;
            set => SetProperty(ref _selectedRoom, value);
        }

        private async void LoadRoomsAsync()
        {
            try
            {
                DataService service = new DataService();
                var roomsFromDb = await service.GetRoomsAsync();

                if (roomsFromDb == null || roomsFromDb.Count == 0)
                {
                    MessageBox.Show("Nie wczytano danych z bazy!", "Błąd!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                foreach (var room in roomsFromDb)
                {
                    Rooms.Add(new RoomViewModel(room));
                }

            

                ((RelayCommand)BookRoomCommand).RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania pokoi: {ex.Message}", "Błąd krytyczny", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private Room _currentRoomData;

        public RoomViewModel(Room room)
        {
            _currentRoomData = room;
          
            BookRoomCommand = new RelayCommand(BookRoom, CanBookRoom); 
            ShowReviewsCommand = new RelayCommand(ShowReviews); 
        }

        // cechy
        public int PokojId => _currentRoomData?.PokojId ?? 0;
        public string Nazwa => _currentRoomData?.Nazwa;
        public string Opis => _currentRoomData?.Opis;
        public byte Trudnosc => _currentRoomData?.Trudnosc ?? 0;
        public decimal Cena => _currentRoomData?.Cena ?? 0m;
        public byte MaxGraczy => _currentRoomData?.MaxGraczy ?? 0;
        public int CzasMinut => _currentRoomData?.CzasMinut ?? 0;

        public string TrudnoscText => $"Poziom trudności: {Trudnosc}/5";
        public string CenaText => $"{Cena:C}";
        public string CzasText => $"{CzasMinut} minut";
        public string MaxGraczyText => $"Maksymalnie {MaxGraczy} graczy";

       

        public ICommand BookRoomCommand { get; }
        public ICommand ShowReviewsCommand { get; } 

        private void BookRoom(object parameter)
        {
            if (!IsLoggedIn)
            {
                MessageBox.Show("Aby zarezerwować pokój, musisz być zalogowany.", "Wymagane logowanie", MessageBoxButton.OK, MessageBoxImage.Information);
                ViewNavigationService.Instance.NavigateTo(ViewType.Login);
                return;
            }

            
            if (parameter is RoomViewModel roomToBook)
            {
                
                var reservationViewModel = new ReservationViewModel(roomToBook._currentRoomData); 
                ViewNavigationService.Instance.NavigateTo(ViewType.ReservationForm, reservationViewModel);
            }
        }

        private bool CanBookRoom(object parameter)
        {
            
            return IsLoggedIn;
        }

        private void ShowReviews(object parameter)
        {
            if (parameter is RoomViewModel roomToShowReviews)
            {
                MessageBox.Show($"Wyświetlam opinie dla pokoju: {roomToShowReviews.Nazwa}", "Opinie o pokoju", MessageBoxButton.OK, MessageBoxImage.Information);
              
                // ex: ViewNavigationService.Instance.NavigateTo(ViewType.RoomReviews, roomToShowReviews);
            }
        }
    }
}