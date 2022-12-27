using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaCentral.Models;

public class Season
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    public int Number { get; set; }
    public required string PosterPath { get; set; }
    public required Series Series { get; set; }
    public List<Episode> Episodes { get; set; } = new();
}