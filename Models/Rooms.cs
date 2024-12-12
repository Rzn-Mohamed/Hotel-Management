using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Management.Models
{
    public enum RoomType
    {
        Suite,
        Deluxe,
        Double,
        Standard,
        Junior
    }

    public enum RoomStatus
    {
        Reserved,
        NotReserved
    }

    public class Rooms
    {
        public int Id { get; set; }
        public string NumR { get; set; } // Room Number
        public decimal Nprice { get; set; } // Room Price
        public RoomType TypeR { get; set; } // Room Type
        public RoomStatus Status { get; set; } // Room Status
        public string PicturePath { get; set; } // Room Picture Path
    }
    
}
