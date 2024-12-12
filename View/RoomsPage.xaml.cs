using Hotel_Management.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Hotel_Management.Views
{
    public partial class RoomsPage : Page
    {
        private ObservableCollection<Rooms> _rooms;

        public RoomsPage()
        {
            InitializeComponent();
            _rooms = new ObservableCollection<Rooms>();
            RoomsListView.ItemsSource = _rooms;
            LoadRooms();  // Load rooms from the database when the page is first loaded
        }

        private void AddRoomButton_Click(object sender, RoutedEventArgs e)
        {
            AddRoomWindow addRoomWindow = new AddRoomWindow(_rooms);
            if (addRoomWindow.ShowDialog() == true)
            {
                // The room was added, so refresh the collection
                LoadRooms();  // Fetch the updated list of rooms
            }
        }

        private void EditRoomButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedRoom = (Rooms)RoomsListView.SelectedItem;
            if (selectedRoom != null)
            {
                // Create an instance of the EditRoomWindow and pass the selected room
                EditRoomWindow editRoomWindow = new EditRoomWindow(selectedRoom);

                // Show the edit window and wait for it to be closed
                if (editRoomWindow.ShowDialog() == true)
                {
                    // After editing, refresh the room list
                    LoadRooms();
                }
            }
            else
            {
                MessageBox.Show("Please select a room to edit.");
            }
        }

        private void DeleteRoomButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var room = button?.DataContext as Rooms;

            if (room != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete room {room.NumR}?",
                                              "Delete Room", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    // Remove the room from the ObservableCollection
                    _rooms.Remove(room);

                    // Also remove it from the database
                    using (var dbContext = new AppDbContext())
                    {
                        dbContext.Rooms.Remove(room);  // Remove room from the database
                        dbContext.SaveChanges();  // Save changes to the database
                    }

                    MessageBox.Show("Room deleted successfully!");
                }
            }
        }

        private void LoadRooms()
        {
            using (var dbContext = new AppDbContext())
            {
                var rooms = dbContext.Rooms.ToList();  // Fetch rooms from the database
                _rooms.Clear();  // Clear the existing collection
                foreach (var room in rooms)
                {
                    _rooms.Add(room);  // Add each room from the database to the ObservableCollection
                }
            }
        }

        private void RoomsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // This method can be used if you want to handle selection changes
        }
    }
}
