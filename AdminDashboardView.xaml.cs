using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using EscapeRoom.ViewModels;
using EscapeRoom.Models;
using System.Windows.Media;


namespace EscapeRoom.Views
{
    public partial class AdminDashboardView : UserControl
    {
        public AdminDashboardView()
        {
            InitializeComponent();
        }
        private void OnAddRoomButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Add Room button clicked!");
        }
        private void OnViewReservationsButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {

            System.Windows.MessageBox.Show("View Reservations button clicked!");
        }
    }
}
