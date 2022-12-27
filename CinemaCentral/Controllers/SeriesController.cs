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
        _appDbContext.Series.RemoveRange(
            _appDbContext
                .Series
                .Include(s => s.Genres)
                .Include(s => s.Seasons));
        _appDbContext.Seasons.RemoveRange(
            _appDbContext
                .Seasons
                .Include(s => s.Episodes));
        _appDbContext.Episodes.RemoveRange(
            _appDbContext
                .Episodes
                .Include(e => e.WatchtimeStamps));
        await _appDbContext.SaveChangesAsync();

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
            if (title is null) continue;

            var series = await FindOrCreateSeries(title);
            if (series is null) continue;

            var seasonModel = await FindOrCreateSeason(series, season);
            if (seasonModel is null) continue;

            var episodeModel = await _tmdbProvider.FindEpisode(series.TmdbId, season, episode);
            if (episodeModel is null)
                continue;
            
            seasonModel.Episodes.Add(new Episode()
            {
                Title = episodeModel.Title,
                PosterPath = episodeModel.PosterPath,
                Series = series,
                Season = seasonModel,
                EpisodeNumber = episode,
                Location = file,
                Path = file
            });
            await _appDbContext.SaveChangesAsync();
        }
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

    private async Task<Series?> FindOrCreateSeries(string title)
    {
        var series = await _appDbContext.Series.FirstOrDefaultAsync(s => s.Title == title);
        if (series is not null) return series;
        
        var result = await _tmdbProvider.FindSeries(title);
        if (result is null) return null;

        var genres = new List<Genre>();
        foreach (var name in result.Genres)
        {
            genres.Add(await FindOrCreateGenre(name));
        }

        series = new Series()
        {
            Title = result.Title,
            CommunityRating = (float)result.Rating,
            Genres = genres,
            Overview = result.Overview,
            PosterPath = result.PosterPath,
            TmdbId = result.ProviderId
        };
        _appDbContext.Add(series);
        await _appDbContext.SaveChangesAsync();

        return series;
    }

    private async Task<Season?> FindOrCreateSeason(Series series, int seasonNumber)
    {
        var season = await _appDbContext
                   .Seasons
                   .FirstOrDefaultAsync(s => s.Series == series && s.Number == seasonNumber);
        if (season is not null) return season;
        
        var result = await _tmdbProvider.FindSeason(series.TmdbId, seasonNumber);
        if (result is null) return null;

        season = new Season()
        {
            Number = seasonNumber,
            PosterPath = result.PosterPath,
            Series = series
        };
        _appDbContext.Seasons.Add(season);
        await _appDbContext.SaveChangesAsync();

        return season;
    }

    private async Task<Genre> FindOrCreateGenre(string name)
    {
        return await _appDbContext.Genres.FindAsync(name) ?? new Genre() { Name = name };
    }
}