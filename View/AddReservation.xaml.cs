using Hotel_Management.Models;
using Hotel_Management.Services;
using Hotel_Management.View;
using Microsoft.Office.Interop.Excel;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;


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
            
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(GuestNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(GuestEmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(GuestadresseTextBox.Text) ||
                !CheckInDatePicker.SelectedDate.HasValue ||
                !CheckOutDatePicker.SelectedDate.HasValue)
            {
                var errorMessage = new CustomMessageBox("All fields are required!");
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

                // Navigate back to the reservations page
                NavigationService.Navigate(new Reservation());
            }
            catch (Exception ex)
            {
                var errorMessage = new CustomMessageBox($"Error saving reservation: {ex.Message}");
                errorMessage.ShowDialog();
            }

            //------------------------------------------------------------------------------------
            
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

        }
    }
}
