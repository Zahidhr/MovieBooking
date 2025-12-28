using Microsoft.AspNetCore.Mvc;
using MovieBooking.Api.Contracts.Requests;
using MovieBooking.Api.Contracts.Responses;
using MovieBooking.Application.Services;

namespace MovieBooking.Api.Controllers;

[ApiController]
[Route("api/rooms/{roomId:guid}/screenings")]
public sealed class ScreeningsController : ControllerBase
{
    private readonly ScreeningService _screeningService;

    public ScreeningsController(ScreeningService screeningService)
    {
        _screeningService = screeningService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid roomId, CreateScreeningRequest request, CancellationToken ct)
    {
        var id = await _screeningService.CreateAsync(roomId, request.MovieId, request.StartTime, ct);
        return CreatedAtAction(nameof(GetByRoom), new { roomId }, null);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ScreeningResponse>>> GetByRoom(Guid roomId, CancellationToken ct)
    {
        var screenings = await _screeningService.GetByRoomIdAsync(roomId, ct);

        var response = screenings.Select(s => new ScreeningResponse(
            s.ScreeningId,
            s.RoomId,
            s.MovieId,
            s.StartTime,
            s.EndTime)).ToList();

        return Ok(response);
    }
}
