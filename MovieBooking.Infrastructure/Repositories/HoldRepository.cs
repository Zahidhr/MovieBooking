using MovieBooking.Application.Interfaces.Persistence;
using MovieBooking.Domain.Screenings;
using MovieBooking.Infrastructure.Persistence;

namespace MovieBooking.Infrastructure.Repositories;

public sealed class HoldRepository : IHoldRepository
{
    private readonly MovieBookingDbContext _db;
    public HoldRepository(MovieBookingDbContext db) => _db = db;

    public void Add(Hold hold)
    {
        _db.Holds.Add(hold);
    }
}
