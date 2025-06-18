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
public class CustomerController : ControllerBase
{
    private readonly ILogger<CustomerController> _logger;
    private readonly RestaurantReservationDbContext _context;

    public CustomerController(ILogger<CustomerController> logger, RestaurantReservationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        try
        {
            var customers = await _context.Customers
                .Include(r => r.Reservations)
                .ToListAsync();
            return Ok(customers);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCustomer(int id)
    {
        try
        {
            var customer = await _context.Customers
                .Include(r => r.Reservations)
                .FirstOrDefaultAsync(r => r.CustomerId == id);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
    {
        try
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(
                nameof(GetCustomer),
                new { version = HttpContext.GetRequestedApiVersion()?.ToString(), id = customer.CustomerId },
                customer
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCustomer(int id, [FromBody] Customer customer)
    {
        try
        {
            var customerToUpdate = await _context.Customers.FindAsync(id);
            if (customerToUpdate == null)
                return NotFound();

            customerToUpdate.FirstName = customer.FirstName;
            customerToUpdate.LastName = customer.LastName;
            customerToUpdate.Email = customer.Email;
            customerToUpdate.PhoneNumber = customer.PhoneNumber;
            customerToUpdate.Reservations = customer.Reservations;

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
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        try
        {
            var customerToDelete = await _context.Customers
                .Include(r => r.Reservations)
                .FirstOrDefaultAsync(r => r.CustomerId == id);

            if (customerToDelete == null)
                return NotFound();

            _context.Reservations.RemoveRange(customerToDelete.Reservations);

            _context.Customers.Remove(customerToDelete);
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