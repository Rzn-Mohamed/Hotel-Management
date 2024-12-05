using System.Windows.Controls;
using System.Windows;

namespace HotelManagement.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new DashboardPage());  // Default page load
        }

        private void NavigateToPage(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                switch (button.Tag.ToString())
                {
                    case "DashboardPage":
                        MainFrame.Navigate(new DashboardPage());
                        break;
                    case "RoomsPage":
                        MainFrame.Navigate(new RoomsPage());
                        break;
                    case "ClientsPage":
                        MainFrame.Navigate(new ClientsPage());
                        break;
                }
            }
        }
    }
}
