namespace MovieBooking.Domain.Cinemas;

public class Cinema
{
    public Guid CinemaId { get; private set; }
    public string Name { get; private set; } = null!;
    public string? City { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private Cinema() { } // EF Core

    public Cinema(string name, string? city)
    {
        CinemaId = Guid.NewGuid();
        Name = name;
        City = city;
        CreatedAt = DateTimeOffset.UtcNow;
    }
}
