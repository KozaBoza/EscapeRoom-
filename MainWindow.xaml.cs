using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EscapeRoom.ViewModels;
using EscapeRoom.Models;
using EscapeRoom.Services;

namespace EscapeRoom.Views
{ //do poprawy
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeNavigation();
        }

        private void InitializeNavigation()
        {
            ViewNavigationService.Instance.ViewChanged += OnViewChanged;
        }

        private void OnViewChanged(ViewType newView)
        {
            switch (newView)
            {
                case ViewType.Homepage:
                    this.Title = "Escape Room - Strona główna";
                    break;
                case ViewType.Login:
                    this.Title = "Escape Room - Logowanie";
                    break;
                case ViewType.AdminDashboard:
                    this.Title = "Escape Room - Panel administratora";
                    break;
                case ViewType.ReservationForm:
                    this.Title = "Escape Room - Rezerwacja";
                    break;
                case ViewType.Contact:
                    this.Title = "Escape Room - Kontakt";
                    break;
                case ViewType.Payment:
                    this.Title = "Escape Room - Płatność";
                    break;
                case ViewType.Room:
                    this.Title = "Escape Room - Pokoje";
                    break;
                case ViewType.Review:
                    this.Title = "Escape Room - Opinie";
                    break;
                case ViewType.User:
                    this.Title = "Escape Room - Panel użytkownika";
                    break;
            }
        }

        private void OnContactButtonClick(object sender, RoutedEventArgs e)
        {
            ViewNavigationService.Instance.NavigateTo(ViewType.Contact);
        }

        private void OnReservationButtonClick(object sender, RoutedEventArgs e)
        {
            ViewNavigationService.Instance.NavigateTo(ViewType.ReservationForm);
        }

        private void OnLoginButtonClick(object sender, RoutedEventArgs e)
        {
            ViewNavigationService.Instance.NavigateTo(ViewType.Login);
        }

        private void OnRoomsButtonClick(object sender, RoutedEventArgs e)
        {
            ViewNavigationService.Instance.NavigateTo(ViewType.Room);
        }

        private void OnReviewsButtonClick(object sender, RoutedEventArgs e)
        {
            ViewNavigationService.Instance.NavigateTo(ViewType.Review);
        }

        private void OnAboutButtonClick(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show(
                "Escape Room - Najlepsze pokoje zagadek w mieście!\n\n" +
                "Oferujemy różnorodne tematyczne pokoje z zagadkami,\n" +
                "które sprawdzą Twoje umiejętności logicznego myślenia\n" +
                "i pracy w zespole.\n\n" +
                "Zapraszamy na niezapomnianą przygodę!",
                "O nas",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void OnHomeButtonClick(object sender, RoutedEventArgs e)
        {
            this.Title = "Escape Room - Strona główna";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var button = sender as Button;
            if (button?.Tag != null)
            {
                string action = button.Tag.ToString();
                HandleDynamicAction(action);
            }
        }

        private void HandleDynamicAction(string action)
        {
            switch (action.ToLower())
            {
                case "emergency":
                    OnEmergencyButtonClick();
                    break;
                case "help":
                    OnHelpButtonClick();
                    break;
                default:
                    System.Windows.MessageBox.Show($"Akcja: {action}", "Informacja");
                    break;
            }
        }

        private void OnEmergencyButtonClick()
        {
            var result = System.Windows.MessageBox.Show(
                "Czy potrzebujesz natychmiastowej pomocy?\n\n" +
                "Kliknij 'Tak' aby skontaktować się z obsługą,\n" +
                "lub 'Nie' aby anulować.",
                "Pomoc awaryjna",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                System.Windows.MessageBox.Show(
                    "Łączenie z obsługą...\n\nTelefon: +48 123 456 789\nEmail: help@escaperoom.pl",
                    "Kontakt z obsługą",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void OnHelpButtonClick()
        {
            System.Windows.MessageBox.Show(
                "Pomoc - Escape Room\n\n" +
                "• Aby dokonać rezerwacji, kliknij 'Rezerwacja'\n" +
                "• Aby zobaczyć nasze pokoje, kliknij 'Pokoje'\n" +
                "• Aby się zalogować, kliknij 'Logowanie'\n" +
                "• W razie pytań, skontaktuj się z nami\n\n" +
                "Życzymy udanej gry!",
                "Pomoc",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void OnMenuFileExitClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnMenuViewFullScreenClick(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void OnMenuHelpAboutClick(object sender, RoutedEventArgs e)
        {
            OnAboutButtonClick(sender, e);
        }

        protected override void OnClosed(EventArgs e)
        {
            ViewNavigationService.Instance.ViewChanged -= OnViewChanged;
            base.OnClosed(e);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = System.Windows.MessageBox.Show(
                "Czy na pewno chcesz zamknąć aplikację?",
                "Zamykanie aplikacji",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
