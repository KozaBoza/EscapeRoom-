using EscapeRoom.Helpers;
using EscapeRoom.Models;
using EscapeRoom.Data;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EscapeRoom.ViewModels
{
    public class ReservationHistoryViewModel : BaseViewModel
    {
        private ObservableCollection<ReservationViewModel> _userReservations;

        public ObservableCollection<ReservationViewModel> UserReservations
        {
            get => _userReservations;
            set => SetProperty(ref _userReservations, value);
        }

        public ReservationHistoryViewModel()
        {
            LoadUserReservationsAsync();
        }

        private async void LoadUserReservationsAsync()
        {
            if (!UserSession.IsLoggedIn || string.IsNullOrEmpty(UserSession.CurrentUser.Email))
                return;

            var dataService = new DataService();
            var allReservations = await dataService.GetAllReservationsAsync(); //baza danych
            var userReservations = allReservations
                .Where(r => r.Uzytkownik?.Email == UserSession.CurrentUser.Email)
                .OrderByDescending(r => r.Data)
                .ToList();

            var reservationVMs = new ObservableCollection<ReservationViewModel>();
            foreach (var res in userReservations)
            {
                var vm = new ReservationViewModel(res);
                reservationVMs.Add(vm);
            }

            UserReservations = reservationVMs;
        }
    }
}
