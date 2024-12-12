using Hotel_Management.Models;
using Hotel_Management.Services;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Hotel_Management.Views;


namespace HotelManagement.Views
{
    public partial class DashboardPage : Page
    {
        private readonly ClientService _clientService;

        public DashboardPage()
        {
            InitializeComponent();
            _clientService = new ClientService(new AppDbContext());

            
            DisplayClientsCount();
        }

        private void NavigateToRooms(object sender, RoutedEventArgs e)
        {
       
            NavigationService.Navigate(new RoomsPage());
        }

        private void NavigateToClients(object sender, RoutedEventArgs e)
        {
          
            NavigationService.Navigate(new ClientsPage());
        }

        private void DisplayClientsCount()
        {
           
            int clientCount = _clientService.GetAllClients().Count;
            ClientCount.Text = clientCount.ToString();
        }
    }
}
