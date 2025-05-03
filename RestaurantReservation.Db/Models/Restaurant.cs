namespace RestaurantReservation.Db.models;

public class Restaurant
{
    public int RestaurantId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string OpeningHours { get; set; }

    public List<Table> Tables { get; set; } = new();
    public List<Reservation> Reservations { get; set; } = new();
    public List<Employee> Employees { get; set; } = new();
    public List<MenuItem> MenuItems { get; set; } = new();
}