using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EscapeRoom.Data;
using EscapeRoom.Helpers;
using EscapeRoom.Models;

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
        private ReviewViewModel _selectedReview;

        public ReviewViewModel()
        {
            _review = new Review
            {
                DataUtworzenia = DateTime.Now  // Set current date in constructor
            };
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
            get => _review?.Opinia ?? "";
            set
            {
                if (_review != null)
                {
                    _review.Opinia = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        public DateTime DataUtworzenia
        {
            get => _review?.DataUtworzenia ?? DateTime.Now;  // Return current date if null
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

        public ReviewViewModel SelectedReview
        {
            get => _selectedReview;
            set
            {
                if (SetProperty(ref _selectedReview, value))
                {
                    OnPropertyChanged(nameof(CanDeleteReview));
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
        public bool IsValid => !string.IsNullOrWhiteSpace(Komentarz) &&
                              CurrentRoom != null &&
                              UserSession.IsLoggedIn;
        public bool HasRoomSelected => CurrentRoom != null;
        public string UserName => UserViewModel?.FullName ?? "Użytkownik";

        public bool IsLoggedIn => UserSession.IsLoggedIn;

        public bool IsAdmin => UserSession.IsLoggedIn && UserSession.CurrentUser?.Admin == true;

        public bool CanDeleteReview
        {
            get => UserSession.IsLoggedIn &&
                   UserSession.CurrentUser?.Admin == true && // Only admin can delete
                   SelectedReview != null; // Must have a review selected
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

        private async void SubmitReview(object parameter)
        {
            try
            {
                if (!UserSession.IsLoggedIn)
                {
                    MessageBox.Show("Musisz być zalogowany, aby dodać recenzję.",
                        "Logowanie wymagane", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(Komentarz))
                {
                    MessageBox.Show("Proszę wprowadzić treść recenzji.",
                        "Walidacja", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var review = new Review
                {
                    UzytkownikId = UserSession.CurrentUser.UzytkownikId,
                    PokojId = CurrentRoom.PokojId,
                    Opinia = Komentarz,
                    DataUtworzenia = DateTime.Now
                };

                bool success = await _dataService.AddReviewAsync(review);

                if (success)
                {
                    MessageBox.Show("Recenzja została dodana pomyślnie.",
                        "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                    Komentarz = "";
                    await LoadReviewsForRoomAsync();
                }
                else
                {
                    MessageBox.Show("Nie udało się dodać recenzji.",
                        "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas dodawania recenzji: {ex.Message}",
                    "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanSubmitReview(object parameter)
        {
            return IsValid;
        }

        private async void DeleteReview(object parameter)
        {
            try
            {
                if (!UserSession.IsLoggedIn || !UserSession.CurrentUser.Admin)
                {
                    MessageBox.Show("Tylko administrator może usuwać opinie.",
                        "Brak uprawnień", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (SelectedReview == null)
                {
                    MessageBox.Show("Wybierz opinię do usunięcia.",
                        "Walidacja", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show(
                    "Czy na pewno chcesz usunąć tę opinię?",
                    "Potwierdzenie",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    bool success = await _dataService.DeleteReviewAsync(SelectedReview.RecenzjaId);

                    if (success)
                    {
                        MessageBox.Show("Opinia została usunięta.",
                            "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                        Komentarz = "";
                        DataUtworzenia = DateTime.Now;
                        SelectedReview = null;
                        await LoadReviewsForRoomAsync(); // Refresh the list
                    }
                    else
                    {
                        MessageBox.Show("Nie udało się usunąć opinii.",
                            "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas usuwania opinii: {ex.Message}",
                    "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadReviewsForRoomAsync()
        {
            try
            {
                if (CurrentRoom == null) return;

                var reviews = await _dataService.GetReviewsForRoomAsync(CurrentRoom.PokojId);
                ReviewsForRoom.Clear();

                foreach (var review in reviews)
                {
                    var reviewVM = new ReviewViewModel(review)
                    {
                        UserViewModel = new UserViewModel(review.User),
                        RoomName = CurrentRoom.Nazwa
                    };
                    ReviewsForRoom.Add(reviewVM);
                }

                RoomName = CurrentRoom.Nazwa;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas ładowania recenzji: {ex.Message}",
                    "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}