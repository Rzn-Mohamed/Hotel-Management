using Hotel_Management.Models;
using Hotel_Management.Services;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagement.Views
{
    public partial class AddClientPage : Page
    {
        private readonly ClientService _clientService;

        public AddClientPage()
        {
            InitializeComponent();
            _clientService = new ClientService(new AppDbContext());
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                var errorMessage = new CustomMessageBox("All fields are required!");
                errorMessage.ShowDialog();
                return;
            }

            // Create new client object
            var client = new Client
            {
                Name = NameTextBox.Text.Trim(),
                Email = EmailTextBox.Text.Trim(),
                Password = PasswordBox.Password,
                isClient = true
            };

            // Save client to database
            _clientService.AddClient(client);

            // Show success message
            var successMessage = new CustomMessageBox("Client added successfully!");
            successMessage.ShowDialog();

            // Navigate back to the Clients page
            NavigationService.Navigate(new ClientsPage());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
  
            NavigationService.Navigate(new ClientsPage());
        }
    }
}
