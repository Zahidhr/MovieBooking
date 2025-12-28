using Microsoft.Extensions.Options;
using MovieBooking.Application.Interfaces.Persistence;
using MovieBooking.Application.Options;
using MovieBooking.Domain.Screenings;

namespace MovieBooking.Application.Services;

public sealed class ScreeningService
{
    private readonly IScreeningRepository _screeningRepo;
    private readonly IRoomRepository _roomRepo;
    private readonly IMovieRepository _movieRepo;
    private readonly ScreeningOptions _options;

    public ScreeningService(
        IScreeningRepository screeningRepo,
        IRoomRepository roomRepo,
        IMovieRepository movieRepo,
        IOptions<ScreeningOptions> options)
    {
        _screeningRepo = screeningRepo;
        _roomRepo = roomRepo;
        _movieRepo = movieRepo;
        _options = options.Value;
    }

    public async Task<Guid> CreateAsync(Guid roomId, Guid movieId, DateTimeOffset startTime, CancellationToken ct)
    {
        if (!await _roomRepo.ExistsAsync(roomId, ct))
            throw new InvalidOperationException("Room not found.");

        var duration = await _movieRepo.GetDurationMinutesAsync(movieId, ct);
        if (duration is null)
            throw new InvalidOperationException("Movie not found.");

        if (_options.CleanupBufferMinutes < 0)
            throw new InvalidOperationException("CleanupBufferMinutes cannot be negative.");

        var endTime = startTime
            .AddMinutes(duration.Value + _options.CleanupBufferMinutes);

        var hasOverlap = await _screeningRepo.HasOverlapAsync(roomId, startTime, endTime, ct);
        if (hasOverlap)
            throw new InvalidOperationException("Screening overlaps with an existing screening in this room.");

        var screening = new Screening(roomId, movieId, startTime, endTime);
        await _screeningRepo.AddAsync(screening, ct);

        return screening.ScreeningId;
    }

    public Task<IReadOnlyList<Screening>> GetByRoomIdAsync(Guid roomId, CancellationToken ct)
        => _screeningRepo.GetByRoomIdAsync(roomId, ct);
}
