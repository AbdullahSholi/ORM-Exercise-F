using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db;
using RestaurantReservation.Db.models;

namespace RestaurantReservation.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class RestaurantController : ControllerBase
{
    private readonly ILogger<RestaurantController> _logger;
    private readonly RestaurantReservationDbContext _context;

    public RestaurantController(ILogger<RestaurantController> logger, RestaurantReservationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetRestaurants()
    {
        try
        {
            var restaurants = await _context.Restaurants
                .Include(r => r.Reservations)
                .Include(r => r.Employees)
                .Include(r => r.MenuItems)
                .Include(r => r.Tables)
                .ToListAsync();
            return Ok(restaurants);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetRestaurant(int id)
    {
        try
        {
            var restaurant = await _context.Restaurants
                .Include(r => r.Reservations)
                .Include(r => r.Employees)
                .Include(r => r.MenuItems)
                .Include(r => r.Tables)
                .FirstOrDefaultAsync(r => r.RestaurantId == id);
            if (restaurant == null)
                return NotFound();

            return Ok(restaurant);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateRestaurant([FromBody] Restaurant restaurant)
    {
        try
        {
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();
            return CreatedAtAction(
                nameof(GetRestaurant),
                new { version = HttpContext.GetRequestedApiVersion()?.ToString(), id = restaurant.RestaurantId },
                restaurant
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateRestaurant(int id, [FromBody] Restaurant restaurant)
    {
        try
        {
            var restaurantToUpdate = await _context.Restaurants.FindAsync(id);
            if (restaurantToUpdate == null)
                return NotFound();

            restaurantToUpdate.Name = restaurant.Name;
            restaurantToUpdate.Address = restaurant.Address;
            restaurantToUpdate.PhoneNumber = restaurant.PhoneNumber;
            restaurantToUpdate.OpeningHours = restaurant.OpeningHours;

            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRestaurant(int id)
    {
        try
        {
            var restaurantToDelete = await _context.Restaurants
                .Include(r => r.Reservations)
                .Include(r => r.Employees)
                .Include(r => r.MenuItems)
                .Include(r => r.Tables)
                .FirstOrDefaultAsync(r => r.RestaurantId == id);

            if (restaurantToDelete == null)
                return NotFound();

            _context.Tables.RemoveRange(restaurantToDelete.Tables);
            _context.Reservations.RemoveRange(restaurantToDelete.Reservations);
            _context.Employees.RemoveRange(restaurantToDelete.Employees);
            _context.MenuItems.RemoveRange(restaurantToDelete.MenuItems);

            _context.Restaurants.Remove(restaurantToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }
}