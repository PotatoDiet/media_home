namespace CinemaCentral.Providers;

public interface ITmdbProvider
{
    public Task<ProviderResult?> FindMovie(string title, uint year);
    public Task<ProviderResult?> FindSeries(string title);
    public Task<ProviderResult?> FindEpisode(int seriesId, int season, int episode);
}