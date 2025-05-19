using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using EscapeRoom.Helpers;
using EscapeRoom.Models;

namespace EscapeRoom.ViewModels
{
    internal class AdminDashboard : INotifyPropertyChanged
    {   //do zmiany jeszcze
        private ObservableCollection<Room> _rooms;
        private ObservableCollection<Reservation> _recentReservations;
        private ObservableCollection<User> _recentUsers;
        private int _totalRooms;
        private int _activeRooms;
        private int _pendingReservations;
        private int _confirmedReservations;
        private int _totalUsers;
        private decimal _totalRevenue;
        private bool _isBusy;
        private string _statusMessage;


      
    }

}