namespace RestaurantReservation.Db.models;

public class MenuItem
{
    public long MenuItemId { get; set; }
    public long RestaurantId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }

    public Restaurant Restaurant { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
}