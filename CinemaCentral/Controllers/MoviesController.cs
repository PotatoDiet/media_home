using System.Text.RegularExpressions;
using CinemaCentral.Models;
using CinemaCentral.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace CinemaCentral.Controllers;

[ApiController]
[Route("api/[controller]")]
public partial class MoviesController
{
    private readonly AppDbContext _appDbContext;
    private readonly TmdbProvider _tmdbProvider = new();

    public MoviesController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }


    [GeneratedRegex(@"([\w,': ]+) \((\d{4})\)")]
    private static partial Regex DecodeMoviePathRegex();

    [HttpGet]
    public async Task<IEnumerable<Movie>> Movies([FromQuery] string? search)
    {
        return await _appDbContext.Movies.Where(m => EF.Functions.Like(m.Title, $"%{search}%")).ToListAsync();
    }

    [HttpGet("{id:Guid}")]
    public async Task<Movie?> Movie([FromRoute] Guid id)
    {
        var movie = await _appDbContext.Movies.Include(m => m.Genres).FirstOrDefaultAsync(m => m.Id == id);
        movie?.Genres?.ForEach(g => g.Movies.Clear());
        return movie;
    }

    [HttpGet("{id:Guid}/Stream")]
    public async Task<FileStreamResult?> Stream([FromRoute] Guid id)
    {
        var movie = await _appDbContext.Movies.FindAsync(id);
        if (movie?.Location is null)
            return null;

        new FileExtensionContentTypeProvider()
            .TryGetContentType(movie.Location, out var contentType);
        contentType ??= "application/octet-stream";

        var fs = File.OpenRead(movie.Location);
        return new FileStreamResult(fs, contentType)
        {
            EnableRangeProcessing = true
        };
    }

    [HttpPost("{id:Guid}/UpdateWatchTimestamp")]
    public async Task UpdateWatchTimestamp([FromRoute] Guid id, [FromBody] uint timestamp)
    {
        var movie = await _appDbContext.Movies.FindAsync(id);
        if (movie?.Location is null)
            return;

        movie.CurrentWatchTimestamp = timestamp;
        await _appDbContext.SaveChangesAsync();
    }

    [HttpPost("Update")]
    public async Task Update()
    {
        await _appDbContext.Database.ExecuteSqlRawAsync("DELETE FROM Movies");
        await _appDbContext.Database.ExecuteSqlRawAsync("DELETE FROM Genres");

        foreach (var path in Directory.GetFiles("Libraries/Movies"))
        {
            var (title, year) = DecodeMoviePath(path);
            if (title is null)
                continue;
            var movie = await _tmdbProvider.FindMovie(title, year);
            if (movie is null)
                continue;

            var genres = new List<Genre>();
            foreach (var genre in movie.Genres)
            {
                var savedGenre = await _appDbContext.Genres.FindAsync(genre.Name) ?? genre;
                genres.Add(savedGenre);
            }

            movie.Genres = genres;

            movie.Location = path;

            _appDbContext.Add(movie);
        }

        await _appDbContext.SaveChangesAsync();
    }

    private static (string? title, uint year) DecodeMoviePath(string path)
    {
        var match = DecodeMoviePathRegex().Match(path);
        if (!match.Success)
            return (null, 0);

        var title = match.Groups[1].Value;
        var year = uint.Parse(match.Groups[2].Value);
        return (title, year);
    }
}