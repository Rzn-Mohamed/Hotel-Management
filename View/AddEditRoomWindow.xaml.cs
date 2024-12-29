using System;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using Hotel_Management.Models;

namespace Hotel_Management.View
{
    public partial class AddEditRoomWindow : Window
    {
        private readonly AppDbContext _dbContext;

        public Rooms Room { get; private set; }

        public AddEditRoomWindow(Rooms? room = null)
        {
            InitializeComponent();
            _dbContext = new AppDbContext(); 
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
            if (string.IsNullOrWhiteSpace(RoomNumberTextBox.Text) ||
                string.IsNullOrWhiteSpace(RoomPriceTextBox.Text))
            {
                MessageBox.Show("Please fill all the fields.");
                return;
            }

            var roomNumber = RoomNumberTextBox.Text;
            if (_dbContext.Rooms.Any(r => r.NumR == roomNumber && r.Id != Room.Id)) 
            {
                MessageBox.Show($"Room number {roomNumber} already exists. Please use a unique number.");
                return;
            }

            if (!decimal.TryParse(RoomPriceTextBox.Text, out var price) || price <= 0)
            {
                MessageBox.Show("Price must be a positive number.");
                return;
            }

            if (RoomTypeComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a room type.");
                return;
            }

            if (RoomStatusComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a room status.");
                return;
            }

            var picturePath = RoomPictureTextBox.Text;
            if (!string.IsNullOrWhiteSpace(picturePath) && !File.Exists(picturePath))
            {
                MessageBox.Show("The selected picture file does not exist.");
                return;
            }

            Room.NumR = roomNumber;
            Room.Nprice = price;
            Room.TypeR = (RoomType)RoomTypeComboBox.SelectedIndex;
            Room.Status = (RoomStatus)RoomStatusComboBox.SelectedIndex;
            Room.PicturePath = picturePath;

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
