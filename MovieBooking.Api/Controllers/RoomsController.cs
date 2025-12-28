using Microsoft.AspNetCore.Mvc;
using MovieBooking.Api.Contracts.Requests;
using MovieBooking.Api.Contracts.Responses;
using MovieBooking.Application.Services;

namespace MovieBooking.Api.Controllers;

[ApiController]
[Route("api/cinemas/{cinemaId:guid}/rooms")]
public class RoomsController : ControllerBase
{
    private readonly RoomService _roomService;

    public RoomsController(RoomService roomService) => _roomService = roomService;

    [HttpPost]
    public async Task<IActionResult> Create(Guid cinemaId, CreateRoomRequest request, CancellationToken ct)
    {
        var id = await _roomService.CreateAsync(cinemaId, request.LayoutId, request.Name, ct);
        return CreatedAtAction(nameof(GetAll), new { cinemaId }, null);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RoomResponse>>> GetAll(Guid cinemaId, CancellationToken ct)
    {
        var rooms = await _roomService.GetByCinemaIdAsync(cinemaId, ct);

        var response = rooms
            .Select(r => new RoomResponse(r.RoomId, r.CinemaId, r.LayoutId, r.Name))
            .ToList();

        return Ok(response);
    }
}
