namespace MovieBooking.Domain.Screenings;

public class Screening
{
    public Guid ScreeningId { get; private set; }
    public Guid RoomId { get; private set; }
    public Guid MovieId { get; private set; }

    public DateTimeOffset StartTime { get; private set; }
    public DateTimeOffset EndTime { get; private set; } // includes cleanup buffer
    public DateTimeOffset CreatedAt { get; private set; }

    private Screening() { } // EF

    public Screening(Guid roomId, Guid movieId, DateTimeOffset startTime, DateTimeOffset endTime)
    {
        ScreeningId = Guid.NewGuid();
        RoomId = roomId;
        MovieId = movieId;
        StartTime = startTime;
        EndTime = endTime;
        CreatedAt = DateTimeOffset.UtcNow;
    }
}
