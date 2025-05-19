using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EscapeRoom.Models
{
    public class Room
    {
        private int _id; //-- do ustalenia czy robimy id
        private string _name;
        private string _description;
        private int _difficulty; // 1-5 poziom trudnosci
        private int _maxParticipants;
        private int _durationMinutes;
        private decimal _price;
        private string _imageUrl;
        private bool _isActive;
        private ObservableCollection<Review> _reviews;
        private double _averageRating;


    }
}