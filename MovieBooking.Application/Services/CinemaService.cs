using MovieBooking.Application.Interfaces.Persistence;
using MovieBooking.Domain.Cinemas;

namespace MovieBooking.Application.Services;

public class CinemaService
{
    private readonly ICinemaRepository _cinemaRepository;

    public CinemaService(ICinemaRepository cinemaRepository)
    {
        _cinemaRepository = cinemaRepository;
    }

    public async Task<Guid> CreateAsync(string name, string? city, CancellationToken ct)
    {
        var cinema = new Cinema(name, city);
        await _cinemaRepository.AddAsync(cinema, ct);
        return cinema.CinemaId;
    }

    public Task<IReadOnlyList<Cinema>> GetAllAsync(CancellationToken ct)
        => _cinemaRepository.GetAllAsync(ct);
}
