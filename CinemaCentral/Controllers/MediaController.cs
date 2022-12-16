using CinemaCentral.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaCentral.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaController
{
    private readonly AppDbContext _appDbContext;

    public MediaController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<List<Media>> All()
    {
        var movies = from movie in await _appDbContext.Movies.ToListAsync()
            select new Media()
            {
                Id = movie.Id,
                PosterPath = movie.PosterPath ?? "",
                MediaType = MediaType.Movie
            };
        var series = from tv in await _appDbContext.Series.ToListAsync()
            select new Media()
            {
                Id = tv.Id,
                PosterPath = tv.PosterPath,
                MediaType = MediaType.Series
            };

        return movies.Concat(series).ToList();
    }
}

public readonly record struct Media(Guid Id, string PosterPath, MediaType MediaType);

public enum MediaType
{
    Movie,
    Series
}