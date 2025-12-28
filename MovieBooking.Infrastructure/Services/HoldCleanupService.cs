using Microsoft.EntityFrameworkCore;
using MovieBooking.Application.Interfaces;
using MovieBooking.Application.Interfaces.Services;
using MovieBooking.Domain.Enums;
using MovieBooking.Infrastructure.Persistence;

namespace MovieBooking.Infrastructure.Services;

public sealed class HoldCleanupService : IHoldCleanupService
{
    private readonly MovieBookingDbContext _db;
    private readonly IClock _clock;

    public HoldCleanupService(MovieBookingDbContext db, IClock clock)
    {
        _db = db;
        _clock = clock;
    }

    public Task CleanupExpiredAsync(CancellationToken ct)
        => CleanupInternalAsync(null, ct);

    public Task CleanupExpiredByScreeningAsync(Guid screeningId, CancellationToken ct)
        => CleanupInternalAsync(screeningId, ct);

    private async Task CleanupInternalAsync(Guid? screeningId, CancellationToken ct)
    {
        var now = _clock.UtcNow;

        var expiredHoldsQuery = _db.Holds
            .Where(h => h.Status == HoldStatus.Active && h.ExpiresAt <= now);

        if (screeningId.HasValue)
            expiredHoldsQuery = expiredHoldsQuery.Where(h => h.ScreeningId == screeningId.Value);

        var expiredHolds = await expiredHoldsQuery.ToListAsync(ct);

        if (expiredHolds.Count == 0)
            return;

        foreach (var hold in expiredHolds)
            hold.MarkExpired();

        // Release expired seat holds
        var seatsQuery = _db.SeatReservations
            .Where(s => s.Status == SeatReservationStatus.Held)
            .Where(s => s.ExpiresAt.HasValue && s.ExpiresAt <= now);

        if (screeningId.HasValue)
            seatsQuery = seatsQuery.Where(s => s.ScreeningId == screeningId.Value);

        var seats = await seatsQuery.ToListAsync(ct);

        foreach (var seat in seats)
            seat.ReleaseToAvailable();

        await _db.SaveChangesAsync(ct);
    }
}
