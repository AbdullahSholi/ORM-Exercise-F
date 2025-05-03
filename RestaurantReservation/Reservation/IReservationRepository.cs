namespace RestaurantReservation.Reservation;

using RestaurantReservation;
using Db.models;

public interface IReservationRepository : IRepository<Reservation>
{
    Task<List<Reservation>> GetReservationsByCustomerAsync(int customerId);
}