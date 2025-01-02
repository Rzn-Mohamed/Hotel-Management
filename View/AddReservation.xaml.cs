using Hotel_Management.Models;
using Hotel_Management.Services;
using Hotel_Management.View;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace HotelManagement.Views
{
    /// <summary>
    /// Interaction logic for AddReservation.xaml
    /// </summary>
    public partial class AddReservation : Page
    {
        public AddReservation()
        {
            InitializeComponent();
            CheckInDatePicker.SelectedDateChanged += OnDateChanged;
            CheckOutDatePicker.SelectedDateChanged += OnDateChanged;
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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(GuestNameTextBox.Text) ||
               string.IsNullOrWhiteSpace(GuestEmailTextBox.Text) ||
               string.IsNullOrWhiteSpace(GuestadresseTextBox.Text) ||
               !CheckInDatePicker.SelectedDate.HasValue ||
               !CheckOutDatePicker.SelectedDate.HasValue ||
               RoomComboBox.SelectedItem == null)
            {
                var errorMessage = new CustomMessageBox("All fields are required!");
                errorMessage.ShowDialog();
                return;
            }

            if (!IsValidEmail(GuestEmailTextBox.Text))
            {
                var errorMessage = new CustomMessageBox("Invalid email format!");
                errorMessage.ShowDialog();
                return;
            }

            if (CheckOutDatePicker.SelectedDate <= CheckInDatePicker.SelectedDate)
            {
                var errorMessage = new CustomMessageBox("Check-out date must be after check-in date.");
                errorMessage.ShowDialog();
                return;
            }

            var selectedRoom = (Rooms)RoomComboBox.SelectedItem;

            if (!IsRoomAvailable(selectedRoom.Id, CheckInDatePicker.SelectedDate.Value, CheckOutDatePicker.SelectedDate.Value))
            {
                var errorMessage = new CustomMessageBox("The selected room is not available for the selected dates.");
                errorMessage.ShowDialog();
                return;
            }

            var newReservation = new ReservationModel
            {
                GuestName = GuestNameTextBox.Text.Trim(),
                GuestEmail = GuestEmailTextBox.Text.Trim(),
                Guestadresse = GuestadresseTextBox.Text.Trim(),
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

        private bool IsValidEmail(string email)
        {
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
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
    }
}
