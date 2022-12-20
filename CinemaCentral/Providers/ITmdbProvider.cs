using CinemaCentral.Models;

namespace CinemaCentral.Providers;

public interface ITmdbProvider
{
    public Task<Movie?> FindMovie(string title, uint year);
    public Task<Series?> FindSeries(string title);
    public Task<Episode?> FindEpisode(int seriesId, int season, int episode);
}