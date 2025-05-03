using RestaurantReservation.Db.models;

namespace RestaurantReservation;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<List<Employee>> ListManagersAsync();
}