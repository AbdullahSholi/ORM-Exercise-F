using RestaurantReservation.Db.models;

public interface IMenuItemRepository : IRepository<MenuItem>
{
    Task<List<MenuItem>> ListOrderedMenuItemsAsync(int reservationId);
}