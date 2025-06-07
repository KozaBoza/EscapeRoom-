using System.Windows.Controls;
using EscapeRoom.ViewModels;
using EscapeRoom.Models;
using System.Windows.Media;
using EscapeRoom.Services;
using System.Windows;
using System;

namespace EscapeRoom.Views
{  //można wkleić do xaml jakieś themowe elementy + do tego opis 
    public partial class RoomView : UserControl
    {
        public RoomView()
        {
            InitializeComponent();
            LoadRooms();
            DataContext = new RoomViewModel();
        }
        private void LoadRooms()
        {

        }

        private void OnReserveRoomButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewNavigationService.Instance.NavigateTo(ViewType.ReservationForm);
        }

        private void OnViewDetailsButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            var button = sender as Button;
            string roomName = button?.Tag?.ToString() ?? "Nieznany pokój";

            System.Windows.MessageBox.Show($"Szczegóły pokoju: {roomName}\n\nTutaj będą wyświetlone szczegółowe informacje o pokoju.",
                "Szczegóły pokoju", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }

        private void OnBackButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewNavigationService.Instance.NavigateTo(ViewType.Homepage);
        }

        private void OnFilterButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Funkcja filtrowania zostanie wkrótce zaimplementowana.",
                "Filtrowanie", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }
    }
}