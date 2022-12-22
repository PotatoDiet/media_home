using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaCentral.Models;

public class WatchtimeStamp
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    public required User User { get; set; }
    public uint Time { get; set; }
    public Movie? Movie { get; set; }
    public Episode? Episode { get; set; }
}