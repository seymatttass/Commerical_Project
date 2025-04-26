using Auth.API.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;

    public AuthController(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel login)
    {
        if (login.Username == "admin" && login.Password == "12345")
        {
            var token = _tokenService.GenerateToken(login.Username);
            return Ok(new { token });
        }
        return Unauthorized();
    }
}
