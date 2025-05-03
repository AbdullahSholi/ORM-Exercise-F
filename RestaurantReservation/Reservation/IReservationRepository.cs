namespace RestaurantReservation.Reservation;

public interface IReservationRepository : IRepository<Db.models.Reservation>
{
    Task<List<Db.models.Reservation>> GetReservationsByCustomerAsync(int customerId);
}