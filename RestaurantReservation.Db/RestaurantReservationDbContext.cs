using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.models;

namespace RestaurantReservation.Db;

public class RestaurantReservationDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Table> Tables { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=192.168.1.104,1433;Database=RestaurantReservationCore;User=testuser;Password=Sholi@971;TrustServerCertificate=True;");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Reservation)
            .WithMany(r => r.Orders)
            .HasForeignKey(o => o.ReservationId)
            .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Employee)
            .WithMany(e => e.Orders)
            .HasForeignKey(o => o.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict); 
            
        modelBuilder.Entity<Table>()
            .HasOne(r => r.Restaurant)
            .WithMany(t => t.Tables)
            .HasForeignKey(r => r.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Table>()
            .HasOne(r => r.Reservation)
            .WithMany(t => t.Tables)
            .HasForeignKey(r => r.ReservationId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<MenuItem>()
            .HasOne(r => r.Restaurant)
            .WithMany(mi => mi.MenuItems)
            .HasForeignKey(r => r.RestaurantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}