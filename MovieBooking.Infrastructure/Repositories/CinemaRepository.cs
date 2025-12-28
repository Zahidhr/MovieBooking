using Microsoft.EntityFrameworkCore;
using MovieBooking.Application.Interfaces.Persistence;
using MovieBooking.Domain.Cinemas;
using MovieBooking.Infrastructure.Persistence;

namespace MovieBooking.Infrastructure.Repositories;

public class CinemaRepository : ICinemaRepository
{
    private readonly MovieBookingDbContext _dbContext;

    public CinemaRepository(MovieBookingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Cinema cinema, CancellationToken cancellationToken)
    {
        _dbContext.Cinemas.Add(cinema);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Cinema>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Cinemas
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> ExistsAsync(Guid cinemaId, CancellationToken ct)
    => _dbContext.Cinemas.AnyAsync(c => c.CinemaId == cinemaId, ct);

}
