using System;
using System.ComponentModel;

namespace EscapeRoom.Services
{
    public enum ViewType
    {
        Homepage,
        Login,
        Register,
        AdminDashboard,
        ReservationForm,
        ReservationHistory,
        Contact,
        Payment,
        Room,
        Review,
        User
    }

    public class ViewNavigationService : INotifyPropertyChanged
    {
        private static ViewNavigationService _instance;
        public static ViewNavigationService Instance => _instance ?? (_instance = new ViewNavigationService());

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

        private object _currentParameter;
        public object GetNavigationParameter()
        {
            return _currentParameter;
        }

        public void NavigateTo(ViewType viewType)
        {
        if (viewType == ViewType.Homepage)
            {
                CurrentView = ViewType.Homepage;
                return;
            }

            CurrentView = viewType;
        }

        public void NavigateTo(ViewType viewType, object parameter)
        {
            _currentParameter = parameter;
            NavigateTo(viewType);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}