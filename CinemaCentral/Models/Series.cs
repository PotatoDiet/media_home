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
    
    public string Title { get; set; }
    public string Overview { get; set; }
    public List<Genre> Genres { get; set; }
    public string? PosterPath { get; set; }
    public float CommunityRating { get; set; }
    public List<Episode> Episodes { get; set; }
}