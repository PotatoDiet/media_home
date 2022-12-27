using CinemaCentral.Services;
using TMDbLib.Client;

namespace CinemaCentral.Providers;

public class TmdbProvider : ITmdbProvider
{
    private readonly TMDbClient _client = new("5ef572428a688eddbb5e68049f7fedd8");
    private readonly ImageService _imageService;
    private readonly IStorageService _storageService;

    public TmdbProvider(ImageService imageService, IStorageService storageService)
    {
        _imageService = imageService;
        _storageService = storageService;
    }

    public async Task<ProviderResult?> FindMovie(string title, uint year)
    {
        Thread.Sleep(1000);
        
        var results = await _client.SearchMovieAsync(title, year: Convert.ToInt32(year));
        var result = results?.Results.FirstOrDefault();
        if (result is null)
            return null;

        var movie = await _client.GetMovieAsync(result.Id);

        return new ProviderResult()
        {
            Title = result.Title,
            Overview = result.Overview,
            Rating = Convert.ToSingle(result.VoteAverage),
            PosterPath = await DownloadPoster(result.PosterPath, 200, 300),
            ProviderId = result.Id,
            Genres = movie.Genres.Select(g => g.Name).ToHashSet()
        };
    }

    public async Task<ProviderResult?> FindSeries(string title)
    {
        Thread.Sleep(1000);
        
        var results = await _client.SearchTvShowAsync(title);
        var result = results?.Results.FirstOrDefault();
        if (result is null)
            return null;

        var series = await _client.GetTvShowAsync(result.Id);

        return new ProviderResult()
        {
            Title = series.Name,
            Overview = series.Overview,
            Rating = Convert.ToSingle(series.VoteAverage),
            Genres = series.Genres.Select(g => g.Name).ToHashSet(),
            PosterPath = await DownloadPoster(series.PosterPath, 200, 300),
            ProviderId = series.Id
        };
    }

    public async Task<ProviderResult?> FindSeason(int seriesId, int season)
    {
        var result = await _client.GetTvSeasonAsync(seriesId, season);
        if (result is null) return null;

        return new ProviderResult()
        {
            Title = result.Name,
            Overview = result.Overview,
            PosterPath = await DownloadPoster(result.PosterPath, 200, 300)
        };
    }
    
    public async Task<ProviderResult?> FindEpisode(int seriesId, int season, int episode)
    {
        Thread.Sleep(1000);
        
        var result = await _client.GetTvEpisodeAsync(seriesId, season, episode);
        if (result is null)
            return null;
        
        return new ProviderResult()
        {
            Title = result.Name,
            Overview = result.Overview,
            Rating = result.VoteAverage,
            Genres = new HashSet<string>(),
            ProviderId = result.Id ?? -1,
            PosterPath = await DownloadPoster(result.StillPath, 320, 180),
        };
    }

    private async Task<string> DownloadPoster(string remoteRelativePath, int width, int height)
    {
        var remotePath = Path.Join("https://image.tmdb.org/t/p/original", remoteRelativePath);
        var extension = Path.GetExtension(remotePath);
        using var destination = new MemoryStream();
        
        await _imageService.Resize(remotePath, destination, width, height);
        destination.Seek(0, SeekOrigin.Begin);
        return await _storageService.StoreObject(destination, extension);
    }
}

public record ProviderResult
{
    public required string Title { get; init; }
    public required string Overview { get; init; }
    public double Rating { get; init; }
    public HashSet<string> Genres { get; init; } = new();
    public int ProviderId { get; init; }
    public required string PosterPath { get; init; }
}