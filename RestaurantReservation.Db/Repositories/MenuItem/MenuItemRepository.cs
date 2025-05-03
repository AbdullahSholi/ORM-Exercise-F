using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db;
using RestaurantReservation.Db.models;

public class MenuItemRepository : Repository<MenuItem>, IMenuItemRepository
{
    private readonly RestaurantReservationDbContext _context;

    public MenuItemRepository(RestaurantReservationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<MenuItem>> ListOrderedMenuItemsAsync(int reservationId)
    {
        var result = await _context.OrderItems
            .Where(oi => oi.Order.ReservationId == reservationId)
            .Include(oi => oi.MenuItem)
            .Select(oi => oi.MenuItem)
            .AsNoTracking()
            .ToListAsync();
        return result;
    }
}