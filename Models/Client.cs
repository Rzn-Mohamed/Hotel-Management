namespace Hotel_Management.Models
{
    public class Client : User
    {
        public bool isClient { get; set; } = true;
        public string Gender { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
