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
public partial class SeriesController : Controller
{
    private readonly AppDbContext _appDbContext;
    private readonly TmdbProvider _tmdbProvider;

    public SeriesController(AppDbContext appDbContext, TmdbProvider tmdbProvider)
    {
        _appDbContext = appDbContext;
        _tmdbProvider = tmdbProvider;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<List<Series>> All([FromQuery] string? search)
    {
        return await _appDbContext.Series.Where(s => EF.Functions.Like(s.Title, $"%{search}%")).ToListAsync();
    }

    [HttpPost("Update")]
    [Authorize]
    public async Task Update()
    {
        await _appDbContext.Database.ExecuteSqlRawAsync("DELETE FROM Series");
        await _appDbContext.Database.ExecuteSqlRawAsync("DELETE FROM Episodes");

        var matcher = new Matcher();
        matcher.AddInclude("**/*.mp4");
        matcher.AddInclude("**/*.mkv");
        matcher.AddInclude("**/*.mov");
        matcher.AddInclude("**/*.avi");

        foreach (var file in matcher.GetResultsInFullPath("Libraries/Series"))
        {
            var parsedFilename = new Torrent(Path.GetFileName(file));
            var title = parsedFilename.Title;
            var season = parsedFilename.Season;
            var episode = parsedFilename.Episode;
            if (title is null)
            {
                continue;
            }

            var series = await _appDbContext.Series.Include(s => s.Episodes).Where(e => e.Title == title).FirstOrDefaultAsync();
            if (series is null)
            {
                var seriesResult = await _tmdbProvider.FindSeries(title);
                if (seriesResult is null)
                    continue;
                series = new Series()
                {
                    Title = seriesResult.Title,
                    Overview = seriesResult.Overview,
                    CommunityRating = (float)seriesResult.Rating,
                    Episodes = new List<Episode>(),
                    Genres = new List<Genre>(),
                    PosterPath = seriesResult.PosterPath,
                    TmdbId = seriesResult.ProviderId
                };
                series.Episodes = new List<Episode>();
                
                _appDbContext.Series.Add(series);
            }

            var episodeModel =
                await _tmdbProvider.FindEpisode(series.TmdbId, season, episode);
            if (episodeModel is null)
                continue;
            
            series.Episodes.Add(new Episode()
            {
                Title = episodeModel.Title,
                PosterPath = episodeModel.PosterPath,
                Series = series,
                SeasonNumber = season,
                Path = file
            });
            await _appDbContext.SaveChangesAsync();
        }
    }

    [HttpGet("GetSeries/{id:Guid}")]
    [Authorize]
    public async Task<Series?> GetSeries([FromRoute] Guid id)
    {
        var series = await _appDbContext.Series.Include(s => s.Episodes).FirstOrDefaultAsync(s => s.Id == id);
        series?.Episodes?.ForEach(e => e.Series = null);
        return series;
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
        if (episode?.Location is null)
            return null;

        new FileExtensionContentTypeProvider()
            .TryGetContentType(episode.Location, out var contentType);
        contentType ??= "application/octet-stream";

        var fs = System.IO.File.OpenRead(episode.Location);
        return new FileStreamResult(fs, contentType)
        {
            EnableRangeProcessing = true
        };
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