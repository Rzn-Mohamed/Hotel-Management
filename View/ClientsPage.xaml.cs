using Hotel_Management.Models;
using Microsoft.Win32;
using OfficeOpenXml; // Ensure EPPlus is installed
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagement.Views
{
    public partial class ClientsPage : Page
    {
        private ObservableCollection<Client> _clients;

        private string _userRole;

        //public ClientPage(string role)
        //{
        //    InitializeComponent();
        //    _userRole = role;
        //    ApplyRoleRestrictions();
        //}

        //private void ApplyRoleRestrictions()
        //{
        //    if (_userRole == "Employee")
        //    {
        //        // Disable delete button
        //        DeleteClientButton.IsEnabled = false;
        //    }
        //}

        public ClientsPage()
        {
            InitializeComponent();
            _clients = new ObservableCollection<Client>();
            ClientsDataGrid.ItemsSource = _clients;
            LoadClients(); // Load existing clients when the page initializes
        }

        private void AddNewClientButton_Click(object sender, RoutedEventArgs e)
        {
            // Logic to add a new client
            MessageBox.Show("Add New Client functionality is not implemented yet.");
        }

        private void ImportFromExcelButton_Click(object sender, RoutedEventArgs e)
        {
            // Open file dialog to select the Excel file
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Excel Files|*.xlsx;*.xls",
                Title = "Select Excel File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                try
                {
                    ImportClientsFromExcel(filePath);
                    MessageBox.Show("Clients imported successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error importing clients: {ex.Message}");
                }
            }
        }

        private void ImportClientsFromExcel(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified file does not exist.");
            }

            // Temporary list to hold clients from the file
            var importedClients = new List<Client>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                {
                    throw new InvalidOperationException("The Excel file is empty.");
                }

                int rowCount = worksheet.Dimension.Rows;
                for (int row = 2; row <= rowCount; row++) // Skip header row
                {
                    var client = new Client
                    {
                        Name = worksheet.Cells[row, 1].Text.Trim(),
                        Email = worksheet.Cells[row, 2].Text.Trim(),
                        Password = worksheet.Cells[row, 3].Text.Trim()
                    };

                    importedClients.Add(client);
                }
            }

            // Add imported clients to the database and update UI
            using (var dbContext = new AppDbContext())
            {
                dbContext.Clients.AddRange(importedClients); 
                try
                {
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error importing clients: {ex.Message}\nDetails: {ex.InnerException?.Message}");
                }
            }

            // Add imported clients to the ObservableCollection to update the UI
            foreach (var client in importedClients)
            {
                _clients.Add(client);
            }
        }

        private void LoadClients()
        {
            using (var dbContext = new AppDbContext())
            {
                var clients = dbContext.Clients.ToList();
                _clients.Clear();
                foreach (var client in clients)
                {
                    _clients.Add(client);
                }
            }
        }

        private void DeleteClientButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var clientId = button?.CommandParameter as int?;

            if (clientId.HasValue)
            {
                var client = _clients.FirstOrDefault(c => c.Id == clientId.Value);
                if (client != null)
                {
                    // Remove from collection
                    _clients.Remove(client);

                    // Remove from database
                    using (var dbContext = new AppDbContext())
                    {
                        dbContext.Clients.Remove(client);
                        dbContext.SaveChanges();
                    }

                    MessageBox.Show("Client deleted successfully!");
                }
            }
        }
    }
}
