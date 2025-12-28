using MovieBooking.Domain.Cinemas;

namespace MovieBooking.Application.Interfaces.Persistence;

public interface IRoomRepository
{
    Task AddAsync(Room room, CancellationToken ct);
    Task<IReadOnlyList<Room>> GetByCinemaIdAsync(Guid cinemaId, CancellationToken ct);
    Task<bool> ExistsAsync(Guid roomId, CancellationToken ct);

}
