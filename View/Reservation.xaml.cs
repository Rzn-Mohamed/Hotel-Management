using Hotel_Management.Models;
using HotelManagement.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

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

        private void GeneratePdfReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReservationDataGrid.SelectedItem is ReservationModel selectedReservation)
            {
                using (var dbContext = new AppDbContext())
                {
                    var room = dbContext.Rooms.FirstOrDefault(r => r.Id == selectedReservation.RoomId);
                    if (room != null)
                    {
                        var totalPrice = (selectedReservation.dateFin - selectedReservation.dateDebut).Days * room.Nprice;
                        GeneratePdfReport(selectedReservation, totalPrice);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a reservation to generate the report.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GeneratePdfReport(ReservationModel reservation, decimal totalPrice)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "PDF Files|*.pdf",
                Title = "Save PDF Report"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (var stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    var document = new Document(PageSize.A4, 50, 50, 25, 25);
                    PdfWriter.GetInstance(document, stream);
                    document.Open();

                    // Add hotel logo or name
                    var hotelNameFont = FontFactory.GetFont("Helvetica", 24, BaseColor.DARK_GRAY);
                    var hotelName = new Paragraph("HOTEL MANAGEMENT SYSTEM", hotelNameFont)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 20
                    };
                    document.Add(hotelName);

                    // Add reservation title
                    var titleFont = FontFactory.GetFont("Helvetica", 18, BaseColor.DARK_GRAY);
                    var title = new Paragraph("Reservation Details", titleFont)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 30
                    };
                    document.Add(title);

                    // Create main table
                    var table = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        SpacingBefore = 10,
                        SpacingAfter = 20
                    };
                    table.SetWidths(new float[] { 1, 2 });

                    // Define fonts and colors
                    var headerFont = FontFactory.GetFont("Helvetica", 12, Font.BOLD, BaseColor.WHITE);
                    var cellFont = FontFactory.GetFont("Helvetica", 11, BaseColor.DARK_GRAY);
                    var headerBackgroundColor = new BaseColor(41, 128, 185); // Nice blue color

                    // Add header row
                    AddTableHeader(table, "Reservation Information", headerFont, headerBackgroundColor, 2);

                    // Add styled cells
                    AddStyledRow(table, "Guest Name", reservation.GuestName, cellFont);
                    AddStyledRow(table, "Email", reservation.GuestEmail, cellFont);
                    AddStyledRow(table, "Address", reservation.Guestadresse, cellFont);
                    AddStyledRow(table, "Check-in Date", reservation.dateDebut.ToString("MM/dd/yyyy"), cellFont);
                    AddStyledRow(table, "Check-out Date", reservation.dateFin.ToString("MM/dd/yyyy"), cellFont);
                    AddStyledRow(table, "Total Price", $"{totalPrice} Dh", cellFont);

                    document.Add(table);

                    // Add footer
                    var footerFont = FontFactory.GetFont("Helvetica", 10, BaseColor.GRAY);
                    var footer = new Paragraph($"Generated on {DateTime.Now:MM/dd/yyyy HH:mm:ss}", footerFont)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingBefore = 30
                    };
                    document.Add(footer);

                    document.Close();
                }

                MessageBox.Show("PDF report generated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void AddTableHeader(PdfPTable table, string headerText, Font font, BaseColor backgroundColor, int colspan)
        {
            var cell = new PdfPCell(new Phrase(headerText, font))
            {
                BackgroundColor = backgroundColor,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 8,
                Colspan = colspan
            };
            table.AddCell(cell);
        }

        private void AddStyledRow(PdfPTable table, string label, string value, Font font)
        {
            var labelCell = new PdfPCell(new Phrase(label, font))
            {
                BackgroundColor = new BaseColor(245, 245, 245),
                PaddingLeft = 5,
                PaddingTop = 8,
                PaddingBottom = 8,
                BorderColor = BaseColor.LIGHT_GRAY
            };

            var valueCell = new PdfPCell(new Phrase(value, font))
            {
                PaddingLeft = 5,
                PaddingTop = 8,
                PaddingBottom = 8,
                BorderColor = BaseColor.LIGHT_GRAY
            };

            table.AddCell(labelCell);
            table.AddCell(valueCell);
        }
    }
}
