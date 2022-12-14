using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaCentral.Models;

public class Episode
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    public string Location { get; set; }
    public string Title { get; set; }
    public string PosterPath { get; set; }
    public uint CurrentWatchTimestamp { get; set; }
    public Series Series { get; set; }
    public int SeasonNumber { get; set; }
    public int EpisodeNumber { get; set; }
}