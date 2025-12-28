using MovieBooking.Application.Interfaces.Persistence;
using MovieBooking.Domain.Cinemas;

namespace MovieBooking.Application.Services;

public class RoomService
{
    private readonly IRoomRepository _roomRepo;
    private readonly ICinemaRepository _cinemaRepo;
    private readonly ILayoutRepository _layoutRepo;

    public RoomService(IRoomRepository roomRepo, ICinemaRepository cinemaRepo, ILayoutRepository layoutRepo)
    {
        _roomRepo = roomRepo;
        _cinemaRepo = cinemaRepo;
        _layoutRepo = layoutRepo;
    }

    public async Task<Guid> CreateAsync(Guid cinemaId, Guid layoutId, string name, CancellationToken ct)
    {
        if (!await _cinemaRepo.ExistsAsync(cinemaId, ct))
            throw new InvalidOperationException("Cinema not found.");

        if (!await _layoutRepo.ExistsAsync(layoutId, ct))
            throw new InvalidOperationException("Layout not found.");

        var room = new Room(cinemaId, layoutId, name);
        await _roomRepo.AddAsync(room, ct);

        return room.RoomId;
    }

    public Task<IReadOnlyList<Room>> GetByCinemaIdAsync(Guid cinemaId, CancellationToken ct)
        => _roomRepo.GetByCinemaIdAsync(cinemaId, ct);
}
