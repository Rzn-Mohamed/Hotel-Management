using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Hotel_Management.Models;
using Hotel_Management.View;
using Hotel_Management.Views;

namespace Hotel_Management.Views
{
    public partial class RoomsPage : Page
    {
        private AppDbContext _dbContext;
        private string _userRole;

        

        public RoomsPage()
        {
            InitializeComponent();
            _dbContext = new AppDbContext();
            _userRole = string.Empty;
            LoadRooms();
        }

        private void ApplyRoleRestrictions()
        {
            if (_userRole == "Employee")
            {
                var deleteRoomButton = (Button)FindName("DeleteRoomButton");
                if (deleteRoomButton != null)
                {
                    deleteRoomButton.IsEnabled = false;
                }
            }
        }



        private void LoadRooms()
        {
            RoomsListView.ItemsSource = _dbContext.Rooms.ToList();
        }

        private void AddRoomButton_Click(object sender, RoutedEventArgs e)
        {
            var addRoomWindow = new AddEditRoomWindow();
            if (addRoomWindow.ShowDialog() == true)
            {
                var newRoom = new Rooms
                {
                    NumR = addRoomWindow.Room.NumR,
                    Nprice = addRoomWindow.Room.Nprice,
                    TypeR = addRoomWindow.Room.TypeR,
                    Status = addRoomWindow.Room.Status,
                    PicturePath = addRoomWindow.Room.PicturePath
                };

                _dbContext.Rooms.Add(newRoom);
                _dbContext.SaveChanges();
                LoadRooms();
            }
        }

        private void EditRoomButton_Click(object sender, RoutedEventArgs e)
        {
            if (RoomsListView.SelectedItem is Rooms selectedRoom)
            {
                var editRoomWindow = new AddEditRoomWindow(selectedRoom);
                if (editRoomWindow.ShowDialog() == true)
                {
                    selectedRoom.NumR = editRoomWindow.Room.NumR;
                    selectedRoom.Nprice = editRoomWindow.Room.Nprice;
                    selectedRoom.TypeR = editRoomWindow.Room.TypeR;
                    selectedRoom.Status = editRoomWindow.Room.Status;
                    selectedRoom.PicturePath = editRoomWindow.Room.PicturePath;

                    _dbContext.Entry(selectedRoom).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _dbContext.SaveChanges();
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
            if (_userRole == "Employee")
            {
                MessageBox.Show("You do not have permission to delete rooms.");
                return;
            }

            if (RoomsListView.SelectedItem is Rooms selectedRoom)
            {
                var result = MessageBox.Show($"Are you sure you want to delete room {selectedRoom.NumR}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _dbContext.Rooms.Remove(selectedRoom);
                    _dbContext.SaveChanges();
                    LoadRooms();
                }
            }
            else
            {
                MessageBox.Show("Please select a room to delete.");
            }
        }


        private void RoomsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RoomsListView.SelectedItem is Rooms selectedRoom)
            {
            }
        }

        private void SeeMoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (RoomsListView.SelectedItem is Rooms selectedRoom)
            {
                var roomDetailsPage = new RoomDetailsPage(selectedRoom);
                NavigationService?.Navigate(roomDetailsPage);
            }
            else
            {
                MessageBox.Show("Please select a room to view details.");
            }
        }

    }
}
