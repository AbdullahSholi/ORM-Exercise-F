using RestaurantReservation.Db.models;

public interface IReservationRepository : IRepository<Reservation>
{
    Task<List<Reservation>> GetReservationsByCustomerAsync(int customerId);
}