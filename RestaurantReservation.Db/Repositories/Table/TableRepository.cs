using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db;
using RestaurantReservation.Db.models;

public class TableRepository : Repository<Table>, ITableRepository
{
    private readonly RestaurantReservationDbContext _context;
    
    public TableRepository(RestaurantReservationDbContext context) : base(context)
    {
        _context = context;
    }
}