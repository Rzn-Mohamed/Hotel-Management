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
            if (!ValidateInputs())
                return;

            var client = new Client
            {
                Name = NameTextBox.Text.Trim(),
                Email = EmailTextBox.Text.Trim(),
                Password = PasswordBox.Password,
                Role = "client",
                isClient = true
            };

            _clientService.AddClient(client);

            var successMessage = new CustomMessageBox("Client added successfully!");
            successMessage.ShowDialog();

            NavigationService.Navigate(new ClientsPage());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ClientsPage());
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                ShowErrorMessage("All fields are required!");
                return false;
            }

            if (!IsValidEmail(EmailTextBox.Text.Trim()))
            {
                ShowErrorMessage("Please enter a valid email address.");
                return false;
            }

            if (_clientService.ClientExists(EmailTextBox.Text.Trim()))
            {
                ShowErrorMessage("A client with this email already exists.");
                return false;
            }

            if (!IsValidPassword(PasswordBox.Password))
            {
                ShowErrorMessage("Password must be at least 8 characters long.");
                return false;
            }

            return true;
        }

        private void ShowErrorMessage(string message)
        {
            var errorMessage = new CustomMessageBox(message);
            errorMessage.ShowDialog();
        }

        private bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        private bool IsValidPassword(string password)
        {
            return password.Length >= 8;
        }
    }
}
