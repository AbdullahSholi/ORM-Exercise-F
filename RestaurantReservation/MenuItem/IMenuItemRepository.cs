namespace RestaurantReservation.MenuItem;

public interface IMenuItemRepository : IRepository<Db.models.MenuItem>
{
    Task<List<Db.models.MenuItem>> ListOrderedMenuItemsAsync(int reservationId);
}