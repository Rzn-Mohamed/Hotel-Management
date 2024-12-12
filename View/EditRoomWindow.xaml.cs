using System;
using System.Linq;
using System.Windows;
using Hotel_Management.Models;
using System.Windows.Controls;

namespace Hotel_Management.Views
{
    public partial class EditRoomWindow : Window
    {
        private Rooms _roomToEdit;

        // Constructor that accepts a Room object
        public EditRoomWindow(Rooms room)
        {
            InitializeComponent();

            _roomToEdit = room;

            // Pre-fill the form fields with the room's current data
            RoomNumberTextBox.Text = _roomToEdit.NumR;
            RoomPriceTextBox.Text = _roomToEdit.Nprice.ToString();
            RoomTypeComboBox.SelectedItem = RoomTypeComboBox.Items.Cast<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.ToString() == _roomToEdit.TypeR.ToString());
            RoomStatusComboBox.SelectedItem = RoomStatusComboBox.Items.Cast<ComboBoxItem>()
                .FirstOrDefault(item => item.Content.ToString() == _roomToEdit.Status.ToString());
        }

        private void ChoosePicture_Click(object sender, RoutedEventArgs e)
        {
            // Handle image selection here
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            // Update the room details
            _roomToEdit.NumR = RoomNumberTextBox.Text;
            _roomToEdit.Nprice = decimal.Parse(RoomPriceTextBox.Text);
            _roomToEdit.TypeR = (RoomType)Enum.Parse(typeof(RoomType), (RoomTypeComboBox.SelectedItem as ComboBoxItem).Content.ToString());
            _roomToEdit.Status = (RoomStatus)Enum.Parse(typeof(RoomStatus), (RoomStatusComboBox.SelectedItem as ComboBoxItem).Content.ToString());

            // Save the room changes (you would usually save this to a database or collection)
            this.DialogResult = true;
            this.Close();
        }
    }
}
