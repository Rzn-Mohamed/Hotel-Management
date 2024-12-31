using Hotel_Management.Models;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagement.Views
{
    public partial class EditClientPage : Page
    {
        private Client _client;

        public EditClientPage(Client client)
        {
            InitializeComponent();
            _client = client;
            DataContext = _client;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new AppDbContext())
            {
                var clientInDb = dbContext.Clients.FirstOrDefault(c => c.Id == _client.Id);
                if (clientInDb != null)
                {
                    clientInDb.Name = _client.Name;
                    clientInDb.Email = _client.Email;
                    clientInDb.Gender = _client.Gender;
                    clientInDb.Address = _client.Address;
                    clientInDb.BirthDate = _client.BirthDate;

                    dbContext.SaveChanges();
                    MessageBox.Show("Client updated successfully!");
                    NavigationService.GoBack();
                }
                else
                {
                    MessageBox.Show("Client not found.");
                }
            }
        }
    }
}
