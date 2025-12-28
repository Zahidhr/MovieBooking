namespace MovieBooking.Domain.Movies;

public class Movie
{
    public Guid MovieId { get; private set; }
    public string Title { get; private set; } = null!;
    public int DurationMinutes { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private Movie() { } // EF

    public Movie(string title, int durationMinutes)
    {
        MovieId = Guid.NewGuid();
        Title = title;
        DurationMinutes = durationMinutes;
        CreatedAt = DateTimeOffset.UtcNow;
    }
}
