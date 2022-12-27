using CinemaCentral.Models;
using CinemaCentral.Providers;
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
    private readonly TmdbProvider _tmdbProvider;

    public MoviesController(AppDbContext appDbContext, TmdbProvider tmdbProvider)
    {
        _appDbContext = appDbContext;
        _tmdbProvider = tmdbProvider;
    }

    [HttpGet]
    [Authorize]
    public async Task<IEnumerable<Movie>> Movies([FromQuery] string? search)
    {
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
        if (movie?.Location is null) return null;

        new FileExtensionContentTypeProvider()
            .TryGetContentType(movie.Location, out var contentType);
        contentType ??= "application/octet-stream";

        var fs = System.IO.File.OpenRead(movie.Location);
        return new FileStreamResult(fs, contentType)
        {
            EnableRangeProcessing = true
        };
    }

    [HttpGet("{id:Guid}/GetWatchtimeStamp")]
    [Authorize]
    public async Task<IActionResult> GetWatchtimeStamp([FromRoute] Guid id)
    {
        var user = await Models.User.GetCurrent(_appDbContext, User);
        var watchtime = await _appDbContext
            .WatchtimeStamps
            .Include(w => w.Movie)
            .Include(w => w.User)
            .FirstOrDefaultAsync(w => w.Movie!.Id == id && w.User!.Id == user!.Id);
        return Ok(watchtime?.Time ?? 0);
    }

    [HttpPost("{id:Guid}/UpdateWatchTimestamp")]
    [Authorize]
    public async Task UpdateWatchTimestamp([FromRoute] Guid id, [FromBody] uint timestamp)
    {
        var user = await Models.User.GetCurrent(_appDbContext, User);
        var watchtime = await _appDbContext
            .WatchtimeStamps
            .Include(w => w.Movie)
            .FirstOrDefaultAsync(w => w.Movie!.Id == id && w.User == user);
        watchtime ??= new WatchtimeStamp()
        {
            User = user!,
            Movie = await _appDbContext.Movies.FindAsync(id)
        };
        _appDbContext.Update(watchtime);

        watchtime.Time = timestamp;
        await _appDbContext.SaveChangesAsync();
    }

    [HttpPost("Update")]
    [Authorize]
    public async Task Update()
    {
        _appDbContext.Movies.RemoveRange(_appDbContext.Movies);
        await _appDbContext.SaveChangesAsync();

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
            if (title is null) continue;
            
            var movie = await _tmdbProvider.FindMovie(title, year);
            if (movie is null) continue;

            var genres = new List<Genre>();
            foreach (var name in movie.Genres)
            {
                genres.Add(await FindOrCreateGenre(name));
            }

            _appDbContext.Add(new Movie()
            {
                Title = movie.Title,
                Year = year,
                Genres = genres,
                CommunityRating = (float)movie.Rating,
                PosterPath = movie.PosterPath,
                Location = file,
                Path = file
            });
            await _appDbContext.SaveChangesAsync();
        }
    }
    
    private async Task<Genre> FindOrCreateGenre(string name)
    {
        return await _appDbContext.Genres.FindAsync(name) ?? new Genre() { Name = name };
    }
}