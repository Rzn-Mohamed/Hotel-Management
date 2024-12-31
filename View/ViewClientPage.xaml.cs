using Hotel_Management.Models;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace HotelManagement.Views
{
    public partial class ViewClientPage : Page
    {
        public ViewClientPage(Client client)
        {
            InitializeComponent();
            DataContext = client;
        }

        private void BackButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (NavigationService?.CanGoBack == true)
            {
                NavigationService.GoBack();
            }
        }
    }
}