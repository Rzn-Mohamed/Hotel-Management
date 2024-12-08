using System.Windows;
using Hotel_Management.Models;
using System.Windows.Controls;

namespace HotelManagement.Views
{
    public partial class Signup : Window
    {
        private readonly AppDbContext _context;

        public Signup()
        {
            InitializeComponent();
            _context = new AppDbContext();
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            var name = NameTextBox.Text;
            var email = EmailTextBox.Text;
            var password = PasswordBox.Password;
            var role = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Please fill out all fields.");
                return;
            }

            User newUser;

            switch (role)
            {
                case "Admin":
                    newUser = new Manager { Name = name, Email = email, Password = password };
                    break;
                case "Employee":
                    newUser = new Employee { Name = name, Email = email, Password = password };
                    break;
                case "Client":
                    newUser = new Client { Name = name, Email = email, Password = password };
                    break;
                default:
                    MessageBox.Show("Invalid role selected.");
                    return;
            }

            _context.Users.Add(newUser);
            _context.SaveChanges();

            MessageBox.Show("Account created successfully!");
            this.Close();
        }
    }
}
