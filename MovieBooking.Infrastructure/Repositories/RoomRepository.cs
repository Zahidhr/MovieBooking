using Microsoft.EntityFrameworkCore;
using MovieBooking.Application.Interfaces.Persistence;
using MovieBooking.Domain.Cinemas;
using MovieBooking.Infrastructure.Persistence;

namespace MovieBooking.Infrastructure.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly MovieBookingDbContext _db;

    public RoomRepository(MovieBookingDbContext db) => _db = db;

    public async Task AddAsync(Room room, CancellationToken ct)
    {
        _db.Rooms.Add(room);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<Room>> GetByCinemaIdAsync(Guid cinemaId, CancellationToken ct)
    {
        return await _db.Rooms
            .AsNoTracking()
            .Where(r => r.CinemaId == cinemaId)
            .OrderBy(r => r.Name)
            .ToListAsync(ct);
    }

    public Task<bool> ExistsAsync(Guid roomId, CancellationToken ct)
    => _db.Rooms.AnyAsync(r => r.RoomId == roomId, ct);

}
