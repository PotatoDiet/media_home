using CinemaCentral.Models;
using CinemaCentral.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaCentral.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibraryController
{
    private readonly AppDbContext _appDbContext;
    private readonly LibraryService _libraryService;

    public LibraryController(AppDbContext appDbContext, LibraryService libraryService)
    {
        _appDbContext = appDbContext;
        _libraryService = libraryService;
    }

    [Authorize]
    [HttpGet("ListLibraries")]
    public async Task<IActionResult> ListLibraries()
    {
        var libraries = await _appDbContext.Libraries.ToListAsync();
        return new OkObjectResult(libraries);
    }

    [Authorize]
    [HttpGet("ListMedia")]
    public async Task<IActionResult> ListMedia([FromQuery] string? search)
    {
        var movies = from movie in await _appDbContext
                .Movies
                .Where(m => EF.Functions.Like(m.Title, $"%{search}%"))
                .ToListAsync()
            select new Media()
            {
                Id = movie.Id,
                PosterPath = movie.PosterPath ?? "",
                MediaType = MediaType.Movie
            };
        var series = from tv in await _appDbContext
                .Series
                .Where(s => EF.Functions.Like(s.Title, $"%{search}%"))
                .ToListAsync()
            select new Media()
            {
                Id = tv.Id,
                PosterPath = tv.PosterPath,
                MediaType = MediaType.Series
            };

        return new OkObjectResult(movies.Concat(series).ToList());
    }

    [Authorize]
    [HttpGet("GetLibraries")]
    public async Task<IActionResult> GetLibraries()
    {
        var libraries = await _appDbContext
            .Libraries
            .Include(x => x.Movies)
            .Include(x => x.Series)
            .ToListAsync();
        return new OkObjectResult(libraries);
    }

    [Authorize]
    [HttpGet("GetLibrary/{id:Guid}")]
    public async Task<IActionResult> GetLibrary([FromRoute] Guid id)
    {
        var library = await _appDbContext
            .Libraries
            .Include(x => x.Movies)
            .Include(x => x.Series)
            .FirstOrDefaultAsync(x => x.Id == id);
        
        return library switch
        {
            { }  => new OkObjectResult(library),
            null => new NotFoundResult()
        };
    }
    
    [Authorize]
    [HttpGet("GetLibraryMedia/{id:Guid}")]
    public async Task<IActionResult> GetLibraryMedia([FromRoute] Guid id, [FromQuery] string? search)
    {
        var movies = from movie in await _appDbContext
                .Movies
                .Where(x => x.Library.Id == id)
                .Where(m => EF.Functions.Like(m.Title, $"%{search}%"))
                .ToListAsync()
            select new Media()
            {
                Id = movie.Id,
                PosterPath = movie.PosterPath ?? "",
                MediaType = MediaType.Movie
            };
        var series = from tv in await _appDbContext
                .Series
                .Where(x => x.Library.Id == id)
                .Where(s => EF.Functions.Like(s.Title, $"%{search}%"))
                .ToListAsync()
            select new Media()
            {
                Id = tv.Id,
                PosterPath = tv.PosterPath,
                MediaType = MediaType.Series
            };

        return new OkObjectResult(movies.Concat(series).ToList());
    }

    [Authorize]
    [HttpPost("Update")]
    public async Task<IActionResult> UpdateLibraries()
    {
        await _libraryService.DeleteMedia();

        foreach (var library in await _appDbContext.Libraries.ToListAsync())
        {
            await _libraryService.ScanLibrary(library);
        }
        
        return new OkResult();
    }
}

public readonly record struct Media(Guid Id, string PosterPath, MediaType MediaType);

public enum MediaType
{
    Movie,
    Series
}