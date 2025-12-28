using MovieBooking.Domain.Screenings;

namespace MovieBooking.Application.Interfaces.Persistence;

public interface ISeatReservationRepository
{
    Task<IReadOnlyList<SeatReservation>> GetByScreeningAsync(Guid screeningId, CancellationToken ct);

    Task<bool> TryHoldSeatsAsync(
        Guid screeningId,
        Guid holdId,
        DateTimeOffset expiresAt,
        IReadOnlyList<(int RowNumber, int SeatNumber)> seats,
        DateTimeOffset now,
        CancellationToken ct);
}
