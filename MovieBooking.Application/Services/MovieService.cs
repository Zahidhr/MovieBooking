using MovieBooking.Application.Interfaces.Persistence;
using MovieBooking.Domain.Movies;

namespace MovieBooking.Application.Services;

public class MovieService
{
    private readonly IMovieRepository _movieRepo;

    public MovieService(IMovieRepository movieRepo)
    {
        _movieRepo = movieRepo;
    }

    public async Task<Guid> CreateAsync(string title, int durationMinutes, CancellationToken ct)
    {
        if (durationMinutes <= 0)
            throw new ArgumentException("DurationMinutes must be > 0");

        var movie = new Movie(title, durationMinutes);
        await _movieRepo.AddAsync(movie, ct);
        return movie.MovieId;
    }

    public Task<IReadOnlyList<Movie>> GetAllAsync(CancellationToken ct)
        => _movieRepo.GetAllAsync(ct);
}
