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
{ //obsłużyc przyciski i poprawić errormessage etc
    public partial class ContactView : UserControl
    {
        public ContactView()
        {
            InitializeComponent();
        }
        private void OnSendButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {

            System.Windows.MessageBox.Show("Message sent");
        }
    }
}
