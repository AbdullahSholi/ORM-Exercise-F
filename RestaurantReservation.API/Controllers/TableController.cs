using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db;
using RestaurantReservation.Db.models;

namespace RestaurantReservation.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class TableController : ControllerBase
{
    private readonly ILogger<TableController> _logger;
    private readonly RestaurantReservationDbContext _context;

    public TableController(ILogger<TableController> logger, RestaurantReservationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetTables()
    {
        try
        {
            var tables = await _context.Tables
                .ToListAsync();
            return Ok(tables);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTable(int id)
    {
        try
        {
            var table = await _context.Tables
                .FirstOrDefaultAsync(r => r.TableId == id);
            if (table == null)
                return NotFound();

            return Ok(table);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateTable([FromBody] Table table)
    {
        try
        {
            _context.Tables.Add(table);
            await _context.SaveChangesAsync();
            return CreatedAtAction(
                nameof(GetTable),
                new { version = HttpContext.GetRequestedApiVersion()?.ToString(), id = table.TableId },
                table
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTable(int id, [FromBody] Table table)
    {
        try
        {
            var tableToUpdate = await _context.Tables.FindAsync(id);
            if (tableToUpdate == null)
                return NotFound();

            tableToUpdate.RestaurantId = table.RestaurantId;
            tableToUpdate.Capacity = table.Capacity;

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
    public async Task<IActionResult> DeleteTable(int id)
    {
        try
        {
            var tableToDelete = await _context.Tables
                .FirstOrDefaultAsync(r => r.TableId == id);

            if (tableToDelete == null)
                return NotFound();

            _context.Tables.Remove(tableToDelete);
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