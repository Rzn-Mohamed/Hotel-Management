﻿using System.Windows;
using System.Windows.Controls;
using Hotel_Management.Models;

namespace Hotel_Management.View
{
    public partial class AddEmployeeWindow : Window
    {
        private readonly AppDbContext _dbContext;

        public AddEmployeeWindow()
        {
            InitializeComponent();
            _dbContext = new AppDbContext();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) || string.IsNullOrWhiteSpace(EmailTextBox.Text) || string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            var newEmployee = new Employee
            {
                Name = NameTextBox.Text,
                Email = EmailTextBox.Text,
                Password = PasswordBox.Password,
                Role = "Employee"
            };

            _dbContext.Employees.Add(newEmployee);
            _dbContext.SaveChanges();

            MessageBox.Show("Employee added successfully!");

            this.DialogResult = true;  // This will indicate successful addition
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;  // Close without saving
            this.Close();
        }
    }
}
