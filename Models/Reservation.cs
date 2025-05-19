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

    public class Reservation
    {
        private int _id; //wlasnosci do negocjacji
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

    }
}