using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db;

namespace RestaurantReservation.MenuItem;

public class MenuItemRepository : Repository<Db.models.MenuItem>, IMenuItemRepository
{
    private readonly RestaurantReservationDbContext _context;

    public MenuItemRepository(RestaurantReservationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Db.models.MenuItem>> ListOrderedMenuItemsAsync(int reservationId)
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