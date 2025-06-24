using EscapeRoom.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using EscapeRoom.Helpers;
using System.Threading.Tasks;
using EscapeRoom.Data;
using System;

namespace EscapeRoom.ViewModels
{
    public class RoomViewModel : BaseViewModel
    {
        private ObservableCollection<Room> _rooms;
        private Room _selectedRoom;
        private ReviewViewModel _reviewViewModel;

        public RoomViewModel()
        {
            Rooms = new ObservableCollection<Room>();
            LoadRoomsCommand = new RelayCommand(async param => await LoadRoomsAsync());
            ReviewViewModel = new ReviewViewModel();
        }

        public ObservableCollection<Room> Rooms
        {
            get => _rooms;
            set => SetProperty(ref _rooms, value);
        }

        public Room SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                if (SetProperty(ref _selectedRoom, value))
                {
                    ReviewViewModel.SetRoom(_selectedRoom);
                    Task.Run(async () => await ReviewViewModel.LoadReviewsForRoomAsync());
                }
            }
        }

        public ReviewViewModel ReviewViewModel
        {
            get => _reviewViewModel;
            set => SetProperty(ref _reviewViewModel, value);
        }

        public ICommand LoadRoomsCommand { get; }

        public async Task LoadRoomsAsync()
        {
            try
            {
                var dataService = new DataService();
                var roomsFromDb = await dataService.GetRoomsAsync();

                Rooms.Clear();
                foreach (var room in roomsFromDb)
                {
                    Rooms.Add(room);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd podczas ładowania pokoi: {ex.Message}");
            }
        }
    }
}
