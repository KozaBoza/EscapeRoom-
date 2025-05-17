using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EscapeRoom.Models
{
    public enum ReservationStatus
    {
        Pending,
        Confirmed,
        Completed,
        Cancelled
    }

    public class Reservation : INotifyPropertyChanged
    {
        private int _id;
        private int _userId;
        private int _roomId;
        private DateTime _date;
        private TimeSpan _startTime;
        private int _participantsCount;
        private ReservationStatus _status;
        private DateTime _createdAt;
        private bool _isPaid;

        private User _user;
        private Room _room;

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

        public int UserId
        {
            get => _userId;
            set
            {
                if (_userId != value)
                {
                    _userId = value;
                    OnPropertyChanged();
                }
            }
        }

        public int RoomId
        {
            get => _roomId;
            set
            {
                if (_roomId != value)
                {
                    _roomId = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Date
        {
            get => _date;
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged();
                }
            }
        }

        public TimeSpan StartTime
        {
            get => _startTime;
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public int ParticipantsCount
        {
            get => _participantsCount;
            set
            {
                if (_participantsCount != value)
                {
                    _participantsCount = value;
                    OnPropertyChanged();
                }
            }
        }

        public ReservationStatus Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set
            {
                if (_createdAt != value)
                {
                    _createdAt = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsPaid
        {
            get => _isPaid;
            set
            {
                if (_isPaid != value)
                {
                    _isPaid = value;
                    OnPropertyChanged();
                }
            }
        }

        public User User
        {
            get => _user;
            set
            {
                if (_user != value)
                {
                    _user = value;
                    OnPropertyChanged();
                }
            }
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

        //zeby wyswietlic
        public string FullDateTimeDisplay => $"{Date.ToShortDateString()} {StartTime.ToString(@"hh\:mm")}";

        
        public Reservation()
        {
            CreatedAt = DateTime.Now;
            Status = ReservationStatus.Pending;
            IsPaid = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}