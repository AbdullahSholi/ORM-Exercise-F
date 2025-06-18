using RestaurantReservation.Db.models;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<List<Employee>> ListManagersAsync();
}