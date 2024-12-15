using System.Windows.Controls;
using System.Windows;
using Hotel_Management.Views;
using Hotel_Management.View;

namespace HotelManagement.Views
{
    public partial class MainWindow : Window
    {
        private string _userRole;

        public MainWindow(string role)
        {
            InitializeComponent();
            _userRole = role;
            MainFrame.Navigate(new DashboardPage());
            ApplyRoleRestrictions();
        }

        private void ApplyRoleRestrictions()
        {
            if (_userRole == "Employee")
            {
                var navigateClientsButton = (Button)FindName("NavigateClients");
                var navigateRoomsButton = (Button)FindName("NavigateRooms");
                var navigateEmployeeButton = (Button)FindName("NavigateEmployee");

                if (navigateClientsButton != null)
                {
                    navigateClientsButton.Visibility = Visibility.Collapsed;
                }

                if (navigateRoomsButton != null)
                {
                    navigateRoomsButton.Visibility = Visibility.Collapsed;
                }

                if (navigateEmployeeButton != null)
                {
                    navigateEmployeeButton.Visibility = Visibility.Collapsed;
                }

            }
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
                    case "EmployeePage":
                        MainFrame.Navigate(new EmployeePage());
                        break;
                }
            }
        }
    }
}
