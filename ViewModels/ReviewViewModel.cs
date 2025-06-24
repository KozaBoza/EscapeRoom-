using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using EscapeRoom.Helpers;
using EscapeRoom.Models;
using EscapeRoom.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;

namespace EscapeRoom.ViewModels
{
    public class ReviewViewModel : BaseViewModel
    {
        private Review _review;
        private UserViewModel _userViewModel;
        private RoomViewModel _roomViewModel;
        private Room _currentRoom;
        private ObservableCollection<ReviewViewModel> _reviewsForRoom;
        private string _roomName;
        private bool _canDeleteReview;

        public ReviewViewModel()
        {
            _review = new Review();
            ReviewsForRoom = new ObservableCollection<ReviewViewModel>();
            SubmitReviewCommand = new RelayCommand(SubmitReview, CanSubmitReview);
            DeleteReviewCommand = new RelayCommand(DeleteReview, CanDeleteReview);
            LoadRoomsCommand = new RelayCommand(async param => await LoadAvailableRoomsAsync());

            // Sprawdź uprawnienia do usuwania
            UpdateDeletePermissions();
        }

        public ReviewViewModel(Review review) : this()
        {
            _review = review ?? new Review();
            UpdateDeletePermissions();
        }

        // Konstruktor z pokojem
        public ReviewViewModel(Room room) : this()
        {
            _currentRoom = room;
            RoomName = room?.Nazwa ?? "Nieznany pokój";
            UpdateDeletePermissions();
        }

        // Właściwości Review
        public int Id
        {
            get => _review?.RecenzjaId ?? 0;
            set
            {
                if (_review != null)
                {
                    _review.RecenzjaId = value;
                    OnPropertyChanged();
                    UpdateDeletePermissions();
                }
            }
        }

        public int UserId
        {
            get => _review?.UzytkownikId ?? 0;
            set
            {
                if (_review != null)
                {
                    _review.UzytkownikId = value;
                    OnPropertyChanged();
                    UpdateDeletePermissions();
                }
            }
        }

        public int RoomId
        {
            get => _review?.PokojId ?? 0;
            set
            {
                if (_review != null)
                {
                    _review.PokojId = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Rating
        {
            get => _review?.Ocena ?? 0;
            set
            {
                if (_review != null)
                {
                    _review.Ocena = (byte)value;
                    OnPropertyChanged();
                }
            }
        }

        public string Comment
        {
            get => _review?.Komentarz ?? string.Empty;
            set
            {
                if (_review != null)
                {
                    _review.Komentarz = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        public DateTime CreatedAt
        {
            get => _review?.DataUtworzenia ?? DateTime.MinValue;
            set
            {
                if (_review != null)
                {
                    _review.DataUtworzenia = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CreatedAtText));
                }
            }
        }

        // Nowe właściwości
        public Room CurrentRoom
        {
            get => _currentRoom;
            set
            {
                if (SetProperty(ref _currentRoom, value))
                {
                    RoomName = value?.Nazwa ?? "Nieznany pokój";
                    OnPropertyChanged(nameof(HasRoomSelected));
                }
            }
        }

        public string RoomName
        {
            get => _roomName;
            set => SetProperty(ref _roomName, value);
        }

        public ObservableCollection<ReviewViewModel> ReviewsForRoom
        {
            get => _reviewsForRoom;
            set => SetProperty(ref _reviewsForRoom, value);
        }

        public bool CanDeleteReview
        {
            get => _canDeleteReview;
            set => SetProperty(ref _canDeleteReview, value);
        }

        public UserViewModel UserViewModel
        {
            get => _userViewModel;
            set => SetProperty(ref _userViewModel, value);
        }

        public RoomViewModel RoomViewModel
        {
            get => _roomViewModel;
            set => SetProperty(ref _roomViewModel, value);
        }

        // Właściwości obliczane
        public string CreatedAtText => CreatedAt != DateTime.MinValue ? CreatedAt.ToString("dd.MM.yyyy HH:mm") : "";
        public bool IsValid => !string.IsNullOrWhiteSpace(Comment);
        public bool HasRoomSelected => CurrentRoom != null;
        public string UserName => UserViewModel?.FullName ?? "Użytkownik";

        // Komendy
        public ICommand SubmitReviewCommand { get; }
        public ICommand DeleteReviewCommand { get; }
        public ICommand LoadRoomsCommand { get; }

        // Metody komend
        private void SubmitReview(object parameter)
        {
            if (IsValid && CurrentRoom != null)
            {
                CreatedAt = DateTime.Now;
                // Logika zapisywania będzie w ReviewView
            }
        }

        private bool CanSubmitReview(object parameter) => IsValid && HasRoomSelected;

        private async void DeleteReview(object parameter)
        {
            if (Id > 0)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Czy na pewno chcesz usunąć tę recenzję?",
                    "Potwierdzenie usunięcia",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await DeleteReviewAsync(Id);
                        MessageBox.Show("Recenzja została usunięta.", "Usunięto",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Błąd podczas usuwania recenzji: {ex.Message}",
                            "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        //private bool CanDeleteReview(object parameter) => (CanDeleteReview && Id) > 0;

        // Metody async do pracy z bazą danych
        public async Task LoadReviewsForRoomAsync()
        {
            if (CurrentRoom == null) return;

            try
            {
                var dataService = new DataService();
                var reviews = await dataService.GetReviewsForRoomAsync(CurrentRoom.PokojId);

                ReviewsForRoom.Clear();
                foreach (var review in reviews)
                {
                    var reviewViewModel = new ReviewViewModel(review);

                    // Załaduj dane użytkownika
                    var user = await dataService.GetUserByIdAsync(review.UzytkownikId);
                    if (user != null)
                    {
                        reviewViewModel.UserViewModel = new UserViewModel(user);
                    }

                    ReviewsForRoom.Add(reviewViewModel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania recenzji: {ex.Message}",
                    "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task AddReviewAsync(Review review)
        {
            try
            {
                var dataService = new DataService();
                await dataService.AddReviewAsync(review);
            }
            catch (Exception ex)
            {
                throw new Exception($"Nie udało się dodać recenzji: {ex.Message}");
            }
        }

        public async Task DeleteReviewAsync(int reviewId)
        {
            try
            {
                var dataService = new DataService();
                await dataService.DeleteReviewAsync(reviewId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Nie udało się usunąć recenzji: {ex.Message}");
            }
        }

        public async Task LoadAvailableRoomsAsync()
        {
            try
            {
                var dataService = new DataService();
                var rooms = await dataService.GetRoomsAsync();

               
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania pokoi: {ex.Message}",
                    "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateDeletePermissions()
        {
            CanDeleteReview = UserSession.IsLoggedIn &&
                            (UserSession.CurrentUser.Admin ||
                             (Id > 0 && UserId == UserSession.CurrentUser.UzytkownikId));
        }

        public void SetRoom(Room room)
        {
            CurrentRoom = room;
            if (room != null)
            {
                RoomId = room.PokojId;
            }
        }
    }
}