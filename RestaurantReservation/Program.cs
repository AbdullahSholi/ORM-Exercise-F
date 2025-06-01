using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestaurantReservation.Db;
using RestaurantReservation.Db.DataGeneration;
using RestaurantReservation.Db.models;
using RestaurantReservation.Db.Validators;

public class Program
{
    public static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddDbContext<RestaurantReservationDbContext>(options =>
                options.UseSqlServer(
                    "Server=192.168.1.104,1433;Database=RestaurantReservationCore;User=testuser;Password=Sholi@971;TrustServerCertificate=True;"))
            .AddDbContext<RestaurantReservationDbContext>()
            .AddScoped<IValidator<Customer>, CustomerValidator>()
            .AddScoped<IValidator<Employee>, EmployeeValidator>()
            .AddScoped<IValidator<MenuItem>, MenuItemValidator>()
            .AddScoped<IValidator<Order>, OrderValidator>()
            .AddScoped<IValidator<Reservation>, ReservationValidator>()
            .AddScoped<IValidator<Restaurant>, RestaurantValidator>()
            .AddScoped<IValidator<Table>, TableValidator>()
            .AddScoped<ICustomerRepository, CustomerRepository>()
            .AddScoped<IEmployeeRepository, EmployeeRepository>()
            .AddScoped<IMenuItemRepository, MenuItemRepository>()
            .AddScoped<IOrderRepository, OrderRepository>()
            .AddScoped<IReservationRepository, ReservationRepository>()
            .AddScoped<IRestaurantRepository, RestaurantRepository>()
            .AddScoped<ITableRepository, TableRepository>()
            .BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        ClassModelValidation(scope);

        var customerRepository = scope.ServiceProvider.GetService<ICustomerRepository>();
        var employeeRepository = scope.ServiceProvider.GetService<IEmployeeRepository>();
        var menuItemRepository = scope.ServiceProvider.GetService<IMenuItemRepository>();
        var orderRepository = scope.ServiceProvider.GetService<IOrderRepository>();
        var reservationRepository = scope.ServiceProvider.GetService<IReservationRepository>();
        var tableRepository = scope.ServiceProvider.GetService<ITableRepository>();
        
        var context = scope.ServiceProvider.GetRequiredService<RestaurantReservationDbContext>();
        await context.Database.MigrateAsync();
        await context.SeedDataAsync();

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
        var managers = await employeeRepository.ListManagersAsync();
        foreach (var manager in managers) Console.WriteLine(manager.FirstName + " " + manager.LastName);

        Console.WriteLine();
        var reservationsByCustomer = await reservationRepository.GetReservationsByCustomerAsync(4);
        foreach (var reservation in reservationsByCustomer) Console.WriteLine(reservation.ReservationId);

        Console.WriteLine();
        var listOrdersAndMenuItems = await orderRepository.ListOrdersAndMenuItemsAsync(3);
        foreach (var element in listOrdersAndMenuItems)
        {
            Console.WriteLine("Order Id: " + element.OrderId + " ");
            foreach (var orderItem in element.OrderItems)
                Console.WriteLine("     Menu Items: " + orderItem.MenuItem.Name);
        }

        Console.WriteLine();
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

    private static async Task ClassModelValidation(IServiceScope? scope)
    {
        var customerValidator = scope.ServiceProvider.GetService<IValidator<Customer>>();
        var employeeValidator = scope.ServiceProvider.GetService<IValidator<Employee>>();
        var menuItemValidator = scope.ServiceProvider.GetService<IValidator<MenuItem>>();
        var orderValidator = scope.ServiceProvider.GetService<IValidator<Order>>();
        var reservationValidator = scope.ServiceProvider.GetService<IValidator<Reservation>>();
        var restaurantValidator = scope.ServiceProvider.GetService<IValidator<Restaurant>>();
        var tableValidator = scope.ServiceProvider.GetService<IValidator<Table>>();
        
        var newCustomer = FakeDataGenerator.GenerateFakeCustomer();
        var newEmployee = FakeDataGenerator.GenerateFakeEmployee();
        var newMenuItem = FakeDataGenerator.GenerateFakeMenuItem();
        var newOrder = FakeDataGenerator.GenerateFakeOrder();
        var newReservation = FakeDataGenerator.GenerateFakeReservation();
        var newRestaurant = FakeDataGenerator.GenerateFakeRestaurant();
        var newTable = FakeDataGenerator.GenerateFakeTable();
        
        var customerValidationResult = await customerValidator.ValidateAsync(newCustomer);
        if (!customerValidationResult.IsValid)
        {
            foreach (var error in customerValidationResult.Errors)
                Console.WriteLine($"Validation Error: {error.ErrorMessage}");
            return;
        }

        var employeeValidationResult = await employeeValidator.ValidateAsync(newEmployee);
        if (!employeeValidationResult.IsValid)
        {
            foreach (var error in employeeValidationResult.Errors)
                Console.WriteLine($"Validation Error: {error.ErrorMessage}");
            return;
        }

        var menuItemValidationResult = await menuItemValidator.ValidateAsync(newMenuItem);
        if (!menuItemValidationResult.IsValid)
        {
            foreach (var error in menuItemValidationResult.Errors)
                Console.WriteLine($"Validation Error: {error.ErrorMessage}");
            return;
        }

        var orderValidationResult = await orderValidator.ValidateAsync(newOrder);
        if (!orderValidationResult.IsValid)
        {
            foreach (var error in orderValidationResult.Errors)
                Console.WriteLine($"Validation Error: {error.ErrorMessage}");
            return;
        }
        
        var reservationValidationResult = await reservationValidator.ValidateAsync(newReservation);
        if (!reservationValidationResult.IsValid)
        {
            foreach (var error in reservationValidationResult.Errors)
                Console.WriteLine($"Validation Error: {error.ErrorMessage}");
            return;
        }
        
        var restaurantValidationResult = await restaurantValidator.ValidateAsync(newRestaurant);
        if (!restaurantValidationResult.IsValid)
        {
            foreach (var error in restaurantValidationResult.Errors)
                Console.WriteLine($"Validation Error: {error.ErrorMessage}");
            return;
        }
        
        var tableValidationResult = await tableValidator.ValidateAsync(newTable);
        if (!tableValidationResult.IsValid)
        {
            foreach (var error in tableValidationResult.Errors)
                Console.WriteLine($"Validation Error: {error.ErrorMessage}");
            return;
        }
    }
}