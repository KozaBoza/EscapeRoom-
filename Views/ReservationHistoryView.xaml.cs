using System.Windows.Controls;

namespace EscapeRoom.Views
{
    public partial class ReservationHistoryView : UserControl
    {
        public ReservationHistoryView()
        {
            InitializeComponent();
            DataContext = new ViewModels.ReservationHistoryViewModel();
        }
    }
}