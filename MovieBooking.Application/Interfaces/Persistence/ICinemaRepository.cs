using MovieBooking.Domain.Cinemas;

namespace MovieBooking.Application.Interfaces.Persistence;

public interface ICinemaRepository
{
    Task AddAsync(Cinema cinema, CancellationToken cancellationToken);
    Task<IReadOnlyList<Cinema>> GetAllAsync(CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid cinemaId, CancellationToken ct);

}
