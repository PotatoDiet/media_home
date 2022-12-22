using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CinemaCentral.Models;

[Index(nameof(Title))]
public class Series
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    public int TmdbId { get; set; }
    
    public required string Title { get; set; }
    public required string Overview { get; set; }
    public required List<Genre> Genres { get; set; }
    public required string PosterPath { get; set; }
    public float CommunityRating { get; set; }
    public required List<Episode> Episodes { get; set; }
}