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
            
            using (var dbContext = new AppDbContext())
            {
                var client = new Client
                {
                    Name = "Jane Doe", 
                    Email = "janedoe@example.com",
                    isClient = true ,
                    Password=""
                };

                dbContext.Clients.Add(client);
                dbContext.SaveChanges();
            }

            // Refresh the clients list after adding a new client
            LoadClients();
        }


        private void DeleteClientButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if(button?.CommandParameter is int clientId)
            {
                _clientService.DeleteClient(clientId);
            }

            LoadClients();
        }


        private void MenuItemClick(object sender, RoutedEventArgs e)
        {

            MessageBox.Show(((Button)sender).Content.ToString() + " Add client!");
        }

    }
}
