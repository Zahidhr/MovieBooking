using System.Data;
using Microsoft.EntityFrameworkCore;
using MovieBooking.Application.Interfaces.Persistence;
using MovieBooking.Domain.Enums;
using MovieBooking.Domain.Screenings;
using MovieBooking.Infrastructure.Persistence;

namespace MovieBooking.Infrastructure.Repositories;

public sealed class SeatReservationRepository : ISeatReservationRepository
{
    private readonly MovieBookingDbContext _db;
    public SeatReservationRepository(MovieBookingDbContext db) => _db = db;

    public async Task<IReadOnlyList<SeatReservation>> GetByScreeningAsync(Guid screeningId, CancellationToken ct)
    {
        return await _db.SeatReservations
            .AsNoTracking()
            .Where(s => s.ScreeningId == screeningId)
            .OrderBy(s => s.RowNumber)
            .ThenBy(s => s.SeatNumber)
            .ToListAsync(ct);
    }

    public async Task<bool> TryHoldSeatsAsync(
        Guid screeningId,
        Guid holdId,
        DateTimeOffset expiresAt,
        IReadOnlyList<(int RowNumber, int SeatNumber)> seats,
        DateTimeOffset now,
        CancellationToken ct)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);

        var keys = seats.ToHashSet();

        var existing = await _db.SeatReservations
            .Where(s => s.ScreeningId == screeningId)
            .Where(s => seats.Select(x => x.RowNumber).Contains(s.RowNumber))
            .ToListAsync(ct);

        var map = existing.ToDictionary(x => (x.RowNumber, x.SeatNumber));

        // Validate availability
        foreach (var seatKey in keys)
        {
            if (!map.TryGetValue(seatKey, out var current))
                continue;

            if (current.Status == SeatReservationStatus.Booked)
                return false;

            if (current.Status == SeatReservationStatus.Held && !current.IsExpired(now))
                return false;
        }

        // Apply holds
        foreach (var seatKey in keys)
        {
            if (!map.TryGetValue(seatKey, out var current))
            {
                var reservation = new SeatReservation(screeningId, seatKey.RowNumber, seatKey.SeatNumber, now);
                reservation.Hold(holdId, expiresAt);
                _db.SeatReservations.Add(reservation);
            }
            else
            {
                if (current.Status == SeatReservationStatus.Held && current.IsExpired(now))
                    current.ReleaseToAvailable();

                current.Hold(holdId, expiresAt);
            }
        }

        return true;
    }
}
