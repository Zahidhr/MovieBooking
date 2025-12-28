using Microsoft.AspNetCore.Mvc;
using MovieBooking.Api.Contracts.Requests;
using MovieBooking.Api.Contracts.Responses;
using MovieBooking.Application.Services;

namespace MovieBooking.Api.Controllers;

[ApiController]
[Route("api/movies")]
public class MoviesController : ControllerBase
{
    private readonly MovieService _movieService;

    public MoviesController(MovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMovieRequest request, CancellationToken ct)
    {
        var id = await _movieService.CreateAsync(request.Title, request.DurationMinutes, ct);
        return CreatedAtAction(nameof(GetAll), new { id }, null);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MovieResponse>>> GetAll(CancellationToken ct)
    {
        var movies = await _movieService.GetAllAsync(ct);

        var response = movies
            .Select(m => new MovieResponse(m.MovieId, m.Title, m.DurationMinutes))
            .ToList();

        return Ok(response);
    }
}
