using Microsoft.EntityFrameworkCore;
using MovieBooking.Application.Exceptions;
using MovieBooking.Application.Interfaces.Persistence;
using MovieBooking.Domain.Screenings;
using MovieBooking.Infrastructure.Persistence;

namespace MovieBooking.Infrastructure.Repositories;

public sealed class ScreeningRepository : IScreeningRepository
{
    private readonly MovieBookingDbContext _db;

    public ScreeningRepository(MovieBookingDbContext db) => _db = db;

    public Task AddAsync(Screening screening, CancellationToken ct)
    {
        _db.Screenings.Add(screening);
        return Task.CompletedTask;
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

    public async Task<ScreeningSeatCapacity> GetSeatCapacityAsync(Guid screeningId, CancellationToken ct)
    {
        var capacity = await _db.Screenings
            .AsNoTracking()
            .Where(s => s.ScreeningId == screeningId)
            .Join(
                _db.Rooms,
                s => s.RoomId,
                r => r.RoomId,
                (s, r) => r.LayoutId
            )
            .Join(
                _db.Layouts,
                layoutId => layoutId,
                l => l.LayoutId,
                (layoutId, l) => new ScreeningSeatCapacity(l.RowCount, l.SeatsPerRow)
            )
            .SingleOrDefaultAsync(ct);

        if (capacity is null)
            throw new NotFoundException("Screening not found.");

        return capacity;
    }

}
