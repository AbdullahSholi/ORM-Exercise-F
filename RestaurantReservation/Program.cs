using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestaurantReservation.Db;
using RestaurantReservation.Db.models;

public class Program
{
    public static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddDbContext<RestaurantReservationDbContext>(options =>
                options.UseSqlServer(
                    "Server=192.168.1.104,1433;Database=RestaurantReservationCore;User=testuser;Password=Sholi@971;TrustServerCertificate=True;"))
            .BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<RestaurantReservationDbContext>();
        await context.Database.MigrateAsync();
        await context.SeedDataAsync();

        var customerRepository = new CustomerRepository(context);
        await customerRepository.AddAsync(new Customer
        {
            FirstName = "Abdullah", LastName = "Sholi", Email = "abdullah.ghassan.sholi@gmail.com",
            PhoneNumber = "999-3333"
        });
        var customer = customerRepository.GetByIdAsync(3).Result.FirstName;
        Console.WriteLine(customer);
        await customerRepository.DeleteAsync(11);
        var customers = customerRepository.GetAllAsync().Result;
        foreach (var c in customers)
            Console.WriteLine(c.FirstName + " " + c.LastName + " " + c.Email + " " + c.PhoneNumber);

        Console.WriteLine();
        var employeeRepository = new EmployeeRepository(context);
        var managers = await employeeRepository.ListManagersAsync();
        foreach (var manager in managers) Console.WriteLine(manager.FirstName + " " + manager.LastName);

        Console.WriteLine();
        var reservationRepository = new ReservationRepository(context);
        var reservationsByCustomer = await reservationRepository.GetReservationsByCustomerAsync(4);
        foreach (var reservation in reservationsByCustomer) Console.WriteLine(reservation.ReservationId);

        Console.WriteLine();
        var orderRepository = new OrderRepository(context);
        var listOrdersAndMenuItems = await orderRepository.ListOrdersAndMenuItemsAsync(3);
        foreach (var element in listOrdersAndMenuItems)
        {
            Console.WriteLine("Order Id: " + element.OrderId + " ");
            foreach (var orderItem in element.OrderItems)
                Console.WriteLine("     Menu Items: " + orderItem.MenuItem.Name);
        }

        Console.WriteLine();
        var menuItemRepository = new MenuItemRepository(context);
        var listOrderedMenuItems = await menuItemRepository.ListOrderedMenuItemsAsync(3);
        foreach (var element in listOrderedMenuItems) Console.WriteLine(element.MenuItemId + " " + element.Name);

        Console.WriteLine();
        var averageOrderAmount = orderRepository.CalculateAverageOrderAmountAsync(5);
        Console.WriteLine("Average Order Amount: " + averageOrderAmount.Result);

        Console.WriteLine();
        var viewReservationsWithAssociatedInformation =
            await context.ReservationsWithAssociatedInformation.ToListAsync();
        foreach (var element in viewReservationsWithAssociatedInformation)
            Console.WriteLine(element.CustomerId + " " + element.FirstName + " " + element.LastName + " " +
                              element.ReservationId + " " + element.ReservationDate + " " + element.RestaurantId + " " +
                              element.Name);

        Console.WriteLine();
        var viewEmployeesWithAssociatedInformation =
            await context.EmployeesWithAssociatedInformation.ToListAsync();
        foreach (var element in viewEmployeesWithAssociatedInformation)
            Console.WriteLine(element.FirstName + " " + element.LastName + " " + element.EmployeeId + " " +
                              element.Position + " " + element.PhoneNumber + " " + element.RestaurantId + " " +
                              element.Name);

        Console.WriteLine();
        var restaurantId = 1;
        var revenue = await context.TotalRevenueResults
            .FromSqlInterpolated(
                $"SELECT dbo.TotalRevenueGeneratedBySpecificRestaurant({restaurantId}) AS TotalRevenue")
            .AsNoTracking()
            .FirstOrDefaultAsync();
        Console.WriteLine(revenue.TotalRevenue);

        Console.WriteLine();
        var customersByPartySize = await customerRepository.GetCustomersWithLargePartySizeAsync(4);
        foreach (var element in customersByPartySize)
            Console.WriteLine(element.CustomerId + " " + element.FirstName + " " + element.LastName + " " +
                              element.Email + " " + element.PhoneNumber);
    }
}