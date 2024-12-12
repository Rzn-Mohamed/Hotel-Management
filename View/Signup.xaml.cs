using System.Windows;
using Hotel_Management.Models;

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

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill out all fields.");
                return;
            }

            // Create a new Client by default (or adjust to another user type)
            var newUser = new Client
            {
                Name = name,
                Email = email,
                Password = password
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            MessageBox.Show("Account created successfully!");
            this.Close();
        }
    }
}
