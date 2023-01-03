using CinemaCentral.Controllers;
using CinemaCentral.Models;
using CinemaCentral.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing;
using TorrentTitleParser;

namespace CinemaCentral.Services;

public class LibraryService
{
    private readonly AppDbContext _appDbContext;
    private readonly TmdbProvider _tmdbProvider;

    public LibraryService(AppDbContext appDbContext, TmdbProvider tmdbProvider)
    {
        _appDbContext = appDbContext;
        _tmdbProvider = tmdbProvider;
    }
    
    public async Task DeleteMedia()
    {
        // Very inefficient since EF loads all rows into memory, but it's good enough for now.

        var movies = await _appDbContext
            .Movies
            .Include(m => m.Genres)
            .Include(m => m.WatchtimeStamps)
            .ToListAsync();
        var series = await _appDbContext
            .Series
            .Include(s => s.Genres)
            .Include(s => s.Seasons)
            .ToListAsync();
        var seasons = await _appDbContext
            .Seasons
            .Include(s => s.Episodes)
            .ToListAsync();
        var episodes = await _appDbContext
            .Episodes
            .Include(e => e.WatchtimeStamps)
            .ToListAsync();
        
        _appDbContext.RemoveRange(movies);
        _appDbContext.RemoveRange(series);
        _appDbContext.RemoveRange(seasons);
        _appDbContext.RemoveRange(episodes);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task ScanLibrary(Library library)
    {
        var matcher = new Matcher();
        matcher.AddInclude("**/*.mp4");
        matcher.AddInclude("**/*.mkv");
        matcher.AddInclude("**/*.mov");
        matcher.AddInclude("**/*.avi");

        foreach (var file in matcher.GetResultsInFullPath(library.Root))
        {
            var filename = Path.GetFileName(file);
            var parsedFilename = new Torrent(filename);
            var path = RemoveRoot(file, library.Root);
            switch (DetermineMediaType(parsedFilename))
            {
                case MediaType.Movie:
                    await CreateMovie(parsedFilename, library, path);
                    break;
                case MediaType.Series:
                    await CreateEpisode(parsedFilename, library, path);
                    break;
                default:
                    throw new Exception();
            }
        }
    }

    private static MediaType DetermineMediaType(Torrent torrent)
    {
        return torrent.Season switch
        {
            0 => MediaType.Movie,
            _ => MediaType.Series
        };
    }

    private async Task CreateMovie(Torrent torrent, Library library, string path)
    {
        var providerResult = await _tmdbProvider.FindMovie(torrent.Title, (uint)torrent.Year);
        if (providerResult is null) return;
        
        var genres = new List<Genre>();
        foreach (var name in providerResult.Genres)
        {
            genres.Add(await FindOrCreateGenre(name));
        }

        _appDbContext.Movies.Add(new Movie()
        {
            Title = providerResult.Title,
            Year = (uint)torrent.Year,
            Genres = genres,
            CommunityRating = (float)providerResult.Rating,
            PosterPath = providerResult.PosterPath,
            Location = torrent.Name,
            Path = path,
            Library = library
        });
        await _appDbContext.SaveChangesAsync();
    }

    private async Task CreateEpisode(Torrent torrent, Library library, string path)
    {
        var title = torrent.Title;
        var season = torrent.Season;
        var episode = torrent.Episode;
        if (title is null) return;

        var series = await FindOrCreateSeries(title, library);
        if (series is null) return;

        var seasonModel = await FindOrCreateSeason(series, season);
        if (seasonModel is null) return;

        var episodeModel = await _tmdbProvider.FindEpisode(series.TmdbId, season, episode);
        if (episodeModel is null) return;
            
        seasonModel.Episodes.Add(new Episode()
        {
            Title = episodeModel.Title,
            PosterPath = episodeModel.PosterPath,
            Series = series,
            Season = seasonModel,
            EpisodeNumber = episode,
            Location = torrent.Name,
            Path = path
        });
        await _appDbContext.SaveChangesAsync();
    }
    
    private async Task<Series?> FindOrCreateSeries(string title, Library library)
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
            TmdbId = result.ProviderId,
            Library = library
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

    private static string RemoveRoot(string path, string root)
    {
        return path.Replace(root, "").TrimStart('/');
    }
}