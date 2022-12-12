using System.ComponentModel.DataAnnotations;

namespace CinemaCentral.Models;

public class Genre
{
    [Key]
    public string Name { get; set; }
    
    public List<Movie> Movies { get; set; }
}