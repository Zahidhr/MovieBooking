using Microsoft.AspNetCore.Mvc;
using MovieBooking.Api.Contracts.Requests;
using MovieBooking.Api.Contracts.Responses;
using MovieBooking.Application.Services;

namespace MovieBooking.Api.Controllers;

[ApiController]
[Route("api/cinemas")]
public class CinemasController : ControllerBase
{
    private readonly CinemaService _cinemaService;

    public CinemasController(CinemaService cinemaService)
    {
        _cinemaService = cinemaService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateCinemaRequest request,
        CancellationToken ct)
    {
        var id = await _cinemaService.CreateAsync(request.Name, request.City, ct);
        return CreatedAtAction(nameof(GetAll), new { id }, null);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CinemaResponse>>> GetAll(CancellationToken ct)
    {
        var cinemas = await _cinemaService.GetAllAsync(ct);

        var response = cinemas
            .Select(c => new CinemaResponse(c.CinemaId, c.Name, c.City))
            .ToList();

        return Ok(response);
    }
}
