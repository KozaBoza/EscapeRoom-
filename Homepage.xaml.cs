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
    public partial class Homepage : UserControl
    {
        public Homepage()
        {
            InitializeComponent();
        }
        private void OnContactButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {

            System.Windows.MessageBox.Show("Contact button clicked!");
        }
        private void OnReservationButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Reservation button clicked!");
        }
    }
}
