using Bogus;
using RestaurantReservation.Db.models;
using System;
using System.Collections.Generic;

namespace RestaurantReservation.Db.DataGeneration;

public static class FakeDataGenerator
{
    public static Customer GenerateFakeCustomer()
    {
        var faker = new Faker("en");

        var customer = new Customer
        {
            FirstName = faker.Name.FirstName(),
            LastName = faker.Name.LastName(),
            Email = faker.Internet.Email(),
            PhoneNumber = faker.Phone.PhoneNumber("+9705########"),
            Reservations = new List<Reservation>()
            {
                new()
                {
                    RestaurantId = faker.Random.Int(1, 5),
                    ReservationDate = faker.Date.Soon(10),
                    PartySize = faker.Random.Int(1, 8),
                    Tables = new List<Table>
                    {
                        new()
                        {
                            RestaurantId = faker.Random.Int(1, 5),
                            Capacity = faker.Random.Int(2, 6)
                        }
                    },
                    Orders = new List<Order>
                    {
                        new()
                        {
                            OrderDate = faker.Date.Recent(),
                            TotalAmount = faker.Random.Int(50, 300)
                        }
                    }
                }
            }
        };

        return customer;
    }

    public static Employee GenerateFakeEmployee()
    {
        var restaurantFaker = new Faker<Restaurant>()
            .RuleFor(r => r.RestaurantId, f => f.IndexFaker + 1)
            .RuleFor(r => r.Name, f => f.Company.CompanyName())
            .RuleFor(r => r.Address, f => f.Address.FullAddress())
            .RuleFor(r => r.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(r => r.OpeningHours, f => "9 AM - 9 PM");

        var orderFaker = new Faker<Order>()
            .RuleFor(o => o.OrderId, f => f.IndexFaker + 1)
            .RuleFor(o => o.ReservationId, f => f.Random.Int(1, 100))
            .RuleFor(o => o.OrderDate, f => f.Date.Past(1))
            .RuleFor(o => o.TotalAmount, f => f.Random.Int(10, 500))
            .RuleFor(o => o.OrderItems, f => new List<OrderItem>());

        var employeeFaker = new Faker<Employee>()
            .RuleFor(e => e.EmployeeId, f => f.IndexFaker + 1)
            .RuleFor(e => e.FirstName, f => f.Name.FirstName())
            .RuleFor(e => e.LastName, f => f.Name.LastName())
            .RuleFor(e => e.Position, EmployeePosition.Manager)
            .RuleFor(e => e.Restaurant, () => restaurantFaker.Generate())
            .RuleFor(e => e.RestaurantId, (f, e) => e.Restaurant.RestaurantId)
            .RuleFor(e => e.Orders, f => orderFaker.Generate(1));

        return employeeFaker.Generate();
    }

    public static MenuItem GenerateFakeMenuItem(int restaurantId = 1)
    {
        var faker = new Faker();

        var fakeRestaurant = new Restaurant
        {
            RestaurantId = restaurantId,
            Name = faker.Company.CompanyName(),
            Address = faker.Address.FullAddress(),
            PhoneNumber = faker.Phone.PhoneNumber(),
            OpeningHours = "9 AM - 9 PM"
        };

        var fakeMenuItem = new MenuItem
        {
            Name = faker.Commerce.ProductName(),
            Description = faker.Commerce.ProductDescription(),
            Price = Math.Round(faker.Random.Decimal(5, 50), 2),
            RestaurantId = restaurantId,
            Restaurant = fakeRestaurant
        };

        var fakeOrderItem = new OrderItem
        {
            MenuItem = fakeMenuItem,
            MenuItemId = 0,
            Quantity = faker.Random.Int(1, 5)
        };

        fakeMenuItem.OrderItems = new List<OrderItem> { fakeOrderItem };

        return fakeMenuItem;
    }

    public static Order GenerateFakeOrder(int restaurantId = 1)
    {
        var faker = new Faker();

        var employee = new Employee
        {
            EmployeeId = faker.Random.Int(1, 1000),
            FirstName = faker.Name.FirstName(),
            LastName = faker.Name.LastName(),
            Position = EmployeePosition.Manager,
            RestaurantId = restaurantId
        };

        var reservation = new Reservation
        {
            ReservationId = faker.Random.Int(1, 1000),
            CustomerId = faker.Random.Int(1, 1000),
            RestaurantId = restaurantId,
            ReservationDate = faker.Date.Future(),
            PartySize = faker.Random.Int(1, 10)
        };

        var menuItems = new List<MenuItem>();
        for (var i = 0; i < 3; i++)
        {
            var menuItem = new MenuItem
            {
                MenuItemId = faker.Random.Int(1, 10000),
                Name = faker.Commerce.ProductName(),
                Description = faker.Commerce.ProductDescription(),
                Price = Math.Round(faker.Random.Decimal(5, 50), 2),
                RestaurantId = restaurantId
            };
            menuItems.Add(menuItem);
        }

        var orderItems = menuItems.Select(item => new OrderItem
        {
            MenuItem = item,
            MenuItemId = item.MenuItemId,
            Quantity = faker.Random.Int(1, 5)
        }).ToList();

        var totalAmount = orderItems.Sum(oi => (int)(oi.MenuItem.Price * oi.Quantity));

        var order = new Order
        {
            OrderId = faker.Random.Int(1, 10000),
            Reservation = reservation,
            ReservationId = reservation.ReservationId,
            Employee = employee,
            EmployeeId = employee.EmployeeId,
            OrderDate = DateTime.Now,
            TotalAmount = totalAmount,
            OrderItems = orderItems
        };

        foreach (var oi in orderItems)
        {
            oi.Order = order;
            oi.OrderId = order.OrderId;
        }

        return order;
    }
    public static Reservation GenerateFakeReservation(int restaurantId = 1, bool includeOrders = true)
    {
        var faker = new Faker();
        
        var customer = new Customer
        {
            CustomerId = faker.Random.Int(1, 10000),
            FirstName = faker.Name.FirstName(),
            LastName = faker.Name.LastName(),
            Email = faker.Internet.Email(),
            PhoneNumber = faker.Phone.PhoneNumber()
        };
        
        var restaurant = new Restaurant
        {
            RestaurantId = restaurantId,
            Name = faker.Company.CompanyName(),
            Address = faker.Address.FullAddress(),
            PhoneNumber = faker.Phone.PhoneNumber(),
            OpeningHours = "10:00 AM - 10:00 PM"
        };
        
        var tables = new List<Table>();
        int tableCount = faker.Random.Int(1, 3);
        for (int i = 0; i < tableCount; i++)
        {
            var table = new Table
            {
                TableId = faker.Random.Int(1, 10000),
                Capacity = faker.Random.Int(2, 6),
                RestaurantId = restaurantId
            };
            tables.Add(table);
        }
        
        var reservation = new Reservation
        {
            ReservationId = faker.Random.Int(1, 10000),
            CustomerId = customer.CustomerId,
            Customer = customer,
            RestaurantId = restaurantId,
            Restaurant = restaurant,
            ReservationDate = faker.Date.Future(),
            PartySize = tables.Sum(t => t.Capacity),
            Tables = tables
        };
        
        foreach (var table in tables)
        {
            table.ReservationId = reservation.ReservationId;
            table.Reservation = reservation;
            table.Restaurant = restaurant;
        }
        
        if (includeOrders)
        {
            var order = GenerateFakeOrder(reservation.ReservationId);
            reservation.Orders.Add(order);
        }

        return reservation;
    }
    public static Restaurant GenerateFakeRestaurant(int? restaurantId = null)
    {
        var faker = new Faker();
        int restId = restaurantId ?? faker.Random.Int(1000, 9999);

        var restaurant = new Restaurant
        {
            RestaurantId = restId,
            Name = faker.Company.CompanyName() + " Restaurant",
            Address = faker.Address.FullAddress(),
            PhoneNumber = faker.Phone.PhoneNumber(),
            OpeningHours = "10:00 AM - 10:00 PM"
        };

        var tables = Enumerable.Range(1, faker.Random.Int(5, 10)).Select(i =>
            new Table
            {
                TableId = faker.Random.Int(10000, 99999),
                Capacity = faker.Random.Int(2, 6),
                RestaurantId = restId,
                Restaurant = restaurant
            }).ToList();

        restaurant.Tables = tables;

        var employees = new Faker<Employee>()
            .RuleFor(e => e.EmployeeId, f => f.Random.Int(1000, 9999))
            .RuleFor(e => e.FirstName, f => f.Name.FirstName())
            .RuleFor(e => e.LastName, f => f.Name.LastName())
            .RuleFor(e => e.Position, EmployeePosition.Manager)
            .RuleFor(e => e.RestaurantId, f => restId)
            .RuleFor(e => e.Restaurant, f => restaurant)
            .Generate(faker.Random.Int(3, 6));

        restaurant.Employees = employees;

        var menuItems = new Faker<MenuItem>()
            .RuleFor(m => m.MenuItemId, f => f.Random.Int(1000, 9999))
            .RuleFor(m => m.Name, f => f.Commerce.ProductName())
            .RuleFor(m => m.Description, f => f.Lorem.Sentence(5))
            .RuleFor(m => m.Price, f => f.Random.Decimal(5, 50))
            .RuleFor(m => m.RestaurantId, f => restId)
            .RuleFor(m => m.Restaurant, f => restaurant)
            .Generate(faker.Random.Int(5, 15));

        restaurant.MenuItems = menuItems;

        var reservations = new List<Reservation>();
        for (int i = 0; i < faker.Random.Int(2, 5); i++)
        {
            var customer = new Customer
            {
                CustomerId = faker.Random.Int(1000, 9999),
                FirstName = faker.Name.FirstName(),
                LastName = faker.Name.LastName(),
                Email = faker.Internet.Email(),
                PhoneNumber = faker.Phone.PhoneNumber()
            };

            var reservation = new Reservation
            {
                ReservationId = faker.Random.Int(10000, 99999),
                CustomerId = customer.CustomerId,
                RestaurantId = restId,
                ReservationDate = faker.Date.Between(DateTime.Today.AddDays(-30), DateTime.Today.AddDays(30)),
                PartySize = faker.Random.Int(2, 6),
                Customer = customer,
                Restaurant = restaurant,
                Tables = faker.PickRandom(tables, faker.Random.Int(1, 2)).ToList()
            };

            foreach (var table in reservation.Tables)
            {
                table.ReservationId = reservation.ReservationId;
                table.Reservation = reservation;
            }

            var orders = new List<Order>();
            for (int j = 0; j < faker.Random.Int(1, 2); j++)
            {
                var employee = faker.PickRandom(employees);

                var order = new Order
                {
                    OrderId = faker.Random.Int(10000, 99999),
                    ReservationId = reservation.ReservationId,
                    EmployeeId = employee.EmployeeId,
                    OrderDate = reservation.ReservationDate.AddHours(faker.Random.Int(0, 3)),
                    Employee = employee,
                    Reservation = reservation
                };

                var orderItems = faker.PickRandom(menuItems, faker.Random.Int(1, 3)).Select(mi =>
                    new OrderItem
                    {
                        OrderItemId = faker.Random.Int(10000, 99999),
                        OrderId = order.OrderId,
                        MenuItemId = mi.MenuItemId,
                        Quantity = faker.Random.Int(1, 3),
                        MenuItem = mi,
                        Order = order
                    }).ToList();

                order.OrderItems = orderItems;
                order.TotalAmount = (int)orderItems.Sum(oi => oi.MenuItem.Price * oi.Quantity);
                orders.Add(order);
            }

            reservation.Orders = orders;
            reservations.Add(reservation);
        }

        restaurant.Reservations = reservations;

        return restaurant;
    }
    public static Table GenerateFakeTable()
    {
        var faker = new Faker();

        var restaurant = new Restaurant
        {
            RestaurantId = faker.Random.Int(1000, 9999),
            Name = faker.Company.CompanyName() + " Restaurant",
            Address = faker.Address.FullAddress(),
            PhoneNumber = faker.Phone.PhoneNumber(),
            OpeningHours = "10:00 AM - 10:00 PM"
        };

        var customer = new Customer
        {
            CustomerId = faker.Random.Int(1000, 9999),
            FirstName = faker.Name.FirstName(),
            LastName = faker.Name.LastName(),
            Email = faker.Internet.Email(),
            PhoneNumber = faker.Phone.PhoneNumber()
        };

        var reservation = new Reservation
        {
            ReservationId = faker.Random.Int(10000, 99999),
            CustomerId = customer.CustomerId,
            Customer = customer,
            RestaurantId = restaurant.RestaurantId,
            Restaurant = restaurant,
            ReservationDate = faker.Date.Soon(),
            PartySize = faker.Random.Int(2, 6)
        };

        var table = new Table
        {
            TableId = faker.Random.Int(10000, 99999),
            Capacity = faker.Random.Int(2, 8),
            ReservationId = reservation.ReservationId,
            Reservation = reservation,
            RestaurantId = restaurant.RestaurantId,
            Restaurant = restaurant
        };

        reservation.Tables.Add(table);
        restaurant.Tables.Add(table);
        restaurant.Reservations.Add(reservation);
        customer.Reservations.Add(reservation);

        return table;
    }
}