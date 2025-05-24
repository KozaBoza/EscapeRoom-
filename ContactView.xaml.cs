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
    public partial class ContactView : UserControl
    {
        public ContactView()
        {
            InitializeComponent();
        }
        private void OnSendButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            // Logic to handle the sending of the contact form
            // This could involve validating input, saving data, etc.
            // For now, we will just show a message box as a placeholder.
            System.Windows.MessageBox.Show("Message sent successfully!");
        }
    }
}
