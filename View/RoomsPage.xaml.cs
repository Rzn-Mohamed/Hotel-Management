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
            LoadRooms();  
        }

        private void AddRoomButton_Click(object sender, RoutedEventArgs e)
        {
            AddRoomWindow addRoomWindow = new AddRoomWindow(_rooms);
            if (addRoomWindow.ShowDialog() == true)
            {
               
                LoadRooms();  
            }
        }

        private void EditRoomButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedRoom = (Rooms)RoomsListView.SelectedItem;
            if (selectedRoom != null)
            {
                EditRoomWindow editRoomWindow = new EditRoomWindow(selectedRoom);

                if (editRoomWindow.ShowDialog() == true)
                {
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
                    _rooms.Remove(room);

                    using (var dbContext = new AppDbContext())
                    {
                        dbContext.Rooms.Remove(room);  
                        dbContext.SaveChanges();  
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
                _rooms.Clear();  // Clear the ObservableCollection
                foreach (var room in rooms)
                {
                    _rooms.Add(room);  // Add each room to the collection
                }
            }
        }


        private void AddRoom(Rooms newRoom)
        {
            try
            {
                using (var dbContext = new AppDbContext())
                {
                    dbContext.Rooms.Add(newRoom); // Add room to database
                    dbContext.SaveChanges();      // Commit changes
                }
                LoadRooms();  // Refresh the displayed room list
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        private void RoomsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
