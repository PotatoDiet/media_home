using CinemaCentral.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace CinemaCentral.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeriesController : Controller
{
    private readonly AppDbContext _appDbContext;

    public SeriesController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    [HttpGet("GetSeries/{id:Guid}")]
    [Authorize]
    public async Task<Series?> GetSeries([FromRoute] Guid id)
    {
        var series = await _appDbContext.Series.Include(s => s.Seasons).FirstOrDefaultAsync(s => s.Id == id);
        series?.Seasons?.ForEach(s => s.Series = null);
        return series;
    }

    [HttpGet("GetSeason/{id:Guid}")]
    [Authorize]
    public async Task<IActionResult> GetSeason([FromRoute] Guid id)
    {
        var season = await _appDbContext
            .Seasons
            .Include(s => s.Episodes)
            .FirstOrDefaultAsync(s => s.Id == id);
        if (season is null) return NotFound();
        season.Episodes.ForEach(e => e.Season = null);
        
        return Ok(season);
    }

    [HttpGet("GetEpisode/{id:Guid}")]
    [Authorize]
    public async Task<Episode?> GetEpisode([FromRoute] Guid id)
    {
        return await _appDbContext.Episodes.FindAsync(id);
    }
    
    [HttpGet("GetEpisodeStream/{id:Guid}")]
    [Authorize]
    public async Task<FileStreamResult?> GetEpisodeStream([FromRoute] Guid id)
    {
        var episode = await _appDbContext.Episodes.FindAsync(id);
        if (episode?.Location is null) return null;

        new FileExtensionContentTypeProvider()
            .TryGetContentType(episode.Location, out var contentType);
        contentType ??= "application/octet-stream";

        var fs = System.IO.File.OpenRead(episode.Location);
        return new FileStreamResult(fs, contentType) { EnableRangeProcessing = true };
    }
    
    [HttpGet("GetWatchtimeStamp/{id:Guid}")]
    [Authorize]
    public async Task<IActionResult> GetWatchtimeStamp([FromRoute] Guid id)
    {
        var user = await Models.User.GetCurrent(_appDbContext, User);
        var watchtime = await _appDbContext
            .WatchtimeStamps
            .Include(w => w.Episode)
            .Include(w => w.User)
            .FirstOrDefaultAsync(w => w.Episode!.Id == id && w.User!.Id == user!.Id);
        return Ok(watchtime?.Time ?? 0);
    }

    [HttpPost("UpdateWatchTimestamp/{id:Guid}")]
    [Authorize]
    public async Task UpdateWatchTimestamp([FromRoute] Guid id, [FromBody] uint timestamp)
    {
        var user = await Models.User.GetCurrent(_appDbContext, User);
        var watchtime = await _appDbContext
            .WatchtimeStamps
            .Include(w => w.Episode)
            .FirstOrDefaultAsync(w => w.Episode!.Id == id && w.User == user);
        watchtime ??= new WatchtimeStamp()
        {
            User = user!,
            Episode = await _appDbContext.Episodes.FindAsync(id)
        };
        _appDbContext.Update(watchtime);

        watchtime.Time = timestamp;
        await _appDbContext.SaveChangesAsync();
    }
}