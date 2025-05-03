using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace RestaurantReservation.Db;

public class RestaurantReservationDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=192.168.1.104,1433;Database=PubDatabase;User=testuser;Password=Sholi@971;TrustServerCertificate=True;");
    }
}