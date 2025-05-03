using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db;
using RestaurantReservation.Db.models;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    private readonly RestaurantReservationDbContext _context;
    public CustomerRepository(RestaurantReservationDbContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<List<Customer>> GetCustomersWithLargePartySizeAsync(int minPartySize)
    {
        return await _context.Customers
            .FromSqlInterpolated($"EXEC dbo.GetCustomersWithLargePartySize @MinPartySize = {minPartySize}")
            .AsNoTracking()
            .ToListAsync();
    }
}