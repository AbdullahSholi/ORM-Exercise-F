using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db;
using RestaurantReservation.Db.models;

namespace RestaurantReservation.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class EmployeeController : ControllerBase
{
    private readonly ILogger<EmployeeController> _logger;
    private readonly RestaurantReservationDbContext _context;

    public EmployeeController(ILogger<EmployeeController> logger, RestaurantReservationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployees()
    {
        try
        {
            var employees = await _context.Employees
                .Include(r => r.Orders)
                .ToListAsync();
            return Ok(employees);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetEmployee(int id)
    {
        try
        {
            var employee = await _context.Employees
                .Include(r => r.Orders)
                .FirstOrDefaultAsync(r => r.EmployeeId == id);
            if (employee == null)
                return NotFound();

            return Ok(employee);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
    {
        try
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return CreatedAtAction(
                nameof(GetEmployee),
                new { version = HttpContext.GetRequestedApiVersion()?.ToString(), id = employee.EmployeeId },
                employee
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee employee)
    {
        try
        {
            var employeeToUpdate = await _context.Employees.FindAsync(id);
            if (employeeToUpdate == null)
                return NotFound();

            employeeToUpdate.FirstName = employee.FirstName;
            employeeToUpdate.LastName = employee.LastName;
            employeeToUpdate.Position = employee.Position;
            employeeToUpdate.RestaurantId = employee.RestaurantId;

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
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        try
        {
            var employeeToDelete = await _context.Employees
                .Include(r => r.Orders)
                .FirstOrDefaultAsync(r => r.EmployeeId == id);

            if (employeeToDelete == null)
                return NotFound();

            _context.Orders.RemoveRange(employeeToDelete.Orders);

            _context.Employees.Remove(employeeToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("managers")]
    public async Task<IActionResult> GetManagers()
    {
        try
        {
            var managers = await _context.Employees
                .Include(r => r.Orders)
                .Where(r => r.Position == EmployeePosition.Manager)
                .ToListAsync();
            return Ok(managers);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{employeeId:int}/average-order-amount")]
    public async Task<IActionResult> GetAverageOrderAmountForEmployee(int employeeId)
    {
        try
        {
            var averageOrderAmount = await _context.Orders
                .Where(o => o.EmployeeId == employeeId)
                .AverageAsync(o => o.TotalAmount);
            return Ok(averageOrderAmount);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }
}