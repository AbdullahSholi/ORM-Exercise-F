namespace RestaurantReservation.Db.ViewModels;

public class ReservationsWithAssociatedInformation
{
    public int CustomerId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int ReservationId { get; set; }
    public DateTime ReservationDate { get; set; }
    public int RestaurantId { get; set; }
    public string Name { get; set; } = string.Empty;
}