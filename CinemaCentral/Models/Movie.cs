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

    [Required] public string Title { get; set; }

    public uint? Year { get; set; }
    public string? Location { get; set; }
    public List<Genre> Genres { get; set; }
    public float CommunityRating { get; set; }
    public string? PosterPath { get; set; }
    public uint CurrentWatchTimestamp { get; set; }
}