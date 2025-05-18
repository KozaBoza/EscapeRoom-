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
using System.Windows.Controls;


namespace EscapeRoom.ViewModels
{
    internal class HomeViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Room> _featuredRooms;
        private ObservableCollection<Review> _latestReviews;
        private bool _isLoading;

        public ObservableCollection<Room> FeaturedRooms
        {
            get => _featuredRooms;
            set
            {
                if (_featuredRooms != value)
                {
                    _featuredRooms = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Review> LatestReviews
        {
            get => _latestReviews;
            set
            {
                if (_latestReviews != value)
                {
                    _latestReviews = value;
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

        public ICommand ViewAllRoomsCommand { get; }
        public ICommand BookNowCommand { get; }
        public ICommand ViewRoomDetailsCommand { get; }

        public HomeViewModel()
        {
            FeaturedRooms = new ObservableCollection<Room>();
            LatestReviews = new ObservableCollection<Review>();

            ViewAllRoomsCommand = new RelayCommand(param => ExecuteViewAllRooms());
            BookNowCommand = new RelayCommand(param => ExecuteBookNow());
            ViewRoomDetailsCommand = new RelayCommand(param => ExecuteViewRoomDetails(param));

            LoadData();
        }

        private async void LoadData()
        {
            IsLoading = true;

            try
            {
                await Task.Delay(500); // Simulate loading
                LoadDummyData();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading home data: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void LoadDummyData()
        {
            FeaturedRooms.Clear();
            FeaturedRooms.Add(new Room
            {
                Id = 1,
                Name = "nazwa",
                Description = "opis.",
                Difficulty = 
                Price = 
                DurationMinutes = 
                MaxParticipants = 
                ImageUrl = 
                AverageRating = 
            });

            LatestReviews.Clear();
        }

        private void ExecuteViewAllRooms()
        {

        }

        private void ExecuteBookNow()
        {

        }

        private void ExecuteViewRoomDetails(object parameter)
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
}