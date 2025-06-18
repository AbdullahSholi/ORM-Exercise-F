using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db;
using RestaurantReservation.Db.models;

namespace RestaurantReservation.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;
    private readonly RestaurantReservationDbContext _context;

    public OrderController(ILogger<OrderController> logger, RestaurantReservationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        try
        {
            var orders = await _context.Orders
                .Include(r => r.OrderItems)
                .ToListAsync();
            return Ok(orders);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        try
        {
            var order = await _context.Orders
                .Include(r => r.OrderItems)
                .FirstOrDefaultAsync(r => r.OrderId == id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        try
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return CreatedAtAction(
                nameof(GetOrder),
                new { version = HttpContext.GetRequestedApiVersion()?.ToString(), id = order.OrderId },
                order
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)
    {
        try
        {
            var orderToUpdate = await _context.Orders.FindAsync(id);
            if (orderToUpdate == null)
                return NotFound();

            orderToUpdate.ReservationId = order.ReservationId;
            orderToUpdate.OrderDate = order.OrderDate;
            orderToUpdate.ReservationId = order.ReservationId;
            orderToUpdate.TotalAmount = order.TotalAmount;

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
    public async Task<IActionResult> DeleteOrder(int id)
    {
        try
        {
            var orderToDelete = await _context.Orders
                .Include(r => r.OrderItems)
                .FirstOrDefaultAsync(r => r.OrderId == id);

            if (orderToDelete == null)
                return NotFound();

            _context.OrderItems.RemoveRange(orderToDelete.OrderItems);

            _context.Orders.Remove(orderToDelete);
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