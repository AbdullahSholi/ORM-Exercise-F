using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db;
using RestaurantReservation.Db.models;

namespace RestaurantReservation.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class MenuItemController : ControllerBase
{
    private readonly ILogger<MenuItemController> _logger;
    private readonly RestaurantReservationDbContext _context;

    public MenuItemController(ILogger<MenuItemController> logger, RestaurantReservationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetMenuItems()
    {
        try
        {
            var menuItems = await _context.MenuItems
                .Include(r => r.OrderItems)
                .ToListAsync();
            return Ok(menuItems);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetMenuItem(int id)
    {
        try
        {
            var menuItem = await _context.MenuItems
                .Include(r => r.OrderItems)
                .FirstOrDefaultAsync(r => r.MenuItemId == id);
            if (menuItem == null)
                return NotFound();

            return Ok(menuItem);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateMenuItem([FromBody] MenuItem menuItem)
    {
        try
        {
            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();
            return CreatedAtAction(
                nameof(GetMenuItem),
                new { version = HttpContext.GetRequestedApiVersion()?.ToString(), id = menuItem.MenuItemId },
                menuItem
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateMenuItem(int id, [FromBody] MenuItem menuItem)
    {
        try
        {
            var menuItemToUpdate = await _context.MenuItems.FindAsync(id);
            if (menuItemToUpdate == null)
                return NotFound();
            menuItemToUpdate.RestaurantId = menuItem.RestaurantId;
            menuItemToUpdate.Name = menuItem.Name;
            menuItemToUpdate.Description = menuItem.Description;
            menuItemToUpdate.Price = menuItem.Price;

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
    public async Task<IActionResult> DeleteMenuItem(int id)
    {
        try
        {
            var menuItemToDelete = await _context.MenuItems
                .Include(r => r.OrderItems)
                .FirstOrDefaultAsync(r => r.MenuItemId == id);

            if (menuItemToDelete == null)
                return NotFound();

            _context.OrderItems.RemoveRange(menuItemToDelete.OrderItems);

            _context.MenuItems.Remove(menuItemToDelete);
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