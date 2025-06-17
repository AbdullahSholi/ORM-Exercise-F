using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db;
using RestaurantReservation.Db.models;

namespace RestaurantReservation.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ReservationController : ControllerBase
{
    private readonly ILogger<ReservationController> _logger;
    private readonly RestaurantReservationDbContext _context;

    public ReservationController(ILogger<ReservationController> logger, RestaurantReservationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetReservations()
    {
        try
        {
            var reservations = await _context.Reservations
                .Include(r => r.Orders)
                .Include(r => r.Tables)
                .ToListAsync();
            return Ok(reservations);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetReservation(int id)
    {
        try
        {
            var reservation = await _context.Reservations
                .Include(r => r.Orders)
                .Include(r => r.Tables)
                .FirstOrDefaultAsync(r => r.ReservationId == id);
            if (reservation == null)
                return NotFound();

            return Ok(reservation);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateReservation([FromBody] Reservation reservation)
    {
        try
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return CreatedAtAction(
                nameof(GetReservation),
                new { version = HttpContext.GetRequestedApiVersion()?.ToString(), id = reservation.ReservationId },
                reservation
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateReservation(int id, [FromBody] Reservation reservation)
    {
        try
        {
            var reservationToUpdate = await _context.Reservations.FindAsync(id);
            if (reservationToUpdate == null)
                return NotFound();

            reservationToUpdate.CustomerId = reservation.CustomerId;
            reservationToUpdate.ReservationDate = reservation.ReservationDate;
            reservationToUpdate.Restaurant = reservation.Restaurant;
            reservationToUpdate.RestaurantId = reservation.RestaurantId;
            reservationToUpdate.Orders = reservation.Orders;
            reservationToUpdate.Tables = reservation.Tables;
            reservationToUpdate.PartySize = reservation.PartySize;

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
    public async Task<IActionResult> DeleteReservation(int id)
    {
        try
        {
            var reservationToDelete = await _context.Reservations
                .Include(r => r.Tables)
                .Include(r => r.Orders)
                .FirstOrDefaultAsync(r => r.ReservationId == id);

            if (reservationToDelete == null)
                return NotFound();

            _context.Tables.RemoveRange(reservationToDelete.Tables);
            _context.Orders.RemoveRange(reservationToDelete.Orders);

            _context.Reservations.Remove(reservationToDelete);
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