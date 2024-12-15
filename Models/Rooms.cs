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
        public string NumR { get; set; }
        public decimal Nprice { get; set; } 
        public RoomType TypeR { get; set; } 
        public RoomStatus Status { get; set; } 
        public string PicturePath { get; set; } 
    }
    
}
