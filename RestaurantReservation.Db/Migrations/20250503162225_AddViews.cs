using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                CREATE VIEW vw_ListReservationsWithAssociatedCustomerAndRestaurantInformation AS
                SELECT c.CustomerId, c.FirstName, c.LastName, r.ReservationId, r.ReservationDate, rest.RestaurantId, rest.Name
                FROM Reservations AS r 
                INNER JOIN Customers AS c
                ON r.CustomerId = c.CustomerId
                INNER JOIN Restaurants AS rest
                ON r.RestaurantId = rest.RestaurantId;
                """);
            migrationBuilder.Sql(
                """
                CREATE VIEW vw_EmployeesWithAssociatedRestaurantInformation AS
                SELECT e.EmployeeId, e.FirstName, e.LastName, e.Position, r.RestaurantId, r.Name, r.Address, r.PhoneNumber, r.OpeningHours
                FROM Employees AS e
                INNER JOIN Restaurants AS r
                ON e.RestaurantId = r.RestaurantId;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW vw_ListReservationsWithAssociatedCustomerAndRestaurantInformation");
            migrationBuilder.Sql("DROP VIEW vw_EmployeesWithAssociatedRestaurantInformation");
        }
    }
}
