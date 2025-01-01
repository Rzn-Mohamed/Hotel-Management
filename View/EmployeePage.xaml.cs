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
            EmployeeDataGrid.ItemsSource = _dbContext.Employees.ToList();
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            var addEmployeeWindow = new AddEmployeeWindow();
            if (addEmployeeWindow.ShowDialog() == true)
            {
                LoadEmployees();
            }
        }

        private void EditEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var employeeId = button?.CommandParameter as int?;

            if (employeeId.HasValue)
            {
                var selectedEmployee = _dbContext.Employees.FirstOrDefault(emp => emp.Id == employeeId.Value);
                if (selectedEmployee != null)
                {
                    var editEmployeeWindow = new EditEmployeeWindow(selectedEmployee, _dbContext);
                    if (editEmployeeWindow.ShowDialog() == true)
                    {
                        LoadEmployees();
                    }
                }
            }
        }

        private void DeleteEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var employeeId = button?.CommandParameter as int?;

            if (employeeId.HasValue)
            {
                var selectedEmployee = _dbContext.Employees.FirstOrDefault(emp => emp.Id == employeeId.Value);
                if (selectedEmployee != null)
                {
                    var result = MessageBox.Show($"Are you sure you want to delete {selectedEmployee.Name}?",
                                                 "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        _dbContext.Employees.Remove(selectedEmployee);
                        _dbContext.SaveChanges();
                        LoadEmployees();
                    }
                }
            }
        }

        private void ViewEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var employeeId = button?.CommandParameter as int?;

            if (employeeId.HasValue)
            {
                var selectedEmployee = _dbContext.Employees.FirstOrDefault(emp => emp.Id == employeeId.Value);
                if (selectedEmployee != null)
                {
                    var viewEmployeeWindow = new ViewEmployeeWindow(selectedEmployee);
                    viewEmployeeWindow.ShowDialog();
                }
            }
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = searchTextBox.Text.ToLower();
            var filteredEmployees = _dbContext.Employees
                .Where(emp => emp.Name.ToLower().Contains(searchText) ||
                              emp.Email.ToLower().Contains(searchText) ||
                              emp.Role.ToLower().Contains(searchText))
                .ToList();
            EmployeeDataGrid.ItemsSource = filteredEmployees;
        }

        private void EmployeeDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Handle DataGrid selection changes if needed
        }
    }

    // Add the missing ViewEmployeeWindow class
    public class ViewEmployeeWindow : Window
    {
        public ViewEmployeeWindow(Employee employee)
        {
            Title = "View Employee";
            Width = 400;
            Height = 300;
            Content = new StackPanel
            {
                Children =
                {
                    new TextBlock { Text = $"Name: {employee.Name}", Margin = new Thickness(10) },
                    new TextBlock { Text = $"Email: {employee.Email}", Margin = new Thickness(10) },
                    new TextBlock { Text = $"Role: {employee.Role}", Margin = new Thickness(10) }
                }
            };
        }
    }
}
