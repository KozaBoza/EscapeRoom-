using System;
using System.ComponentModel;

namespace EscapeRoom.Services
{
    public enum ViewType
    {
        Homepage,
        Login,
        AdminDashboard,
        ReservationForm,
        Contact,
        Payment,
        Room,
        Review,
        User
    }

    public class NavigationService : INotifyPropertyChanged
    {
        private static NavigationService _instance;
        public static NavigationService Instance => _instance =new NavigationService();

        private ViewType _currentView = ViewType.Homepage;
        public ViewType CurrentView
        {
            get => _currentView;
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    OnPropertyChanged(nameof(CurrentView));
                    ViewChanged?.Invoke(value);
                }
            }
        }

        public event Action<ViewType> ViewChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public void NavigateTo(ViewType viewType)
        {
            CurrentView = viewType;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}