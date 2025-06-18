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
using System.Windows;

namespace EscapeRoom.Views
{ //obsługa przycisków
    public partial class ReservationFormView:UserControl
    {
        public ReservationFormView()
        {
            InitializeComponent();

            // Pobierz parametr nawigacji (ReservationViewModel)
            var parameter = ViewNavigationService.Instance.GetNavigationParameter();
            if (parameter is ReservationViewModel reservationViewModel)
            {
                // Ustaw jako DataContext
                DataContext = reservationViewModel;
            }
            else
            {
                // Jeśli brak parametru, utwórz nowy pusty ViewModel
                DataContext = new ReservationViewModel();
            }
        }
    }

}
