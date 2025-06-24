using EscapeRoom.Data;
using EscapeRoom.Helpers;
using EscapeRoom.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EscapeRoom.ViewModels
{
    public class ReviewViewModel : BaseViewModel
    {
        private Review _review;
        private Room _currentRoom;
        private string _roomName;
        private bool _canDeleteReview;
        private UserViewModel _userViewModel;
        private ObservableCollection<ReviewViewModel> _reviewsForRoom;

        public ReviewViewModel()
        {
            _review = new Review();
            ReviewsForRoom = new ObservableCollection<ReviewViewModel>();
            SubmitReviewCommand = new RelayCommand(SubmitReview, CanSubmitReview);
            DeleteReviewCommand = new RelayCommand(DeleteReview, CanDeleteReview);
        }

        public ReviewViewModel(Room room) : this()
        {
            SetRoom(room);
            LoadReviewsForRoomAsync();
        }

        public int Id
        {
            get => _review?.RecenzjaId ?? 0;
            set { if (_review != null) { _review.RecenzjaId = value; OnPropertyChanged(); UpdateDeletePermissions(); } }
        }

        public int UserId
        {
            get => _review?.UzytkownikId ?? 0;
            set { if (_review != null) { _review.UzytkownikId = value; OnPropertyChanged(); UpdateDeletePermissions(); } }
        }

        public int RoomId
        {
            get => _review?.PokojId ?? 0;
            set { if (_review != null) { _review.PokojId = value; OnPropertyChanged(); } }
        }

        public int Rating
        {
            get => _review?.Ocena ?? 0;
            set { if (_review != null) { _review.Ocena = (byte)value; OnPropertyChanged(); } }
        }

        public string Comment
        {
            get => _review?.Komentarz ?? "";
            set { if (_review != null) { _review.Komentarz = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsValid)); } }
        }

        public DateTime CreatedAt
        {
            get => _review?.DataUtworzenia ?? DateTime.MinValue;
            set { if (_review != null) { _review.DataUtworzenia = value; OnPropertyChanged(); OnPropertyChanged(nameof(CreatedAtText)); } }
        }

        public Room CurrentRoom
        {
            get => _currentRoom;
            set { if (SetProperty(ref _currentRoom, value)) { RoomName = value?.Nazwa; RoomId = value?.PokojId ?? 0; OnPropertyChanged(nameof(HasRoomSelected)); } }
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

        public ICommand SubmitReviewCommand { get; }
        public ICommand DeleteReviewCommand { get; }

        public string CreatedAtText => CreatedAt != DateTime.MinValue ? CreatedAt.ToString("dd.MM.yyyy HH:mm") : "";
        public bool IsValid => !string.IsNullOrWhiteSpace(Comment);
        public bool HasRoomSelected => CurrentRoom != null;
        public string RatingText => $"Ocena: {Rating}/5";
        public string UserName => UserViewModel?.FullName ?? "Użytkownik";

        private void SubmitReview(object parameter)
        {
            if (IsValid && HasRoomSelected)
            {
                CreatedAt = DateTime.Now;
                UserId = UserSession.CurrentUser?.UzytkownikId ?? 0;
                RoomId = CurrentRoom.PokojId;

                var dataService = new DataService();
                dataService.AddReviewAsync(_review);
                LoadReviewsForRoomAsync();
                Comment = "";
            }
        }

        private bool CanSubmitReview(object parameter) => IsValid && HasRoomSelected;

        private async void DeleteReview(object parameter)
        {
            if (Id > 0)
            {
                if (MessageBox.Show("Czy chcesz usunąć recenzję?", "Potwierdzenie", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var dataService = new DataService();
                    await dataService.DeleteReviewAsync(Id);
                    await LoadReviewsForRoomAsync();
                }
            }
        }

        private bool CanDeleteReview(object parameter) => CanDeleteReview && Id > 0;

        private async Task LoadReviewsForRoomAsync()
        {
            if (CurrentRoom == null) return;
            var dataService = new DataService();
            var reviews = await dataService.GetReviewsForRoomAsync(CurrentRoom.PokojId);

            ReviewsForRoom.Clear();
            foreach (var review in reviews)
            {
                var vm = new ReviewViewModel(review);
                var user = await dataService.GetUserByIdAsync(review.UzytkownikId);
                if (user != null) vm.UserViewModel = new UserViewModel(user);
                ReviewsForRoom.Add(vm);
            }
        }

        public ReviewViewModel(Review review) : this()
        {
            _review = review;
            Id = review.RecenzjaId;
            Comment = review.Komentarz;
            CreatedAt = review.DataUtworzenia;
            Rating = review.Ocena;
            RoomId = review.PokojId;
            UserId = review.UzytkownikId;
            UpdateDeletePermissions();
        }

        private void UpdateDeletePermissions()
        {
            CanDeleteReview = UserSession.IsLoggedIn &&
                (UserSession.CurrentUser?.Admin == true || UserId == UserSession.CurrentUser?.UzytkownikId);
        }

        public void SetRoom(Room room)
        {
            CurrentRoom = room;
        }
    }
}
