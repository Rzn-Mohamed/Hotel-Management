using Hotel_Management.Models;
using Hotel_Management.Services;
using Hotel_Management.View;
using Microsoft.Office.Interop.Excel;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Linq;

namespace HotelManagement.Views
{
    /// <summary>
    /// Interaction logic for AddReservation.xaml
    /// </summary>
    public partial class AddReservation : System.Windows.Controls.Page
    {

        public AddReservation()
        {
            InitializeComponent();
            try
            {
                using (var dbContext = new AppDbContext())
                {
                    // Fetch available rooms
                    var availableRooms = dbContext.Rooms.ToList();  // Assuming Room is your model

                    // Set the ComboBox's ItemSource to the available rooms
                    RoomComboBox.ItemsSource = availableRooms;
                }
            }
            catch (Exception ex)
            {
                var errorMessage = new CustomMessageBox($"Error fetching rooms: {ex.Message}");
                errorMessage.ShowDialog();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(GuestNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(GuestEmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(GuestadresseTextBox.Text) ||
                !CheckInDatePicker.SelectedDate.HasValue ||
                !CheckOutDatePicker.SelectedDate.HasValue ||
                RoomComboBox.SelectedItem == null) // Ensure a room is selected
            {
                var errorMessage = new CustomMessageBox("All fields are required!");
                errorMessage.ShowDialog();
                return;
            }

            // Validate email format
            if (!IsValidEmail(GuestEmailTextBox.Text))
            {
                var errorMessage = new CustomMessageBox("Invalid email format!");
                errorMessage.ShowDialog();
                return;
            }

            // Ensure Check-out date is after Check-in date
            if (CheckOutDatePicker.SelectedDate <= CheckInDatePicker.SelectedDate)
            {
                var errorMessage = new CustomMessageBox("Check-out date must be after check-in date.");
                errorMessage.ShowDialog();
                return;
            }

            // Check if the selected room is available for the selected dates
            if (!IsRoomAvailable((int)RoomComboBox.SelectedValue, CheckInDatePicker.SelectedDate.Value, CheckOutDatePicker.SelectedDate.Value))
            {
                var errorMessage = new CustomMessageBox("The selected room is not available for the selected dates.");
                errorMessage.ShowDialog();
                return;
            }

            // Create a new reservation object
            var newReservation = new ReservationModel
            {
                GuestName = GuestNameTextBox.Text.Trim(),
                GuestEmail = GuestEmailTextBox.Text.Trim(),
                Guestadresse = GuestadresseTextBox.Text.Trim(),
                dateDebut = (DateTime)CheckInDatePicker.SelectedDate,
                dateFin = (DateTime)CheckOutDatePicker.SelectedDate,
                Id = (int)RoomComboBox.SelectedValue, // Assume RoomComboBox holds room ids
            };

            try
            {
                using (var dbContext = new AppDbContext())
                {
                    dbContext.Reservation.Add(newReservation);
                    dbContext.SaveChanges();
                }

                // Send confirmation email to client
                SendEmailToClient(newReservation);

                var successMessage = new CustomMessageBox("Reservation added successfully!");
                successMessage.ShowDialog();

                // Navigate back to the reservations page
                NavigationService.Navigate(new Reservation());
            }
            catch (Exception ex)
            {
                var errorMessage = new CustomMessageBox($"Error saving reservation: {ex.Message}\n{ex.StackTrace}");
                errorMessage.ShowDialog();

                if (ex.InnerException != null)
                {
                    var innerError = new CustomMessageBox($"Inner exception: {ex.InnerException.Message}");
                    innerError.ShowDialog();
                }
            }

        }

        private bool IsValidEmail(string email)
        {
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$"; // Basic email pattern
            return Regex.IsMatch(email, emailPattern);
        }

        private bool IsRoomAvailable(int roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            using (var dbContext = new AppDbContext())
            {
                // Check if the room is reserved within the selected date range
                var overlappingReservation = dbContext.Reservation
                    .Where(r => r.Id == roomId &&
                                ((r.dateDebut < checkOutDate && r.dateFin > checkInDate)))
                    .FirstOrDefault();

                return overlappingReservation == null; // Room is available if no overlapping reservation exists
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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle Cancel button click if needed
        }
    }
}
