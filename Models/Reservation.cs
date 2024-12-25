using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Management.Models
{
    public class ReservationModel
    {
        public int Id { get; set; }
        public DateTime dateDebut { get; set; }

        public DateTime dateFin { get; set; }
        public String GuestName { get; set; }
    }
}
