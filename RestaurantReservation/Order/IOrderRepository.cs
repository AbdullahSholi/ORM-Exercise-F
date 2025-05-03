namespace RestaurantReservation.Order;

using RestaurantReservation;
using Db.models;
public interface IOrderRepository : IRepository<Order> 
{
    Task<List<Order>> ListOrdersAndMenuItemsAsync(int reservationId);
    Task<decimal> CalculateAverageOrderAmountAsync(int employeeId);
}