using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace CinemaCentral.Models;

public enum UserRole
{
    Admin,
    Normal
}

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    public required string Name { get; set; }
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }
    public required UserRole Role { get; set; }

    public static async Task<User?> GetCurrent(AppDbContext appDbContext, ClaimsPrincipal claims)
    {
        var idClaim = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid.TryParse(idClaim, out var id);
        return await appDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
    }
}