using CinemaCentral.ClientApp.Services;
using CinemaCentral.Models;
using TMDbLib.Client;

namespace CinemaCentral.Providers;

public class TmdbProvider : ITmdbProvider
{
    private readonly TMDbClient _client = new("5ef572428a688eddbb5e68049f7fedd8");
    private readonly ImageService _imageService;

    public TmdbProvider(ImageService imageService)
    {
        _imageService = imageService;
    }

    public async Task<Movie?> FindMovie(string title, uint year)
    {
        Thread.Sleep(1000);
        
        var results = await _client.SearchMovieAsync(title, year: Convert.ToInt32(year));
        var result = results?.Results.FirstOrDefault();
        if (result is null)
            return null;

        var movie = await _client.GetMovieAsync(result.Id);

        return new Movie
        {
            Title = result.Title,
            Year = year,
            CommunityRating = Convert.ToSingle(result.VoteAverage),
            PosterPath = await DownloadPoster(result.PosterPath, 200, 300),
            Genres = (from genre in movie.Genres
                select new Genre { Name = genre.Name }).ToList()
        };
    }

    public async Task<Series?> FindSeries(string title)
    {
        Thread.Sleep(1000);
        
        var results = await _client.SearchTvShowAsync(title);
        var result = results?.Results.FirstOrDefault();
        if (result is null)
            return null;

        var series = await _client.GetTvShowAsync(result.Id);

        return new Series()
        {
            Title = series.Name,
            Overview = series.Overview,
            CommunityRating = Convert.ToSingle(series.VoteAverage),
            PosterPath = await DownloadPoster(series.PosterPath, 200, 300),
            TmdbId = series.Id
        };
    }
    
    public async Task<Episode?> FindEpisode(int seriesId, int season, int episode)
    {
        Thread.Sleep(1000);
        
        var result = await _client.GetTvEpisodeAsync(seriesId, season, episode);
        if (result is null)
            return null;

        return new Episode()
        {
            Title = result.Name,
            PosterPath = await DownloadPoster(result.StillPath, 320, 180),
            SeasonNumber = result.SeasonNumber,
            EpisodeNumber = result.EpisodeNumber
        };
    }

    private async Task<string> DownloadPoster(string remoteRelativePath, int width, int height)
    {
        var path = Path.Join("assets", "posters", remoteRelativePath);
        var destination = Path.Join("./wwwroot", path);
        var remotePath = Path.Join("https://image.tmdb.org/t/p/original", remoteRelativePath);
        
        await _imageService.Resize(remotePath, destination, width, height);

        return Path.Join("/", path);
    }
}