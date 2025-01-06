using Microsoft.AspNetCore.Mvc;
using Productify.Application.DTOs;
using Productify.Application.Services;

namespace Productify.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(TokenService tokenService) : ControllerBase
{
    private readonly TokenService _tokenService = tokenService;
    
    [HttpPost("Login")]
    public IActionResult Login([FromBody] LoginRequestDto loginDto)
    {
        // Admin user for tests
        if (loginDto.Username != "admin" || loginDto.Password != "123456") 
            return Unauthorized("Invalid credentials");
        
        var token = _tokenService.GenerateToken("admin");
        return Ok(new { token });
    }
    
    [HttpGet("force-error")]
    public IActionResult ForceError()
    {
        throw new Exception("This is a forced error for testing the middleware.");
    }
}