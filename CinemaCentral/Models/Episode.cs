using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaCentral.Models;

public class Episode
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public required string Location { get; set; }
    public required string Title { get; set; }
    public required string PosterPath { get; set; }
    public required Series Series { get; set; }
    public required Season Season { get; set; }
    public required int EpisodeNumber { get; set; }
    public required string Path { get; set; }
    public List<WatchtimeStamp> WatchtimeStamps { get; set; } = new();
}