using Hotel_Management.Models;
using HotelManagement.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Hotel_Management.View
{
    /// <summary>
    /// Interaction logic for Reservation.xaml
    /// </summary>
    public partial class Reservation : Page
    {
        private ObservableCollection<ReservationModel> _reservations;

        public Reservation()
        {
            InitializeComponent(); // Initialize the UI components first
            _reservations = new ObservableCollection<ReservationModel>();
            ReservationDataGrid.ItemsSource = _reservations; // Bind data to DataGrid
            LoadReservations(); // Load reservations from the database
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Handle DataGrid selection changes if needed
        }

        private void LoadReservations()
        {
            using (var dbContext = new AppDbContext())
            {
                var reservations = dbContext.Reservation.ToList(); 
                _reservations.Clear(); 
                foreach (var reservation in reservations)
                {
                    _reservations.Add(reservation); 
                }
            }
        }

        private void AddReservationButton_Click(Object sender,RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddReservation());
        }

      

       



    }
}
