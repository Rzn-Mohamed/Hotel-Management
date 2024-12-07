using Hotel_Management.Models;
using Hotel_Management.Services;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagement.Views
{
    public partial class ClientsPage : Page
    {
        private readonly ClientService _clientService;

        public ClientsPage()
        {
            InitializeComponent();
            _clientService = new ClientService(new AppDbContext());
            LoadClients();
        }

        private void LoadClients()
        {
           
            List<Client> clients = _clientService.GetAllClients();

     
            ClientsDataGrid.ItemsSource = clients;
        }

        private void AddNewClientButton_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.Navigate(new AddClientPage());
        }


        private void DeleteClientButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var messageBox = new CustomMessageBox("Client Deleted successfully!");
            if (button?.CommandParameter is int clientId)
            {
                _clientService.DeleteClient(clientId);
                messageBox.ShowDialog();
            }

            LoadClients();
        }


    

   
     

    }
}
