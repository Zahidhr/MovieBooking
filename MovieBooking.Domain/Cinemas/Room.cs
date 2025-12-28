namespace MovieBooking.Domain.Cinemas;

public class Room
{
    public Guid RoomId { get; private set; }
    public Guid CinemaId { get; private set; }
    public Guid LayoutId { get; private set; }

    public string Name { get; private set; } = null!;
    public DateTimeOffset CreatedAt { get; private set; }

    public Cinema? Cinema { get; private set; }

    private Room() { } // EF

    public Room(Guid cinemaId, Guid layoutId, string name)
    {
        RoomId = Guid.NewGuid();
        CinemaId = cinemaId;
        LayoutId = layoutId;
        Name = name;
        CreatedAt = DateTimeOffset.UtcNow;
    }
}
