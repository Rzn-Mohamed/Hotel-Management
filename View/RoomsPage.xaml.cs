using System.Windows;
using System.Windows.Controls;

namespace HotelManagement.Views
{
    public partial class RoomsPage : Page
    {
        public RoomsPage()
        {
            InitializeComponent();
        }

        private void NavigateToRooms(object sender, RoutedEventArgs e)
        {
            // Navigate to RoomsPage
            NavigationService.Navigate(new RoomsPage());
        }

        private void NavigateToClients(object sender, RoutedEventArgs e)
        {
            // Navigate to ClientsPage
            NavigationService.Navigate(new ClientsPage());
        }
    }
}
