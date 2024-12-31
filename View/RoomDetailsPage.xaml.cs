using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Hotel_Management.Models;

namespace Hotel_Management.Views
{
    public partial class RoomDetailsPage : Page
    {
        private Rooms _room;

        public RoomDetailsPage(Rooms room)
        {
            InitializeComponent();
            _room = room;
            LoadRoomDetails();
        }

        private void LoadRoomDetails()
        {
            RoomNumberTextBlock.Text = _room.NumR.ToString();
            PriceTextBlock.Text = _room.Nprice.ToString() + "Dh";
            TypeTextBlock.Text = _room.TypeR.ToString();
            StatusTextBlock.Text = _room.Status.ToString();

            if (!string.IsNullOrEmpty(_room.PicturePath))
            {
                RoomImage.Source = new BitmapImage(new Uri(_room.PicturePath, UriKind.RelativeOrAbsolute));
            }
            else
            {
                RoomImage.Source = null; 
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
