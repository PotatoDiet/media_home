using CinemaCentral.Models;
using TMDbLib.Client;

namespace CinemaCentral.Providers;

public class TmdbProvider
{
    private readonly TMDbClient _client = new("5ef572428a688eddbb5e68049f7fedd8");
    private readonly HttpClient _httpClient = new();

    public async Task<Movie?> Find(string title, uint year)
    {
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
            PosterPath = await DownloadPoster(result.PosterPath),
            Genres = (from genre in movie.Genres
                select new Genre { Name = genre.Name }).ToList()
        };
    }

    private async Task<string> DownloadPoster(string remoteRelativePath)
    {
        var path = Path.Join("assets", "posters", remoteRelativePath);
        var remotePath = Path.Join("https://image.tmdb.org/t/p/original", remoteRelativePath);

        var response = await _httpClient.GetStreamAsync(remotePath);
        await using var fs = new FileStream(Path.Join("./wwwroot", path), FileMode.Create);
        await response.CopyToAsync(fs);

        return Path.Join("/", path);
    }
}