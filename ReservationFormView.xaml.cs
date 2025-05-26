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
{ //obsługa przycisków
    public partial class ReservationFormView
    {
        public ReservationFormView()
        {
            InitializeComponent();
        }
        private void OnSubmitButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Reservation submitted");
        }
    }
}
