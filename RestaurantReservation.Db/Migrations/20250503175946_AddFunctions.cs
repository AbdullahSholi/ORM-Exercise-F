using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddFunctions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                 CREATE FUNCTION dbo.TotalRevenueGeneratedBySpecificRestaurant (@RestaurantId INT)
                                 RETURNS DECIMAL(18, 2)
                                 AS 
                                 BEGIN
                                 	DECLARE @TotalRevenue DECIMAL(18, 2);
                                 
                                 	SELECT @TotalRevenue = SUM(oi.Quantity * mi.Price)
                                 	FROM Restaurants AS r
                                 	INNER JOIN MenuItems AS mi
                                 	ON r.RestaurantId = mi.RestaurantId
                                 	INNER JOIN OrderItems AS oi
                                 	ON mi.MenuItemId = oi.MenuItemId
                                 	WHERE r.RestaurantId = @RestaurantId;
                                 
                                 	RETURN ISNULL(@TotalRevenue, 0);
                                 END
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql("DROP Function dbo.TotalRevenueGeneratedBySpecificRestaurant");
        }
    }
}
