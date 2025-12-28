using Microsoft.EntityFrameworkCore;
using MovieBooking.Application.Interfaces.Persistence;
using MovieBooking.Domain.Screenings;
using MovieBooking.Infrastructure.Persistence;

namespace MovieBooking.Infrastructure.Repositories;

public sealed class ScreeningRepository : IScreeningRepository
{
    private readonly MovieBookingDbContext _db;

    public ScreeningRepository(MovieBookingDbContext db) => _db = db;

    public async Task AddAsync(Screening screening, CancellationToken ct)
    {
        _db.Screenings.Add(screening);
        await _db.SaveChangesAsync(ct);
    }

    public Task<bool> HasOverlapAsync(
        Guid roomId,
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        CancellationToken ct)
    {
        // overlap rule: existing.Start < newEnd AND existing.End > newStart
        return _db.Screenings
            .AsNoTracking()
            .AnyAsync(s =>
                s.RoomId == roomId &&
                s.StartTime < endTime &&
                s.EndTime > startTime,
                ct);
    }

    public async Task<IReadOnlyList<Screening>> GetByRoomIdAsync(Guid roomId, CancellationToken ct)
    {
        return await _db.Screenings
            .AsNoTracking()
            .Where(s => s.RoomId == roomId)
            .OrderBy(s => s.StartTime)
            .ToListAsync(ct);
    }

    public Task<bool> ExistsAsync(Guid screeningId, CancellationToken ct)
    {
        return _db.Screenings
            .AsNoTracking()
            .AnyAsync(s => s.ScreeningId == screeningId, ct);
    }

}
