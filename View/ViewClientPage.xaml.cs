
using Hotel_Management.Models;
using System.Windows.Controls;

namespace HotelManagement.Views
{
    public partial class ViewClientPage : Page
    {
        public ViewClientPage(Client client)
        {
            InitializeComponent();
            DataContext = client;
        }
    }
}