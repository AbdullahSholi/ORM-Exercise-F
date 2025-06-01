namespace RestaurantReservation.Db.models;

public class Employee
{
    public long EmployeeId { get; set; }
    public long RestaurantId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public EmployeePosition Position { get; set; }

    public List<Order>? Orders { get; set; } = new();
    public Restaurant Restaurant { get; set; }
}