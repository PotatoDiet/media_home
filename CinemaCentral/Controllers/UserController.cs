using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CinemaCentral.Models;
using CinemaCentral.Services;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> All()
    {
        return Ok(await _appDbContext.Users.ToListAsync());
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
            Role = user.Role
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
            new Claim(ClaimTypes.Role, user.Role.ToString())
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

    [Authorize(Roles = "Admin")]
    [HttpPost("{id:Guid}")]
    public async Task<IActionResult> Edit([FromRoute] Guid id, [FromBody] UserEdit userChanges)
    {
        var user = await _appDbContext.Users.FindAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var currentUser);
        if (currentUser != id)
        {
            user.Role = userChanges.Role;
        }
        user.Name = userChanges.Name;
        await _appDbContext.SaveChangesAsync();
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> Edit([FromRoute] Guid id)
    {
        var user = await _appDbContext.Users.FindAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        var firstAdmin = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Role == UserRole.Admin);
        if (firstAdmin?.Id == user.Id)
        {
            return BadRequest();
        }

        _appDbContext.Users.Remove(user);
        await _appDbContext.SaveChangesAsync();
        return Ok();
    }
}

public readonly record struct UserRequest(string Name, string Password, UserRole Role);
public readonly record struct UserEdit(string Name, UserRole Role);