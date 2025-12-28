using Microsoft.EntityFrameworkCore;
using MovieBooking.Application.Interfaces.Persistence;
using MovieBooking.Domain.Layouts;
using MovieBooking.Infrastructure.Persistence;

namespace MovieBooking.Infrastructure.Repositories;

public class LayoutRepository : ILayoutRepository
{
    private readonly MovieBookingDbContext _db;

    public LayoutRepository(MovieBookingDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Layout layout, CancellationToken ct)
    {
        _db.Layouts.Add(layout);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<Layout?> GetByIdAsync(Guid layoutId, CancellationToken ct)
    {
        return await _db.Layouts
            .AsNoTracking()
            .Include(l => l.SeatTypes)
            .FirstOrDefaultAsync(l => l.LayoutId == layoutId, ct);
    }

    public Task<bool> ExistsAsync(Guid layoutId, CancellationToken ct)
    => _db.Layouts.AnyAsync(l => l.LayoutId == layoutId, ct);

}
