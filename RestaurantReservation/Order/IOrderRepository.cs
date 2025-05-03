namespace RestaurantReservation.Order;

public interface IOrderRepository : IRepository<Db.models.Order>
{
    Task<List<Db.models.Order>> ListOrdersAndMenuItemsAsync(int reservationId);
    Task<decimal> CalculateAverageOrderAmountAsync(int employeeId);
}