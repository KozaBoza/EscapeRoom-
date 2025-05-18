using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using EscapeRoom.Models;
using EscapeRoom.Helpers;

namespace EscapeRoom.ViewModels
{
    public class RoomDetailsViewModel : INotifyPropertyChanged
    {
        private Room _room;
        private ObservableCollection<Review> _reviews;
        private string _newReviewComment;
        private int _newReviewRating;
        private bool _canReview;
        private string _statusMessage;
        private bool _isBusy;

        public RoomDetailsViewModel()
        {
            Reviews = new ObservableCollection<Review>();
            NewReviewRating = 5; 
 }

        public RoomDetailsViewModel(int roomId) : this()
        {
            LoadRoomAsync(roomId);
        }

        public Room Room
        {
            get => _room;
            set
            {
                if (_room != value)
                {
                    _room = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Review> Reviews
        {
            get => _reviews;
            set
            {
                if (_reviews != value)
                {
                    _reviews = value;
                    OnPropertyChanged();
                }
            }
        }

        public string NewReviewComment
        {
            get => _newReviewComment;
            set
            {
                if (_newReviewComment != value)
                {
                    _newReviewComment = value;
                    OnPropertyChanged();
                }
            }
        }

        public int NewReviewRating
        {
            get => _newReviewRating;
            set
            {
                if (_newReviewRating != value)
                {
                    _newReviewRating = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool CanReview
        {
            get => _canReview;
            set
            {
                if (_canReview != value)
                {
                    _canReview = value;
                    OnPropertyChanged();
                }
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage != value)
                {
                    _statusMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                }
            }
        }

        private ICommand _bookNowCommand;
        public ICommand BookNowCommand
        {
            get
            {
                return _bookNowCommand ?? (_bookNowCommand = new RelayCommand(_ => BookNow(), _ => !IsBusy && Room != null));
            }
        }

        private ICommand _submitReviewCommand;
        public ICommand SubmitReviewCommand
        {
            get
            {
                return _submitReviewCommand ?? (_submitReviewCommand = new RelayCommand(_ => SubmitReview(), _ => CanSubmitReview()));
            }
        }

        private ICommand _backToListCommand;
        public ICommand BackToListCommand
        {
            get
            {
                return _backToListCommand ?? (_backToListCommand = new RelayCommand(_ => BackToList(), _ => !IsBusy));
            }
        }



        private bool CanSubmitReview()
        {
            return !IsBusy && CanReview && NewReviewRating >= 1 && NewReviewRating <= 5 && !string.IsNullOrWhiteSpace(NewReviewComment);
        }

        private async void SubmitReview()
        {
            try
            {
                IsBusy = true;
                StatusMessage = "Submitting review...";

                var review = new Review
                {
                    RoomId = Room.Id,
                    Rating = NewReviewRating,
                    Comment = NewReviewComment,
                    CreatedAt = DateTime.Now
                };
                review.Id = Reviews.Count + 1;
                review.User = new User { Username = "CurrentUser", FirstName = "Current", LastName = "User" };
                Reviews.Insert(0, review);
                Room.Reviews = Reviews;
                NewReviewComment = "";
                NewReviewRating = 5;
                CanReview = false;

                StatusMessage = "Review submitted successfully!";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error submitting review: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void BookNow()
        {
 }

        private void BackToList()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}