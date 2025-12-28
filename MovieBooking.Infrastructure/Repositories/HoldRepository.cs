using MovieBooking.Application.Interfaces.Persistence;
using MovieBooking.Domain.Screenings;
using MovieBooking.Infrastructure.Persistence;

namespace MovieBooking.Infrastructure.Repositories;

public sealed class HoldRepository : IHoldRepository
{
    private readonly MovieBookingDbContext _db;
    public HoldRepository(MovieBookingDbContext db) => _db = db;

    public async Task AddAsync(Hold hold, CancellationToken ct)
    {
        _db.Holds.Add(hold);
        await _db.SaveChangesAsync(ct);
    }
}
