using Hotel_Management.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Manager> Managers { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Rooms> Rooms { get; set; } = null!;
    public DbSet<Employee> Employee { get; set; } = null!;
    public DbSet<ReservationModel> Reservation { get; set; } = null!;




    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLExpress;Database=HotelDB;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Table mappings
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Client>().ToTable("Clients");
        modelBuilder.Entity<Manager>().ToTable("Managers");
        modelBuilder.Entity<Employee>().ToTable("Employees");
        modelBuilder.Entity<Rooms>().ToTable("Rooms");

        
    }
}
