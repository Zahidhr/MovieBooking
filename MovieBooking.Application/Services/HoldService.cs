using Microsoft.Extensions.Options;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Interfaces;
using MovieBooking.Application.Interfaces.Persistence;
using MovieBooking.Application.Interfaces.Services;
using MovieBooking.Application.Options;
using MovieBooking.Domain.Screenings;

namespace MovieBooking.Application.Services;

public sealed class HoldService
{
    private readonly IClock _clock;
    private readonly IHoldRepository _holdRepo;
    private readonly ISeatReservationRepository _seatRepo;
    private readonly IScreeningRepository _screeningRepo;
    private readonly IHoldCleanupService _cleanup;
    private readonly HoldOptions _options;

    public HoldService(
        IClock clock,
        IHoldRepository holdRepo,
        ISeatReservationRepository seatRepo,
        IScreeningRepository screeningRepo,
        IHoldCleanupService cleanup,
        IOptions<HoldOptions> options)
    {
        _clock = clock;
        _holdRepo = holdRepo;
        _seatRepo = seatRepo;
        _screeningRepo = screeningRepo;
        _cleanup = cleanup;
        _options = options.Value;
    }

    public async Task<Guid> CreateHoldAsync(
        Guid screeningId,
        IReadOnlyList<(int RowNumber, int SeatNumber)> seats,
        CancellationToken ct)
    {
        if (seats.Count == 0)
            throw new ValidationException(
                "At least one seat is required.",
                "EMPTY_SEAT_SELECTION");


        if (!await _screeningRepo.ExistsAsync(screeningId, ct))
            throw new NotFoundException("Screening not found.");

        // Lazy cleanup now (Hangfire later will call same cleanup)
        await _cleanup.CleanupExpiredByScreeningAsync(screeningId, ct);

        var now = _clock.UtcNow;
        var expiresAt = now.AddMinutes(_options.HoldDurationMinutes);

        var hold = new Hold(screeningId, expiresAt, now);

        // Save hold first so we have HoldId
        await _holdRepo.AddAsync(hold, ct);

        var success = await _seatRepo.TryHoldSeatsAsync(
            screeningId,
            hold.HoldId,
            expiresAt,
            seats,
            now,
            ct);

        if (!success)
               throw new ConflictException(
                "One or more seats are not available.",
                "SEAT_NOT_AVAILABLE");


        return hold.HoldId;
    }
}
