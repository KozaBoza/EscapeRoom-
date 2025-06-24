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
{
    public partial class MainWindow : Window
    {
        private System.Windows.Threading.DispatcherTimer _userUpdateTimer;

        public MainWindow()
        {
            InitializeComponent();
            InitializeNavigation();
            InitializeUserSessionUpdater();
            //typ homepage
            ViewNavigationService.Instance.NavigateTo(ViewType.Homepage);
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowState = WindowState.Maximized;
            this.Topmost = true;
        }

        private void InitializeNavigation()
        {
            ViewNavigationService.Instance.ViewChanged += OnViewChanged;
        }

        private void InitializeUserSessionUpdater()
        {
            _userUpdateTimer = new System.Windows.Threading.DispatcherTimer();
            _userUpdateTimer.Interval = TimeSpan.FromSeconds(1);
            _userUpdateTimer.Tick += UpdateUserInterface;
            _userUpdateTimer.Start();
        }

        private void UpdateUserInterface(object sender, EventArgs e)
        {
            if (UserSession.IsLoggedIn && UserSession.CurrentUser != null)
            {
                var loginButton = this.FindName("LoginButton") as Button;
                if (loginButton != null)
                {
                    loginButton.Content = $"{UserSession.CurrentUser.Imie} {UserSession.CurrentUser.Nazwisko}";
                }

                //  przycisk wylogowania
                var logoutButton = this.FindName("LogoutButton") as Button;
                if (logoutButton != null)
                {
                    logoutButton.Visibility = Visibility.Visible;
                }
            }
            else
            {
                //  domyślny tekst przycisku logowania
                var loginButton = this.FindName("LoginButton") as Button;
                if (loginButton != null)
                {
                    loginButton.Content = "Logowanie";
                }

                var logoutButton = this.FindName("LogoutButton") as Button;
                if (logoutButton != null)
                {
                    logoutButton.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void OnViewChanged(ViewType newView)
        {
            UserControl newViewControl = null;
            switch (newView)
            {
                case ViewType.Homepage:
                    this.Title = "Escape Room - Strona główna";

                    break;
                case ViewType.Login:
                    this.Title = "Escape Room - Logowanie";
                    newViewControl = new LoginView();
                    break;
                case ViewType.AdminDashboard:
                    this.Title = "Escape Room - Panel administratora";
                    newViewControl = new AdminDashboardView();
                    break;
                case ViewType.ReservationForm:
                    this.Title = "Escape Room - Rezerwacja";
                    newViewControl = new ReservationFormView();
                    break;
                case ViewType.Contact:
                    this.Title = "Escape Room - Kontakt";
                    newViewControl = new ContactView();
                    break;
                case ViewType.Payment:
                    this.Title = "Escape Room - Płatność";
                    newViewControl = new PaymentView();
                    break;
                case ViewType.Room:
                    this.Title = "Escape Room - Pokoje";
                    newViewControl = new RoomView();
                    break;
                case ViewType.Review:
                    this.Title = "Escape Room - Opinie";
                    newViewControl = new ReviewView();
                    break;
                case ViewType.User:
                    this.Title = "Escape Room - Panel użytkownika";
                    newViewControl = new UserView();
                    break;
                case ViewType.Register:
                    this.Title = "Escape Room - Rejestracja";
                    newViewControl = new RegisterView();
                    break;
                case ViewType.ReservationHistory:
                    this.Title = "Escape Room - Historia Rezerwacji";
                    newViewControl = new ReservationHistoryView();
                    break;
            }

            if (newViewControl != null)
            {
                MainContentControl.Content = newViewControl;
            }
        }


        private void OnReservationButtonClick(object sender, RoutedEventArgs e)
        {
            ViewNavigationService.Instance.NavigateTo(ViewType.ReservationHistory);
        }


        private void OnUserButtonClick(object sender, RoutedEventArgs e)
        {
            ViewNavigationService.Instance.NavigateTo(ViewType.User);
        }

        private void OnLoginButtonClick(object sender, RoutedEventArgs e)
        {
            if (UserSession.IsLoggedIn)
            {
                // panele użytkownika do zmiany
                if (UserSession.CurrentUser.Admin)
                {
                    ViewNavigationService.Instance.NavigateTo(ViewType.AdminDashboard);
                }
                else
                {
                    ViewNavigationService.Instance.NavigateTo(ViewType.User);
                }
            }
            else
            {
                // jesli nie jest zalogowany, pokaż ekran logowania
                ViewNavigationService.Instance.NavigateTo(ViewType.Login);
            }
        }

        private void OnLogoutButtonClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Czy na pewno chcesz się wylogować?",
                "Wylogowanie",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                UserSession.Logout();
                ViewNavigationService.Instance.NavigateTo(ViewType.Homepage);
                MessageBox.Show("Wylogowano pomyślnie.", "Informacja",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OnRoomsButtonClick(object sender, RoutedEventArgs e)
        {
            ViewNavigationService.Instance.NavigateTo(ViewType.Room);
        }

        private void OnContactButtonClick(object sender, RoutedEventArgs e)
        {
            ViewNavigationService.Instance.NavigateTo(ViewType.Contact);
        }

        private void OnAboutButtonClick(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show(
                "",
                "O nas",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void OnHomeButtonClick(object sender, RoutedEventArgs e)
        {
            ViewNavigationService.Instance.NavigateTo(ViewType.Homepage);
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
                    "Łączenie z obsługą...\n\nTelefon: +48 000 000 000\nEmail: help@escaperoom.pl",
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
                "• W razie pytań, skontaktuj się\n\n" +
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
            _userUpdateTimer?.Stop();
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

        private void OnAdminButtonClick(object sender, RoutedEventArgs e)
        {
            ViewNavigationService.Instance.NavigateTo(ViewType.AdminDashboard);
        }
    }
}