using RestaurantReservation.Db.models;

namespace RestaurantReservation.Db.ViewModels;

public class EmployeesWithAssociatedInformation
{
    public int EmployeeId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public EmployeePosition Position { get; set; } = EmployeePosition.Manager;
    public int RestaurantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string OpeningHours { get; set; } = string.Empty;
}