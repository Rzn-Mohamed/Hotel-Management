using System.Linq;
using System.Windows;

namespace HotelManagement.Views
{
    public partial class Login : Window
    {
        private readonly AppDbContext _context;

        public Login()
        {
            InitializeComponent();
            _context = new AppDbContext();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var email = EmailTextBox.Text;
            var password = PasswordBox.Password;

            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                MessageBox.Show($"Welcome, {user.Name}!");

                MainWindow mainWindow = new MainWindow(user.Role);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid email or password.");
            }
        }

        private void OpenSignUpWindow(object sender, RoutedEventArgs e)
        {
            Signup signup = new Signup();
            signup.Show();
        }
    }
}
