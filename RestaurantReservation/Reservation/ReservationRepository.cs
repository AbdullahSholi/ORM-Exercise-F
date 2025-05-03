using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db;

namespace RestaurantReservation.Reservation;

using RestaurantReservation;
using Db.models;

public class ReservationRepository : Repository<Reservation>, IReservationRepository
{
    private readonly RestaurantReservationDbContext _context;
    public ReservationRepository(RestaurantReservationDbContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<List<Reservation>> GetReservationsByCustomerAsync(int customerId)
    {
        return await _context.Reservations.Where(r => r.CustomerId == customerId).AsNoTracking().ToListAsync();
    }
}