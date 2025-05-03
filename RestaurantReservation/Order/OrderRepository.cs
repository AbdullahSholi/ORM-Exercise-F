using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db;

namespace RestaurantReservation.Order;

using RestaurantReservation;
using Db.models;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    private readonly RestaurantReservationDbContext _context;
    public OrderRepository(RestaurantReservationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Order>> ListOrdersAndMenuItemsAsync(int reservationId)
    {
        var result = await _context.Orders
            .Where(o => o.ReservationId == reservationId)  
            .Include(o => o.Reservation)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
            .AsNoTracking()
            .ToListAsync();
        return result;
    }

    public async Task<decimal> CalculateAverageOrderAmountAsync(int employeeId)
    {
        var result = await _context.Orders
            .Where(o => o.EmployeeId == employeeId)
            .AverageAsync(o => o.TotalAmount);
        return Convert.ToDecimal(result);
    }
}