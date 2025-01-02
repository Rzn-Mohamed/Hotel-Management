using Hotel_Management.Models;
using Hotel_Management.Services;
using Hotel_Management.View;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace HotelManagement.Views
{
    /// <summary>
    /// Interaction logic for AddReservation.xaml
    /// </summary>
    public partial class AddReservation : Page
    {
        private readonly ClientService _clientService;
        private List<Client> _allClients; 

        public AddReservation()
        {
            InitializeComponent();
            _clientService = new ClientService(new AppDbContext());
            CheckInDatePicker.SelectedDateChanged += OnDateChanged;
            CheckOutDatePicker.SelectedDateChanged += OnDateChanged;
            LoadClients();
        }

        private void OnDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CheckInDatePicker.SelectedDate.HasValue && CheckOutDatePicker.SelectedDate.HasValue)
            {
                LoadAvailableRooms(CheckInDatePicker.SelectedDate.Value, CheckOutDatePicker.SelectedDate.Value);
            }
        }

        private void LoadAvailableRooms(DateTime checkInDate, DateTime checkOutDate)
        {
            try
            {
                using (var dbContext = new AppDbContext())
                {
                    var availableRooms = dbContext.Rooms.Where(room =>
                        !dbContext.Reservation.Any(reservation =>
                            reservation.RoomId == room.Id &&
                            checkInDate < reservation.dateFin &&
                            checkOutDate > reservation.dateDebut)).ToList();

                    RoomComboBox.ItemsSource = availableRooms;
                }
            }
            catch (Exception ex)
            {
                var errorMessage = new CustomMessageBox($"Error fetching available rooms: {ex.Message}");
                errorMessage.ShowDialog();
            }
        }

        private void LoadClients()
        {
            try
            {
                _allClients = _clientService.GetAllClients().ToList();
                ClientComboBox.ItemsSource = _allClients;
            }
            catch (Exception ex)
            {
                var errorMessage = new CustomMessageBox($"Error fetching clients: {ex.Message}");
                errorMessage.ShowDialog();
            }
        }

        private void SearchClientTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchClientTextBox.Text.ToLower();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                ClientComboBox.ItemsSource = _allClients;
            }
            else
            {
                var filteredClients = _allClients.Where(c => 
                    c.Name.ToLower().Contains(searchText) || 
                    c.Email.ToLower().Contains(searchText) ||
                    c.Address.ToLower().Contains(searchText)
                ).ToList();
                ClientComboBox.ItemsSource = filteredClients;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientComboBox.SelectedItem == null ||
                !CheckInDatePicker.SelectedDate.HasValue ||
                !CheckOutDatePicker.SelectedDate.HasValue ||
                RoomComboBox.SelectedItem == null)
            {
                var errorMessage = new CustomMessageBox("All fields are required!");
                errorMessage.ShowDialog();
                return;
            }

            var selectedClient = (Client)ClientComboBox.SelectedItem;
            var selectedRoom = (Rooms)RoomComboBox.SelectedItem;

            if (CheckOutDatePicker.SelectedDate <= CheckInDatePicker.SelectedDate)
            {
                var errorMessage = new CustomMessageBox("Check-out date must be after check-in date.");
                errorMessage.ShowDialog();
                return;
            }

            if (!IsRoomAvailable(selectedRoom.Id, CheckInDatePicker.SelectedDate.Value, CheckOutDatePicker.SelectedDate.Value))
            {
                var errorMessage = new CustomMessageBox("The selected room is not available for the selected dates.");
                errorMessage.ShowDialog();
                return;
            }

            var newReservation = new ReservationModel
            {
                GuestName = selectedClient.Name,
                GuestEmail = selectedClient.Email,
                Guestadresse = selectedClient.Address,
                RoomId = selectedRoom.Id,
                dateDebut = CheckInDatePicker.SelectedDate.Value,
                dateFin = CheckOutDatePicker.SelectedDate.Value,
            };

            try
            {
                using (var dbContext = new AppDbContext())
                {
                    dbContext.Reservation.Add(newReservation);
                    dbContext.SaveChanges();
                }

                SendEmailToClient(newReservation);

                var successMessage = new CustomMessageBox("Reservation added successfully!");
                successMessage.ShowDialog();

                NavigationService.Navigate(new Reservation());
            }
            catch (Exception ex)
            {
                var errorMessage = new CustomMessageBox($"Error saving reservation: {ex.Message}");
                errorMessage.ShowDialog();
            }
        }

        private bool IsRoomAvailable(int roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            try
            {
                using (var dbContext = new AppDbContext())
                {
                    return !dbContext.Reservation.Any(reservation =>
                        reservation.RoomId == roomId &&
                        checkInDate < reservation.dateFin &&
                        checkOutDate > reservation.dateDebut);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error checking room availability.", ex);
            }
        }

        private void SendEmailToClient(ReservationModel reservation)
        {
            try
            {
                using (var mail = new System.Net.Mail.MailMessage())
                {
                    mail.From = new System.Net.Mail.MailAddress("anasjabri35@gmail.com");
                    mail.To.Add(reservation.GuestEmail);
                    mail.Subject = "Reservation Confirmation";
                    mail.Body = $"Dear {reservation.GuestName},\n\n" +
                                $"Thank you for making a reservation with us!\n" +
                                $"Check-in Date: {reservation.dateDebut:MM/dd/yyyy}\n" +
                                $"Check-out Date: {reservation.dateFin:MM/dd/yyyy}\n\n" +
                                $"Best regards,\n Mimouna Hotel ";

                    using (var smtpClient = new System.Net.Mail.SmtpClient("smtp.gmail.com"))
                    {
                        smtpClient.Port = 587;
                        smtpClient.Credentials = new System.Net.NetworkCredential("anasjabri35@gmail.com", "brpw ioaf unau zpfp");
                        smtpClient.EnableSsl = true;
                        smtpClient.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                var errorMessage = new CustomMessageBox($"Failed to send email: {ex.Message}");
                errorMessage.ShowDialog();
            }
        }

        private void AddNewClientButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddClientPage());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Reservation());
        }
    }
}
