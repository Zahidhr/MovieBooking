using MovieBooking.Domain.Screenings;

namespace MovieBooking.Application.Interfaces.Persistence;

public interface ISeatReservationRepository
{
    Task<IReadOnlyList<SeatReservation>> GetByScreeningAsync(Guid screeningId, CancellationToken ct);

    /// <summary>
    /// Attempts to hold the specified seats for a screening.
    /// Stages changes but does NOT call SaveChanges or manage transactions.
    /// Returns false if any seat is unavailable; true if all seats are staged for hold.
    /// </summary>
    Task<bool> TryHoldSeatsAsync(
        Guid screeningId,
        Guid holdId,
        DateTimeOffset expiresAt,
        IReadOnlyList<(int RowNumber, int SeatNumber)> seats,
        DateTimeOffset now,
        CancellationToken ct);
}
