namespace RestaurantReservation.Db.models;

public class Reservation
{
    public int ReservationId { get; set; }
    public int CustomerId { get; set; }
    public int RestaurantId { get; set; }
    public DateTime ReservationDate { get; set; }
    public int PartySize { get; set; }
    
    public Customer Customer { get; set; }
    public Restaurant Restaurant { get; set; }
    public List<Table> Tables { get; set; } = new();
    public List<Order> Orders { get; set; } = new();
}