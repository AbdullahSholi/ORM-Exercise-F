namespace RestaurantReservation.Db.models;

public class OrderItem
{
    public long OrderItemId { get; set; }
    public long OrderId { get; set; }
    public long MenuItemId { get; set; }
    public int Quantity { get; set; }

    public Order Order { get; set; }
    public MenuItem MenuItem { get; set; }
}