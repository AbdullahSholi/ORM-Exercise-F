using Microsoft.AspNetCore.Mvc;

namespace RestaurantReservation.API.Auth;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenGenerator _jwtGenerator;

    public AuthController(JwtTokenGenerator jwtGenerator)
    {
        _jwtGenerator = jwtGenerator;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request.Username == "admin" && request.Password == "password")
        {
            var token = _jwtGenerator.GenerateToken(request.Username);
            return Ok(new { token });
        }

        return Unauthorized();
    }
}

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}