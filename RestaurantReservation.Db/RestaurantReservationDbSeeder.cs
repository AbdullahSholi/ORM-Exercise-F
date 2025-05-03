using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.models;

namespace RestaurantReservation.Db;

public class RestaurantReservationDbSeeder
{
    public static async Task SeedAsync<T>(RestaurantReservationDbContext context, string fileName, DbSet<T> dbSet) where T : class
    {
        if (!await dbSet.AnyAsync())
        {
            var path = Path.Combine("../../../../RestaurantReservation.Db","SeedData", $"{fileName}.json");            
            if (File.Exists(path))
            {
                var jsonData = await File.ReadAllTextAsync(path);
                var entities = JsonSerializer.Deserialize<List<T>>(jsonData);

                if (entities != null && entities.Any())
                {
                    dbSet.AddRange(entities);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}