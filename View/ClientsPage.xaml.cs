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
            NavigationService.Navigate(new AddClientPage());
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


        private void ExportToExcelButton_Click(object sender, RoutedEventArgs e)
        {
            if (_clients.Count > 0)
            {
                // Open a SaveFileDialog to choose where to save the file
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    Title = "Save Excel File"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    ExportClientsToExcel(filePath);
                }
            }
            else
            {
                MessageBox.Show("No clients found to export.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportClientsToExcel(string filePath)
        {
            try
            {
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    var worksheet = package.Workbook.Worksheets.Add("Clients");
                    worksheet.Cells[1, 1].Value = "ID";
                    worksheet.Cells[1, 2].Value = "Name";
                    worksheet.Cells[1, 3].Value = "Email";
                    worksheet.Cells[1, 4].Value = "Gender";
                    worksheet.Cells[1, 5].Value = "Address";
                    worksheet.Cells[1, 6].Value = "BirthDate";

                    for (int i = 0; i < _clients.Count; i++)
                    {
                        worksheet.Cells[i + 2, 1].Value = _clients[i].Id;
                        worksheet.Cells[i + 2, 2].Value = _clients[i].Name;
                        worksheet.Cells[i + 2, 3].Value = _clients[i].Email;
                        worksheet.Cells[i + 2, 4].Value = _clients[i].Gender;
                        worksheet.Cells[i + 2, 5].Value = _clients[i].Address;
                        worksheet.Cells[i + 2, 6].Value = _clients[i].BirthDate.ToString("MM/dd/yyyy");
                    }

                    package.Save();
                    MessageBox.Show("Clients exported successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting clients: {ex.Message}");
            }
        }

        private void ImportClientsFromExcel(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified file does not exist.");
            }

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
                for (int row = 2; row <= rowCount; row++)
                {
                    var client = new Client
                    {
                        Name = worksheet.Cells[row, 1].Text.Trim(),
                        Email = worksheet.Cells[row, 2].Text.Trim(),
                        Password = worksheet.Cells[row, 3].Text.Trim(),
                        Role = worksheet.Cells[row, 4].Text.Trim(),
                        Gender = worksheet.Cells[row, 5].Text.Trim(),
                        Address = worksheet.Cells[row, 6].Text.Trim(),
                        BirthDate = DateTime.Parse(worksheet.Cells[row, 7].Text.Trim())
                    };

                    importedClients.Add(client);
                }
            }

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
                    _clients.Remove(client);

                    using (var dbContext = new AppDbContext())
                    {
                        dbContext.Clients.Remove(client);
                        dbContext.SaveChanges();
                    }

                    MessageBox.Show("Client deleted successfully!");
                }
            }
        }

        private void ViewClientButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var clientId = button?.CommandParameter as int?;

            if (clientId.HasValue)
            {
                var client = _clients.FirstOrDefault(c => c.Id == clientId.Value);
                if (client != null)
                {
                    NavigationService.Navigate(new ViewClientPage(client));
                }
            }
        }

        private void EditClientButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var clientId = button?.CommandParameter as int?;

            if (clientId.HasValue)
            {
                var client = _clients.FirstOrDefault(c => c.Id == clientId.Value);
                if (client != null)
                {
                    NavigationService.Navigate(new EditClientPage(client));
                }
            }
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                var filteredClients = _clients.Where(client => client.Name == searchTextBox.Text).ToList();

                ClientsDataGrid.ItemsSource = filteredClients;
            }
            else
            {
                ClientsDataGrid.ItemsSource = _clients;
            }
        }

        private void ClientsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
