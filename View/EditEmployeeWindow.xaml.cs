using System.Windows;
using Hotel_Management.Models;

namespace Hotel_Management.View
{
    public partial class EditEmployeeWindow : Window
    {
        public Employee Employee { get; set; }

        public EditEmployeeWindow(Employee employee)
        {
            InitializeComponent();
            Employee = employee;
            DataContext = this;  // Bind the window data context to the current instance
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Save changes to the employee in the database
            using (var dbContext = new AppDbContext())
            {
                dbContext.Entry(Employee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                dbContext.SaveChanges();
            }

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
