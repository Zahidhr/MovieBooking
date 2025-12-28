using Microsoft.EntityFrameworkCore;
using MovieBooking.Application.Interfaces.Persistence;
using MovieBooking.Domain.Movies;
using MovieBooking.Infrastructure.Persistence;

namespace MovieBooking.Infrastructure.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly MovieBookingDbContext _db;

    public MovieRepository(MovieBookingDbContext db) => _db = db;

    public async Task AddAsync(Movie movie, CancellationToken ct)
    {
        _db.Movies.Add(movie);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<Movie>> GetAllAsync(CancellationToken ct)
    {
        return await _db.Movies
            .AsNoTracking()
            .OrderBy(m => m.Title)
            .ToListAsync(ct);
    }

    public Task<bool> ExistsAsync(Guid movieId, CancellationToken ct)
        => _db.Movies.AnyAsync(m => m.MovieId == movieId, ct);

    public async Task<int?> GetDurationMinutesAsync(Guid movieId, CancellationToken ct)
    {
        return await _db.Movies
            .Where(m => m.MovieId == movieId)
            .Select(m => (int?)m.DurationMinutes)
            .FirstOrDefaultAsync(ct);
    }

}
