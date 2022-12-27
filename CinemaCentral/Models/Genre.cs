using System.ComponentModel.DataAnnotations;

namespace CinemaCentral.Models;

public class Genre
{
    [Key]
    public required string Name { get; set; }

    public List<Movie> Movies { get; set; } = new();
}