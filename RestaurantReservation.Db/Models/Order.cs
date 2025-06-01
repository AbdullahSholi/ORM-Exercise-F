namespace RestaurantReservation.Db.models;

public class Order
{
    public long OrderId { get; set; }
    public long ReservationId { get; set; }
    public long? EmployeeId { get; set; }
    public DateTime OrderDate { get; set; }
    public int TotalAmount { get; set; }

    public Reservation Reservation { get; set; }
    public Employee Employee { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
}