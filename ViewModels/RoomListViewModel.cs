using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EscapeRoom.Models;
using EscapeRoom.Helpers;
using EscapeRoom.Services;

namespace EscapeRoom.ViewModels
{
    internal class RoomListViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Room> _rooms;
        private Room _selectedRoom;
        private bool _isLoading;
        private string _searchQuery;
        private int _selectedDifficulty;
        private int _maxPrice;
        private int _minPrice;
        private int _currentPrice;

        public ObservableCollection<Room> Rooms
        {
            get => _rooms;
            set
            {
                if (_rooms != value)
                {
                    _rooms = value;
                    OnPropertyChanged();
                }
            }
        }

        public Room SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                if (_selectedRoom != value)
                {
                    _selectedRoom = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (_searchQuery != value)
                {
                    _searchQuery = value;
                    OnPropertyChanged();
                    ApplyFilters();
                }
            }
        }

        public int SelectedDifficulty
        {
            get => _selectedDifficulty;
            set
            {
                if (_selectedDifficulty != value)
                {
                    _selectedDifficulty = value;
                    OnPropertyChanged();
                    ApplyFilters();
                }
            }
        }

        public int MaxPrice
        {
            get => _maxPrice;
            set
            {
                if (_maxPrice != value)
                {
                    _maxPrice = value;
                    OnPropertyChanged();
                }
            }
        }

        public int MinPrice
        {
            get => _minPrice;
            set
            {
                if (_minPrice != value)
                {
                    _minPrice = value;
                    OnPropertyChanged();
                }
            }
        }

        public int CurrentPrice
        {
            get => _currentPrice;
            set
            {
                if (_currentPrice != value)
                {
                    _currentPrice = value;
                    OnPropertyChanged();
                    ApplyFilters();
                }
            }
        }

        public ICommand ViewRoomDetailsCommand { get; }
        public ICommand ResetFiltersCommand { get; }
        public ICommand ApplyFiltersCommand { get; }
        public ICommand BookRoomCommand { get; }

        public event EventHandler<RoomSelectionEventArgs> RoomSelected;

        public RoomListViewModel()
        {
            Rooms = new ObservableCollection<Room>();
            _allRooms = new List<Room>();

            MinPrice = 0;
            MaxPrice = 300;
            CurrentPrice = MaxPrice;
            SelectedDifficulty = 0; 

            ViewRoomDetailsCommand = new RelayCommand(param => ExecuteViewRoomDetails(param));
            ResetFiltersCommand = new RelayCommand(param => ExecuteResetFilters());
            ApplyFiltersCommand = new RelayCommand(param => ApplyFilters());
            BookRoomCommand = new RelayCommand(param => ExecuteBookRoom(param));

            LoadRooms();
        }

        private List<Room> _allRooms;

        private async void LoadRooms()
        {
            IsLoading = true;

            try
            {
                await Task.Delay(500); //
                ApplyFilters();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading rooms: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

       

        private void ApplyFilters()
        {
            if (_allRooms == null)
                return;

            IEnumerable<Room> filteredRooms = _allRooms;

            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                string query = SearchQuery.ToLower();
                filteredRooms = filteredRooms.Where(r =>
                    r.Name.ToLower().Contains(query) ||
                    r.Description.ToLower().Contains(query));
            }

            if (SelectedDifficulty > 0)
            {
                filteredRooms = filteredRooms.Where(r => r.Difficulty == SelectedDifficulty);
            }

            filteredRooms = filteredRooms.Where(r => (int)r.Price <= CurrentPrice);

            Rooms.Clear();
            foreach (var room in filteredRooms)
            {
                Rooms.Add(room);
            }
        }

        private void ExecuteViewRoomDetails(object parameter)
        {
            if (parameter is Room room)
            {
                SelectedRoom = room;
                RoomSelected?.Invoke(this, new RoomSelectionEventArgs(room.Id));
            }
        }

        private void ExecuteResetFilters()
        {
            SearchQuery = string.Empty;
            SelectedDifficulty = 0;
            CurrentPrice = MaxPrice;
        }

        private void ExecuteBookRoom(object parameter)
        {
            if (parameter is Room room)
            {
  }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RoomSelectionEventArgs : EventArgs
    {
        public int RoomId { get; }

        public RoomSelectionEventArgs(int roomId)
        {
            RoomId = roomId;
        }
    }
}