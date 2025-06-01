namespace RestaurantReservation.Db.models;

public class Reservation
{
    public long ReservationId { get; set; }
    public long CustomerId { get; set; }
    public long RestaurantId { get; set; }
    public DateTime ReservationDate { get; set; }
    public int PartySize { get; set; }

    public Customer Customer { get; set; }
    public Restaurant Restaurant { get; set; }
    public List<Table> Tables { get; set; } = new();
    public List<Order> Orders { get; set; } = new();
}