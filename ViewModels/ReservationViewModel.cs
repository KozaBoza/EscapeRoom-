using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EscapeRoom.Helpers;
using EscapeRoom.Models;
using System.Threading.Tasks;
 

namespace EscapeRoom.ViewModels
{
    public class ReservationViewModel : INotifyPropertyChanged
    {
        private Room _selectedRoom;
        private DateTime _selectedDate = DateTime.Today;
        private TimeSpan _selectedTime;
        private int _participantsCount = 1;
        private ObservableCollection<TimeSlot> _availableTimeSlots;
        private bool _isBusy;
        private string _statusMessage;
        private decimal _totalPrice;
        private User _currentUser;
        private string _specialRequests;
        private ObservableCollection<Room> _availableRooms;

        public ReservationViewModel()
        {
            AvailableTimeSlots = new ObservableCollection<TimeSlot>();
            AvailableRooms = new ObservableCollection<Room>();
            LoadAvailableRooms();
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
                    LoadAvailableTimeSlotsForDate();
                    CalculateTotalPrice();
                }
            }
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged();
                    LoadAvailableTimeSlotsForDate();
                }
            }
        }

        public TimeSpan SelectedTime
        {
            get => _selectedTime;
            set
            {
                if (_selectedTime != value)
                {
                    _selectedTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public int ParticipantsCount
        {
            get => _participantsCount;
            set
            {
                if (_participantsCount != value && value >= 1)
                {
                    _participantsCount = value;
                    OnPropertyChanged();
                    CalculateTotalPrice();
                }
            }
        }

        public ObservableCollection<TimeSlot> AvailableTimeSlots
        {
            get => _availableTimeSlots;
            set
            {
                if (_availableTimeSlots != value)
                {
                    _availableTimeSlots = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Room> AvailableRooms
        {
            get => _availableRooms;
            set
            {
                if (_availableRooms != value)
                {
                    _availableRooms = value;
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

        public decimal TotalPrice
        {
            get => _totalPrice;
            set
            {
                if (_totalPrice != value)
                {
                    _totalPrice = value;
                    OnPropertyChanged();
                }
            }
        }

        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                if (_currentUser != value)
                {
                    _currentUser = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SpecialRequests
        {
            get => _specialRequests;
            set
            {
                if (_specialRequests != value)
                {
                    _specialRequests = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime MinDate => DateTime.Today;
        public DateTime MaxDate => DateTime.Today.AddMonths(3);

        private ICommand _makeReservationCommand;
        public ICommand MakeReservationCommand
        {
            get
            {
                return _makeReservationCommand ?? (_makeReservationCommand = new RelayCommand(_ => MakeReservation(), _ => CanMakeReservation()));
            }
        }

        private ICommand _incrementParticipantsCommand;
        public ICommand IncrementParticipantsCommand
        {
            get
            {
                return _incrementParticipantsCommand ?? (_incrementParticipantsCommand = new RelayCommand(_ => IncrementParticipants(), _ => CanIncrementParticipants()));
            }
        }

        private ICommand _decrementParticipantsCommand;
        public ICommand DecrementParticipantsCommand
        {
            get
            {
                return _decrementParticipantsCommand ?? (_decrementParticipantsCommand = new RelayCommand(_ => DecrementParticipants(), _ => CanDecrementParticipants()));
            }
        }

        private ICommand _navigateToSummaryCommand;
        public ICommand NavigateToSummaryCommand
        {
            get
            {
                return _navigateToSummaryCommand ?? (_navigateToSummaryCommand = new RelayCommand(_ => NavigateToSummary(), _ => CanNavigateToSummary()));
            }
        }

        private async void LoadAvailableRooms()
        {
            try
            {
                IsBusy = true;
                StatusMessage = "Loading available rooms...";
                AvailableRooms.Add(new Room { Id = 1, Name = "name", Description = "opis...", MaxParticipants = 6, MinParticipants = 2, PricePerPerson = 25.00m, IsActive = true });
                if (AvailableRooms.Count > 0)
                {
                    SelectedRoom = AvailableRooms[0];
                }

                StatusMessage = string.Empty;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading rooms: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void LoadAvailableTimeSlotsForDate()
        {
            if (SelectedRoom == null || SelectedDate == DateTime.MinValue)
                return;

            try
            {
                IsBusy = true;
                StatusMessage = "Loading available time slots...";
                AvailableTimeSlots.Clear();
                DateTime startTime = SelectedDate.Date.Add(new TimeSpan(10, 0, 0)); //przykladowe
                DateTime endTime = SelectedDate.Date.Add(new TimeSpan(21, 0, 0));   
                TimeSpan interval = TimeSpan.FromHours(1.5);                     // 1.5h

                for (DateTime time = startTime; time <= endTime; time = time.Add(interval))
                {
                    if (SelectedDate.Date == DateTime.Today && time <= DateTime.Now)
                        continue;

                    AvailableTimeSlots.Add(new TimeSlot
                    {
                        Start = time.TimeOfDay,
                        End = time.Add(TimeSpan.FromHours(1)).TimeOfDay,
                        IsAvailable = true
                    });
                }

                if (AvailableTimeSlots.Count > 0)
                {
                    SelectedTime = AvailableTimeSlots[0].Start;
                }

                StatusMessage = string.Empty;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading time slots: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void CalculateTotalPrice()
        {
            if (SelectedRoom != null)
            {
                TotalPrice = ParticipantsCount * SelectedRoom.PricePerPerson;
            }
            else
            {
                TotalPrice = 0;
            }
        }

        private async void LoadCurrentUser()
        {
            try
            {
                IsBusy = true;
                CurrentUser = new User
                {
                    Id = 1,
                    FirstName = "Natalia",
                    LastName = "Tomala",
                    Email = "kozabozandsfnksj",
                    PhoneNumber = "000 000 000"
                };
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading user data: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanMakeReservation()
        {
            return !IsBusy &&
                   SelectedRoom != null &&
                   SelectedDate >= MinDate &&
                   SelectedDate <= MaxDate &&
                   SelectedTime != TimeSpan.Zero &&
                   ParticipantsCount >= SelectedRoom.MinParticipants &&
                   ParticipantsCount <= SelectedRoom.MaxParticipants;
        }

        private async void MakeReservation()
        {
            if (!CanMakeReservation())
                return;

            try
            {
                IsBusy = true;
                StatusMessage = "Creating reservation...";
                var reservation = new Reservation
                {
                    RoomId = SelectedRoom.Id,
                    UserId = CurrentUser?.Id ?? 0,
                    ReservationDate = SelectedDate,
                    StartTime = SelectedTime,
                    EndTime = SelectedTime.Add(TimeSpan.FromHours(1)),
                    ParticipantsCount = ParticipantsCount,
                    TotalPrice = TotalPrice,
                    SpecialRequests = SpecialRequests,
                    Status = "Pending", 
                    CreatedDate = DateTime.Now
                };

                StatusMessage = "Reservation created successfully!";
                NavigateToSummary();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error creating reservation: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanIncrementParticipants()
        {
            return SelectedRoom != null && ParticipantsCount < SelectedRoom.MaxParticipants;
        }

        private void IncrementParticipants()
        {
            if (CanIncrementParticipants())
            {
                ParticipantsCount++;
            }
        }

        private bool CanDecrementParticipants()
        {
            return SelectedRoom != null && ParticipantsCount > SelectedRoom.MinParticipants;
        }

        private void DecrementParticipants()
        {
            if (CanDecrementParticipants())
            {
                ParticipantsCount--;
            }
        }

        private bool CanNavigateToSummary()
        {
            return CanMakeReservation();
        }

        private void NavigateToSummary()
        {
            StatusMessage = "Navigating to summary...";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class TimeSlot
    {
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public bool IsAvailable { get; set; }

        public override string ToString()
        {
            return $"{Start.ToString(@"hh\:mm")} - {End.ToString(@"hh\:mm")}";
        }
    }
}