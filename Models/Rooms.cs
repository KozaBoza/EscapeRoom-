using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EscapeRoom.Models
{
    public class Room : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _description;
        private int _difficulty; // 1-5
        private int _maxParticipants;
        private int _durationMinutes;
        private decimal _price;
        private string _imageUrl;
        private bool _isActive;
        private ObservableCollection<Review> _reviews;
        private double _averageRating;

        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Difficulty
        {
            get => _difficulty;
            set
            {
                if (_difficulty != value)
                {
                    _difficulty = value;
                    OnPropertyChanged();
                }
            }
        }

        public int MaxParticipants
        {
            get => _maxParticipants;
            set
            {
                if (_maxParticipants != value)
                {
                    _maxParticipants = value;
                    OnPropertyChanged();
                }
            }
        }

        public int DurationMinutes
        {
            get => _durationMinutes;
            set
            {
                if (_durationMinutes != value)
                {
                    _durationMinutes = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                if (_imageUrl != value)
                {
                    _imageUrl = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
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
                    CalculateAverageRating();
                }
            }
        }

        public double AverageRating
        {
            get => _averageRating;
            set
            {
                if (_averageRating != value)
                {
                    _averageRating = value;
                    OnPropertyChanged();
                }
            }
        }

        // Konstruktor
        public Room()
        {
            Reviews = new ObservableCollection<Review>();
            IsActive = true;
        }

        // Metoda do obliczania średniej oceny
        private void CalculateAverageRating()
        {
            if (Reviews == null || Reviews.Count == 0)
            {
                AverageRating = 0;
                return;
            }

            double sum = 0;
            foreach (var review in Reviews)
            {
                sum += review.Rating;
            }

            AverageRating = sum / Reviews.Count;
        }

        // Implementacja INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}