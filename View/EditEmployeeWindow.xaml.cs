using System.Windows;
using System.Windows.Controls;
using Hotel_Management.Models;

namespace Hotel_Management.View
{
    public partial class EditEmployeeWindow : Window
    {
        public Employee Employee { get; set; }
        private readonly AppDbContext _dbContext;

        public EditEmployeeWindow(Employee employee, AppDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;
            Employee = employee;
            DataContext = this;  // Bind the window data context to the current instance
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _dbContext.Entry(Employee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _dbContext.SaveChanges();

            MessageBox.Show("Employee updated successfully!");
            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
