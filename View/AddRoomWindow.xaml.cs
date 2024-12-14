using Hotel_Management.Models;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;

namespace Hotel_Management.Views
{
    public partial class AddRoomWindow : Window
    {
        private ObservableCollection<Rooms> _rooms;
        private string? _picturePath;

        // Constructor to initialize the ObservableCollection of rooms
        public AddRoomWindow(ObservableCollection<Rooms> rooms)
        {
            InitializeComponent();
            _rooms = rooms;
        }

        // Choose Picture button click event to select an image file
        private void ChoosePicture_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.png)|*.jpg;*.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _picturePath = openFileDialog.FileName; // Store the selected picture path
            }
        }

        // Add Room button click event to add the new room to the ObservableCollection
        private void AddRoomButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if any required fields are empty
            if (string.IsNullOrEmpty(RoomNumberTextBox.Text) ||
                string.IsNullOrEmpty(RoomPriceTextBox.Text) ||
                RoomTypeComboBox.SelectedItem == null ||
                RoomStatusComboBox.SelectedItem == null ||
                string.IsNullOrEmpty(_picturePath))
            {
                MessageBox.Show("Please fill out all fields.");
                return;
            }

            // Parsing room number and price
            if (!int.TryParse(RoomNumberTextBox.Text, out int roomNumber))
            {
                MessageBox.Show("Please enter a valid room number.");
                return;
            }

            if (!decimal.TryParse(RoomPriceTextBox.Text, out decimal roomPrice))
            {
                MessageBox.Show("Please enter a valid price.");
                return;
            }

            // Get the selected room type and status
            RoomType type = (RoomType)RoomTypeComboBox.SelectedIndex;
            RoomStatus status = (RoomStatus)RoomStatusComboBox.SelectedIndex;

            // Create a new room and add it to the ObservableCollection
            _rooms.Add(new Rooms
            {
                NumR = roomNumber.ToString(), // Room number
                Nprice = roomPrice, // Room price
                TypeR = type, // Room type
                Status = status, // Room status
                PicturePath = _picturePath // Picture path
            });

            // Display success message and close the window
            MessageBox.Show("Room added successfully!");
            this.Close();
        }

        public Rooms NewRoom { get; private set; } = null!;

        


    }
}