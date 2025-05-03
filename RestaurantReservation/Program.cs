using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db;
public class Program
{
    public static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddDbContext<RestaurantReservationDbContext>(options =>
                options.UseSqlServer("Server=192.168.1.104,1433;Database=RestaurantReservationCore;User=testuser;Password=Sholi@971;TrustServerCertificate=True;"))
            .BuildServiceProvider();
        
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<RestaurantReservationDbContext>();
            
            await context.Database.MigrateAsync();

            await context.SeedDataAsync();
        }
    }
}