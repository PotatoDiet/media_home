using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CinemaCentral.ClientApp.Services;
using CinemaCentral.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CinemaCentral.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly AppDbContext _appDbContext;
    private readonly IPasswordService _passwordService = new PasswordService();
    
    public UserController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    [AllowAnonymous]
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] UserRequest user)
    {
        if (await _appDbContext.Users.Where(u => u.Name == user.Name).AnyAsync())
        {
            return Conflict();
        }

        var password = Encoding.UTF8.GetBytes(user.Password);
        var salt = _passwordService.CreateSalt();
        var hash = _passwordService.CreateHash(password, salt);

        _appDbContext.Users.Add(new User()
        {
            Name = user.Name,
            PasswordHash = hash,
            PasswordSalt = salt,
            Role = UserRole.Normal
        });
        await _appDbContext.SaveChangesAsync();

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] UserRequest requestedUser)
    {
        var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Name == requestedUser.Name);
        if (user is null)
        {
            return NotFound();
        }
        
        var password = Encoding.UTF8.GetBytes(requestedUser.Password);
        if (!_passwordService.VerifyHash(password, user.PasswordSalt, user.PasswordHash))
        {
            return BadRequest();
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JustATestJustATestJustATest"));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, nameof(user.Role))
        };
        var token = new JwtSecurityToken("Issuer", "Audience", claims, expires: DateTime.Now.AddDays(1), signingCredentials: credentials);
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        HttpContext.Response.Cookies.Append("Token", tokenString, new CookieOptions()
        {
            HttpOnly = true
        });

        return Ok(tokenString);
    }

    [Authorize]
    [HttpPost("Logout")]
    public IActionResult Logout()
    {
        HttpContext.Response.Cookies.Delete("Token");
        return Ok();
    }

    [Authorize]
    [HttpGet("Current")]
    public async Task<IActionResult> Current()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (idClaim is null)
        {
            return NotFound();
        }
        
        var id = Guid.Parse(idClaim);
        return Ok(await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == id));
    }
}

public readonly record struct UserRequest(string Name, string Password);