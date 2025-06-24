using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using EscapeRoom.Models;
using EscapeRoom.Data;
using EscapeRoom.Helpers;

namespace EscapeRoom.ViewModels
{
    public class ReservationHistoryViewModel : BaseViewModel
    {
        private ObservableCollection<ReservationViewModel> _userReservations;
        private readonly DataService _dataService;

        public ObservableCollection<ReservationViewModel> UserReservations
        {
            get => _userReservations;
            set => SetProperty(ref _userReservations, value);
        }

        public ReservationHistoryViewModel()
        {
            _dataService = new DataService();
            UserReservations = new ObservableCollection<ReservationViewModel>();
            LoadUserReservationsAsync();
        }

        private async void LoadUserReservationsAsync()
        {
            try
            {
                if (!UserSession.IsLoggedIn || UserSession.CurrentUser == null)
                {
                    MessageBox.Show("Musisz być zalogowany, aby zobaczyć swoje rezerwacje.",
                        "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var userReservations = await _dataService.GetUserReservationsAsync(UserSession.CurrentUser.UzytkownikId);

                UserReservations.Clear();
                foreach (var reservation in userReservations)
                {
                    UserReservations.Add(new ReservationViewModel(reservation));
                }

                if (!UserReservations.Any())
                {
                    MessageBox.Show("Nie masz jeszcze żadnych rezerwacji.",
                        "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania rezerwacji: {ex.Message}",
                    "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}