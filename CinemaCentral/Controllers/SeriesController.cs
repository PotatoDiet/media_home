using System.Text.RegularExpressions;
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
public partial class SeriesController
{
    private readonly AppDbContext _appDbContext;
    private readonly TmdbProvider _tmdbProvider = new();

    public SeriesController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
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
                series = await _tmdbProvider.FindSeries(title);
                if (series is null)
                    continue;
                series.Episodes = new List<Episode>();
                
                _appDbContext.Series.Add(series);
            }

            var episodeModel =
                await _tmdbProvider.FindEpisode(series.TmdbId, season, episode);
            if (episodeModel is null)
                continue;
            episodeModel.Location = file;
            episodeModel.SeasonNumber = season;
            
            series.Episodes.Add(episodeModel);
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

        var fs = File.OpenRead(episode.Location);
        return new FileStreamResult(fs, contentType)
        {
            EnableRangeProcessing = true
        };
    }
}