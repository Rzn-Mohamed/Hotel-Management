using Hotel_Management.Models;
using System.Collections.Generic;
using System.Linq;

namespace Hotel_Management.Services
{
    public class RoomsService
    {
        private readonly AppDbContext _context;

        public RoomsService(AppDbContext context)
        {
            _context = context;
        }

        public List<Rooms> GetAllRooms()
        {
            return _context.Rooms.ToList();
        }
    }
}
