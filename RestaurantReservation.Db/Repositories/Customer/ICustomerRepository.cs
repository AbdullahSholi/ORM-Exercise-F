using RestaurantReservation.Db.models;

public interface ICustomerRepository : IRepository<Customer>
{
    public Task<List<Customer>> GetCustomersWithLargePartySizeAsync(int minPartySize);
}