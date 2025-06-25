using EscapeRoom.Services;
using EscapeRoom.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace EscapeRoom.Views
{
    public partial class ReviewView : UserControl
    {
        public ReviewView()
        {
            InitializeComponent();
            this.Loaded += ReviewView_Loaded;
        }

        private void ReviewView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext == null)
            {
                var parameter = ViewNavigationService.Instance.GetNavigationParameter();
                if (parameter is ReviewViewModel viewModel)
                {
                    DataContext = viewModel;
                }
            }
        }

        private void OnSubmitClick(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ReviewViewModel;
            if (vm != null)
            {
                MessageBox.Show(
                    $"Submit clicked!\n" +
                    $"IsValid: {vm.IsValid}\n" +
                    $"HasRoomSelected: {vm.HasRoomSelected}\n" +
                    $"IsLoggedIn: {UserSession.IsLoggedIn}\n" +
                    $"Komentarz: {vm.Komentarz}\n" +
                    $"RoomName: {vm.RoomName}");
            }
            else
            {
                MessageBox.Show("ViewModel is null!");
            }
        }
    }
}