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

        private void AddReservationButton_Click(Object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddReservation());
        }

        private void DeleteClientButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var reservationId = button?.CommandParameter as int?;

            if (reservationId.HasValue)
            {
                using (var dbContext = new AppDbContext())
                {
                    var reservation = dbContext.Reservation.FirstOrDefault(r => r.Id == reservationId.Value);

                    if (reservation != null)
                    {
                        // Confirm deletion
                        var result = MessageBox.Show("Are you sure you want to delete this reservation?",
                                                     "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (result == MessageBoxResult.Yes)
                        {
                            try
                            {
                                // Remove from the database
                                dbContext.Reservation.Remove(reservation);
                                dbContext.SaveChanges(); // Save changes to the database

                                // Optionally, remove from the in-memory ObservableCollection (UI update)
                                _reservations.Remove(reservation);

                                MessageBox.Show("Reservation deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error deleting reservation: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Reservation not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Invalid reservation ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Reload reservations to update the list
            LoadReservations();
        }
    }
}
