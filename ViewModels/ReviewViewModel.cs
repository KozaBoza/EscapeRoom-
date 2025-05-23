using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using EscapeRoom.Helpers;
using EscapeRoom.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;


namespace EscapeRoom.ViewModels
{
    public class ReviewViewModel:BaseViewModel
    {
        private Review _review;
        private UserViewModel _userViewModel;
        private RoomViewModel _roomViewModel;

        public ReviewViewModel()
        {
            _review = new Review();
            SubmitReviewCommand = new RelayCommand(SubmitReview, CanSubmitReview);
            DeleteReviewCommand = new RelayCommand(DeleteReview, CanDeleteReview);
        }

        public ReviewViewModel(Review review) : this()
        {
            _review = review ?? new Review();
        }

        public int Id
        {
            get => GetFieldValue<int>("_id");
            set => SetFieldValue(value, "_id");
        }

        public int UserId
        {
            get => GetFieldValue<int>("_userId");
            set => SetFieldValue(value, "_userId");
        }

        public int RoomId
        {
            get => GetFieldValue<int>("_roomId");
            set => SetFieldValue(value, "_roomId");
        }

        public int Rating
        {
            get => GetFieldValue<int>("_rating");
            set
            {
                if (value >= 1 && value <= 5 && SetFieldValue(value, "_rating"))
                {
                    OnPropertyChanged(nameof(RatingText));
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        public string Comment
        {
            get => GetFieldValue<string>("_comment");
            set
            {
                if (SetFieldValue(value, "_comment"))
                    OnPropertyChanged(nameof(IsValid));
            }
        }

        public DateTime CreatedAt
        {
            get => GetFieldValue<DateTime>("_createdAt");
            set
            {
                if (SetFieldValue(value, "_createdAt"))
                    OnPropertyChanged(nameof(CreatedAtText));
            }
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

        // obliczone 
        public string RatingText => $"{Rating}/5 ⭐";
        public string CreatedAtText => CreatedAt.ToString("dd.MM.yyyy");
        public bool IsValid => Rating >= 1 && Rating <= 5 && !string.IsNullOrWhiteSpace(Comment);

        // komendy
        public ICommand SubmitReviewCommand { get; }
        public ICommand DeleteReviewCommand { get; }

        private void SubmitReview(object parameter)
        {
            if (IsValid)
            {
                CreatedAt = DateTime.Now;
                //logika zapisywania recenzji
            }
        }

        private bool CanSubmitReview(object parameter) => IsValid;

        private void DeleteReview(object parameter)
        {
            // logika usuwania recenzji
        }

        private bool CanDeleteReview(object parameter) => Id > 0;

        //metody pomocnicze do pracy z prywatymi polami przez refleksję
        private T GetFieldValue<T>(string fieldName)
        {
            var field = typeof(Review).GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return field != null ? (T)field.GetValue(_review) : default(T);
        }

        private bool SetFieldValue<T>(T value, string fieldName)
        {
            var field = typeof(Review).GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                var currentValue = (T)field.GetValue(_review);
                if (!Equals(currentValue, value))
                {
                    field.SetValue(_review, value);
                    return true;
                }
            }
            return false;
        }
    }

}

