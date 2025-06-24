using EscapeRoom.Helpers;
using EscapeRoom.Models;
using EscapeRoom.ViewModels;
using System.Collections.ObjectModel;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using EscapeRoom.Data;

namespace EscapeRoom.ViewModels
{
    public class ReviewViewModel : BaseViewModel
    {
        private Review _review;
        private Room _currentRoom;
        private string _roomName;
        private UserViewModel _userViewModel;
        private ObservableCollection<ReviewViewModel> _reviewsForRoom;
        private readonly DataService _dataService;

        public ReviewViewModel()
        {
            _review = new Review();
            _dataService = new DataService();
            ReviewsForRoom = new ObservableCollection<ReviewViewModel>();
            SubmitReviewCommand = new RelayCommand(SubmitReview, CanSubmitReview);
            DeleteReviewCommand = new RelayCommand(DeleteReview, param => CanDeleteReview);
        }

        public ReviewViewModel(Room room) : this()
        {
            SetRoom(room);
            LoadReviewsForRoomAsync();
        }

        public ReviewViewModel(Review review) : this()
        {
            _review = review ?? new Review();
            UpdateDeletePermissions();
        }

        public int RecenzjaId
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

        public int UzytkownikId
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

        public int PokojId
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

        public byte Ocena
        {
            get => _review?.Ocena ?? 0;
            set
            {
                if (_review != null)
                {
                    _review.Ocena = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(RatingText));
                }
            }
        }

        public string Komentarz
        {
            get => _review?.Komentarz ?? "";
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

        public DateTime DataUtworzenia
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

        public Room CurrentRoom
        {
            get => _currentRoom;
            set => SetProperty(ref _currentRoom, value);
        }

        public string RoomName
        {
            get => _roomName;
            set => SetProperty(ref _roomName, value);
        }

        public UserViewModel UserViewModel
        {
            get => _userViewModel;
            set => SetProperty(ref _userViewModel, value);
        }

        public ObservableCollection<ReviewViewModel> ReviewsForRoom
        {
            get => _reviewsForRoom;
            set => SetProperty(ref _reviewsForRoom, value);
        }

        public ICommand SubmitReviewCommand { get; }
        public ICommand DeleteReviewCommand { get; }

        public string RatingText => $"Ocena: {Ocena}/5";
        public string CreatedAtText => DataUtworzenia.ToString("dd.MM.yyyy HH:mm");
        public bool IsValid => !string.IsNullOrWhiteSpace(Komentarz);
        public bool HasRoomSelected => CurrentRoom != null;
        public string UserName => UserViewModel?.FullName ?? "Użytkownik";

        public bool CanDeleteReview
        {
            get => UserSession.IsLoggedIn &&
                (UserSession.CurrentUser?.Admin == true || UzytkownikId == UserSession.CurrentUser?.UzytkownikId);
        }

        private void UpdateDeletePermissions()
        {
            OnPropertyChanged(nameof(CanDeleteReview));
        }

        private void SetRoom(Room room)
        {
            CurrentRoom = room;
            if (room != null)
            {
                PokojId = room.PokojId;
                RoomName = room.Nazwa;
                LoadReviewsForRoomAsync();
            }
        }

        private void SubmitReview(object parameter)
        {
            try
            {
                if (!IsValid || !HasRoomSelected)
                {
                    MessageBox.Show("Proszę wprowadzić treść recenzji.", "Walidacja", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!UserSession.IsLoggedIn)
                {
                    MessageBox.Show("Musisz być zalogowany, aby dodać recenzję.", "Logowanie wymagane", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _review.DataUtworzenia = DateTime.Now;
                _review.UzytkownikId = UserSession.CurrentUser.UzytkownikId;
                _review.PokojId = CurrentRoom.PokojId;

                // Here you would typically save to database
                // await _dataService.AddReviewAsync(_review);

                MessageBox.Show("Recenzja została dodana pomyślnie.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadReviewsForRoomAsync(); // Refresh the reviews list
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas dodawania recenzji: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanSubmitReview(object parameter)
        {
            return IsValid && HasRoomSelected && UserSession.IsLoggedIn;
        }

        private async void DeleteReview(object parameter)
        {
            try
            {
                if (!CanDeleteReview)
                {
                    MessageBox.Show("Nie masz uprawnień do usunięcia tej recenzji.", "Brak uprawnień", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show("Czy na pewno chcesz usunąć tę recenzję?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    // Here you would typically delete from database
                    // await _dataService.DeleteReviewAsync(RecenzjaId);

                    MessageBox.Show("Recenzja została usunięta.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadReviewsForRoomAsync(); // Refresh the reviews list
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas usuwania recenzji: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadReviewsForRoomAsync()
        {
            try
            {
                if (CurrentRoom == null) return;

                // Here you would typically load reviews from database
                // var reviews = await _dataService.GetReviewsForRoomAsync(CurrentRoom.PokojId);
                ReviewsForRoom.Clear();
                // foreach (var review in reviews)
                // {
                //     ReviewsForRoom.Add(new ReviewViewModel(review));
                // }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas ładowania recenzji: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}