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
    public partial class ReservationFormView
    {
        public ReservationFormView()
        {
            InitializeComponent();
        }
        private void OnSubmitButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            // Logic to handle the submission of the reservation form
            // This could involve validating input, saving data, etc.
            // For now, we will just show a message box as a placeholder.
            System.Windows.MessageBox.Show("Reservation submitted successfully!");
        }
    }
}
