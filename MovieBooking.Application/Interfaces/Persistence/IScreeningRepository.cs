using MovieBooking.Domain.Screenings;

namespace MovieBooking.Application.Interfaces.Persistence;

public interface IScreeningRepository
{
    Task AddAsync(Screening screening, CancellationToken ct);

    Task<bool> HasOverlapAsync(
        Guid roomId,
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        CancellationToken ct);

    Task<IReadOnlyList<Screening>> GetByRoomIdAsync(Guid roomId, CancellationToken ct);

    Task<bool> ExistsAsync(Guid screeningId, CancellationToken ct);

    Task<ScreeningSeatCapacity> GetSeatCapacityAsync(
    Guid screeningId,
    CancellationToken ct);

}
