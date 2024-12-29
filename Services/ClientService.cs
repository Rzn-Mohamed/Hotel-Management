using Hotel_Management.Models;
using HotelManagement.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Hotel_Management.Services
{
    public class ClientService: Window
    {
        private readonly AppDbContext _context;

        public ClientService(AppDbContext context)
        {
            _context = context;
        }

        public List<Client> GetAllClients()
        {
            return _context.Clients.ToList(); 
        }

        public void DeleteClient(int clientId)
        {
           
           
            var client = _context.Clients.Find(clientId);
            if (client != null)
            {
                _context.Clients.Remove(client);
                _context.SaveChanges();
           
            }
        }

        public void AddClient(Client client)
        {
            _context.Clients.Add(client);
            _context.SaveChanges();
        }

        public bool ClientExists(string email)
        {
            return _context.Clients.Any(c => c.Email == email);
        }

    }
}
