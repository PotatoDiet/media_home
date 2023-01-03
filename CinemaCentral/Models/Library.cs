using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaCentral.Models;

public class Library
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Root { get; set; }
    public List<Movie> Movies { get; set; } = new();
    public List<Series> Series { get; set; } = new();
}