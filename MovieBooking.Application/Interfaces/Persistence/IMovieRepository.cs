using MovieBooking.Domain.Movies;

namespace MovieBooking.Application.Interfaces.Persistence;

public interface IMovieRepository
{
    Task AddAsync(Movie movie, CancellationToken ct);
    Task<IReadOnlyList<Movie>> GetAllAsync(CancellationToken ct);
    Task<bool> ExistsAsync(Guid movieId, CancellationToken ct);
    Task<int?> GetDurationMinutesAsync(Guid movieId, CancellationToken ct);

}
