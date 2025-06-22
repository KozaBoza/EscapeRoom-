using System.Windows.Controls;
using EscapeRoom.ViewModels; 
using EscapeRoom.Models;    
using System.Windows.Media; 
using EscapeRoom.Services;  
using System.Windows;       
using System;  
using System.Collections.ObjectModel;

namespace EscapeRoom.Views
{
   
    public partial class RoomView : UserControl
    {
        public RoomView()
        {
            InitializeComponent();
          if (this.DataContext == null) // Sprawdzanie, czy DataContext nie został już ustawiony
            {
                this.DataContext = new RoomViewModel();
            }
        }

       
    }
}