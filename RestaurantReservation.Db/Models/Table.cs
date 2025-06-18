namespace RestaurantReservation.Db.models;

public class Table
{
    public int TableId { get; set; }
    public int RestaurantId { get; set; }
    public int ReservationId { get; set; }
    public int Capacity { get; set; }

    public Reservation? Reservation { get; set; }
    public Restaurant? Restaurant { get; set; }
}