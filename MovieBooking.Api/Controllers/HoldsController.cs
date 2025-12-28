using Microsoft.AspNetCore.Mvc;
using MovieBooking.Api.Contracts.Requests;
using MovieBooking.Api.Contracts.Responses;
using MovieBooking.Application.Services;

namespace MovieBooking.Api.Controllers;

[ApiController]
[Route("api/screenings/{screeningId:guid}/holds")]
public sealed class HoldsController : ControllerBase
{
    private readonly HoldService _holdService;

    public HoldsController(HoldService holdService)
    {
        _holdService = holdService;
    }

    [HttpPost]
    public async Task<ActionResult<CreateHoldResponse>> Create(Guid screeningId, CreateHoldRequest request, CancellationToken ct)
    {
        try
        {
            var seats = request.Seats.Select(s => (s.RowNumber, s.SeatNumber)).ToList();
            var holdId = await _holdService.CreateHoldAsync(screeningId, seats, ct);
            return Ok(new CreateHoldResponse(holdId));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

}
