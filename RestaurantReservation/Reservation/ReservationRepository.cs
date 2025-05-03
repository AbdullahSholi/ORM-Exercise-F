using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db;

namespace RestaurantReservation.Reservation;

public class ReservationRepository : Repository<Db.models.Reservation>, IReservationRepository
{
    private readonly RestaurantReservationDbContext _context;

    public ReservationRepository(RestaurantReservationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Db.models.Reservation>> GetReservationsByCustomerAsync(int customerId)
    {
        return await _context.Reservations.Where(r => r.CustomerId == customerId).AsNoTracking().ToListAsync();
    }
}