using Microsoft.AspNetCore.Mvc;
using MovieBooking.Api.Contracts.Responses;
using MovieBooking.Application.Interfaces.Persistence;
using MovieBooking.Application.Interfaces.Services;

namespace MovieBooking.Api.Controllers;

[ApiController]
[Route("api/screenings/{screeningId:guid}/seats")]
public sealed class SeatsController : ControllerBase
{
    private readonly ISeatReservationRepository _seatRepo;
    private readonly IHoldCleanupService _cleanup;

    public SeatsController(ISeatReservationRepository seatRepo, IHoldCleanupService cleanup)
    {
        _seatRepo = seatRepo;
        _cleanup = cleanup;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SeatReservationResponse>>> Get(Guid screeningId, CancellationToken ct)
    {
        // Lazy cleanup keeps DB consistent even without Hangfire
        await _cleanup.CleanupExpiredByScreeningAsync(screeningId, ct);

        var rows = await _seatRepo.GetByScreeningAsync(screeningId, ct);

        var resp = rows.Select(x =>
            new SeatReservationResponse(x.RowNumber, x.SeatNumber, x.Status.ToString(), x.HoldId, x.ExpiresAt)
        ).ToList();

        return Ok(resp);
    }
}
