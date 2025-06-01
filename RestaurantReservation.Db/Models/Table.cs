namespace RestaurantReservation.Db.models;

public class Table
{
    public long TableId { get; set; }
    public long RestaurantId { get; set; }
    public long ReservationId { get; set; }
    public int Capacity { get; set; }

    public Reservation Reservation { get; set; }
    public Restaurant Restaurant { get; set; }
}