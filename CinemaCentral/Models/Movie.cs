using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CinemaCentral.Models;

[Index(nameof(Title))]
public class Movie
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public required string Title { get; set; }

    public uint? Year { get; set; }
    public required string Location { get; set; }
    public required List<Genre> Genres { get; set; }
    public required float CommunityRating { get; set; }
    public required string PosterPath { get; set; }
    public required string Path { get; set; }
    public List<WatchtimeStamp> WatchtimeStamps { get; set; } = new();
    public required Library Library { get; set; }
}