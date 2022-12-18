using CinemaCentral.Models;
using CinemaCentral.Providers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing;
using TorrentTitleParser;

namespace CinemaCentral.Controllers;

[ApiController]
[Route("api/[controller]")]
public partial class MoviesController : Controller
{
    private readonly AppDbContext _appDbContext;
    private readonly TmdbProvider _tmdbProvider = new();

    public MoviesController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    [HttpGet]
    [Authorize]
    public async Task<IEnumerable<Movie>> Movies([FromQuery] string? search)
    {
        var headers = HttpContext.Request.Headers;
        var token = await HttpContext.AuthenticateAsync();
        return await _appDbContext.Movies.Where(m => EF.Functions.Like(m.Title, $"%{search}%")).ToListAsync();
    }

    [HttpGet("{id:Guid}")]
    [Authorize]
    public async Task<Movie?> Movie([FromRoute] Guid id)
    {
        var movie = await _appDbContext.Movies.Include(m => m.Genres).FirstOrDefaultAsync(m => m.Id == id);
        movie?.Genres?.ForEach(g => g.Movies.Clear());
        return movie;
    }

    [HttpGet("{id:Guid}/Stream")]
    [Authorize]
    public async Task<FileStreamResult?> Stream([FromRoute] Guid id)
    {
        var movie = await _appDbContext.Movies.FindAsync(id);
        if (movie?.Location is null)
            return null;

        new FileExtensionContentTypeProvider()
            .TryGetContentType(movie.Location, out var contentType);
        contentType ??= "application/octet-stream";

        var fs = System.IO.File.OpenRead(movie.Location);
        return new FileStreamResult(fs, contentType)
        {
            EnableRangeProcessing = true
        };
    }

    [HttpPost("{id:Guid}/UpdateWatchTimestamp")]
    [Authorize]
    public async Task UpdateWatchTimestamp([FromRoute] Guid id, [FromBody] uint timestamp)
    {
        var movie = await _appDbContext.Movies.FindAsync(id);
        if (movie?.Location is null)
            return;

        movie.CurrentWatchTimestamp = timestamp;
        await _appDbContext.SaveChangesAsync();
    }

    [HttpPost("Update")]
    [Authorize]
    public async Task Update()
    {
        await _appDbContext.Database.ExecuteSqlRawAsync("DELETE FROM Movies");
        await _appDbContext.Database.ExecuteSqlRawAsync("DELETE FROM Genres");

        var matcher = new Matcher();
        matcher.AddInclude("**/*.mp4");
        matcher.AddInclude("**/*.mkv");
        matcher.AddInclude("**/*.mov");
        matcher.AddInclude("**/*.avi");

        foreach (var file in matcher.GetResultsInFullPath("Libraries/Movies"))
        {
            var parsedFilename = new Torrent(Path.GetFileName(file));
            var title = parsedFilename.Title;
            var year = Convert.ToUInt32(parsedFilename.Year);
            if (title is null)
            {
                continue;
            }
            
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

            movie.Location = file;

            _appDbContext.Add(movie);
            await _appDbContext.SaveChangesAsync();
        }
    }
}