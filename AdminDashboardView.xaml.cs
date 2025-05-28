using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using EscapeRoom.ViewModels;
using EscapeRoom.Models;
using System.Windows.Media;
using EscapeRoom.Services;


namespace EscapeRoom.Views
{ //trzeba obsłużyc jescze te przyciski
    public partial class AdminDashboardView : UserControl
    {
        public AdminDashboardView()
        {
            InitializeComponent();
        }
        private void OnAddRoomButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                System.Windows.MessageBox.Show("Funkcja dodawania pokoju zostanie wkrótce zaimplementowana.",
                    "Add Room", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd podczas otwierania formularza dodawania pokoju: {ex.Message}",
                    "Błąd", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
        private void OnViewReservationsButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {

            try
            {
                System.Windows.MessageBox.Show("Wyświetlanie listy rezerwacji...",
                    "Rezerwacje", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd podczas ładowania rezerwacji: {ex.Message}",
                    "Błąd", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void OnManageRoomsButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Instance.NavigateTo(ViewType.Room);
        }

        private void OnViewUsersButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Instance.NavigateTo(ViewType.User);
        }

    }
}
