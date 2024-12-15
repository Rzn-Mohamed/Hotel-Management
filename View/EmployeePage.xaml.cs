using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Hotel_Management.Models;

namespace Hotel_Management.View
{
    public partial class EmployeePage : Page
    {
        private readonly AppDbContext _dbContext;

        public EmployeePage()
        {
            InitializeComponent();
            _dbContext = new AppDbContext();
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            // Load employee data from the database into the ListView
            EmployeeListView.ItemsSource = _dbContext.Employees.ToList();
        }

        // Event handler for Add Employee button click
        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            var addEmployeeWindow = new AddEmployeeWindow();
            if (addEmployeeWindow.ShowDialog() == true)
            {
                LoadEmployees();  // Reload employee list after adding a new employee
            }
        }

        // Event handler for Edit button click
        private void EditEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = EmployeeListView.SelectedItem as Employee;

            if (selectedEmployee != null)
            {
                var editEmployeeWindow = new EditEmployeeWindow(selectedEmployee);
                if (editEmployeeWindow.ShowDialog() == true)
                {
                    LoadEmployees();  // Reload employee list after editing
                }
            }
            else
            {
                MessageBox.Show("Please select an employee to edit.");
            }
        }

        // Event handler for Delete button click
        private void DeleteEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedEmployee = EmployeeListView.SelectedItem as Employee;

            if (selectedEmployee != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete {selectedEmployee.Name}?",
                                             "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _dbContext.Employees.Remove(selectedEmployee);
                    _dbContext.SaveChanges();
                    LoadEmployees();  // Reload employee list after deletion
                }
            }
            else
            {
                MessageBox.Show("Please select an employee to delete.");
            }
        }
    }
}
