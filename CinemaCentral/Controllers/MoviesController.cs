using CinemaCentral.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace CinemaCentral.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : Controller
{
    private readonly AppDbContext _appDbContext;

    public MoviesController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
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
    public async Task<IActionResult> Stream([FromRoute] Guid id)
    {
        var movie = await _appDbContext
            .Movies
            .Include(x => x.Library)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (movie is null) return new NotFoundResult();

        var path = Path.Combine(movie.Library.Root, movie.Path);
        new FileExtensionContentTypeProvider()
            .TryGetContentType(path, out var contentType);
        contentType ??= "application/octet-stream";

        var fs = System.IO.File.OpenRead(path);
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
            .FirstOrDefaultAsync(w => w.Movie!.Id == id && w.User.Id == user!.Id);
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
}