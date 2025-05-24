using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using EscapeRoom.ViewModels;
using EscapeRoom.Models;
using System.Windows.Media;
using System.Windows;

namespace EscapeRoom.Views
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }
        private void OnLoginButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {

            System.Windows.MessageBox.Show("Login button clicked!");
        }

        private void PasswordInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // implementacja eventu
        }

    }
}
    
