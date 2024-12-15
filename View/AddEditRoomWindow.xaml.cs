using System;
using System.Windows;
using Microsoft.Win32;
using Hotel_Management.Models;

namespace Hotel_Management.View
{
    public partial class AddEditRoomWindow : Window
    {
        public Rooms Room { get; private set; }

        public AddEditRoomWindow(Rooms? room = null)
        {
            InitializeComponent();
            Room = room ?? new Rooms();
            DataContext = Room;

            if (Room != null)
            {
                RoomNumberTextBox.Text = Room.NumR;
                RoomPriceTextBox.Text = Room.Nprice.ToString();
                RoomTypeComboBox.SelectedIndex = (int)Room.TypeR;
                RoomStatusComboBox.SelectedIndex = (int)Room.Status;
                RoomPictureTextBox.Text = Room.PicturePath;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RoomNumberTextBox.Text) || string.IsNullOrWhiteSpace(RoomPriceTextBox.Text))
            {
                MessageBox.Show("Please fill all the fields.");
                return;
            }

            Room.NumR = RoomNumberTextBox.Text;
            if (decimal.TryParse(RoomPriceTextBox.Text, out var price))
            {
                Room.Nprice = price;
            }
            else
            {
                MessageBox.Show("Invalid price format.");
                return;
            }

            Room.TypeR = (RoomType)RoomTypeComboBox.SelectedIndex;
            Room.Status = (RoomStatus)RoomStatusComboBox.SelectedIndex;
            Room.PicturePath = RoomPictureTextBox.Text;

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void SelectPictureButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif", 
                Title = "Select a Room Picture"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                RoomPictureTextBox.Text = openFileDialog.FileName;
            }
        }
    }
}
