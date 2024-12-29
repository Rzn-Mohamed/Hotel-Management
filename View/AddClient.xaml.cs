using Hotel_Management.Models;
using Hotel_Management.Services;
using System;
using System.Linq;
using System.Text.RegularExpressions;
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

            // Validate email format
            if (!IsValidEmail(EmailTextBox.Text.Trim()))
            {
                var errorMessage = new CustomMessageBox("Please enter a valid email address.");
                errorMessage.ShowDialog();
                return;
            }

            // Check for duplicate email
            if (_clientService.ClientExists(EmailTextBox.Text.Trim()))
            {
                var errorMessage = new CustomMessageBox("A client with this email already exists.");
                errorMessage.ShowDialog();
                return;
            }

            // Validate password strength (minimum 8 characters, includes letters and numbers)
            if (!IsValidPassword(PasswordBox.Password))
            {
                var errorMessage = new CustomMessageBox("Password must be at least 8 characters long and contain letters and numbers.");
                errorMessage.ShowDialog();
                return;
            }

            // Create new client object
            var client = new Client
            {
                Name = NameTextBox.Text.Trim(),
                Email = EmailTextBox.Text.Trim(),
                Password = PasswordBox.Password,
                Role = "client",
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

        // Helper method to validate email format
        private bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        // Helper method to validate password strength
        private bool IsValidPassword(string password)
        {
            // Password must be at least 8 characters and contain at least one letter and one number
            var passwordRegex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$");
            return passwordRegex.IsMatch(password);
        }
    }
}
