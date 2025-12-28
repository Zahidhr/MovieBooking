namespace MovieBooking.Application.Options;

public sealed class ScreeningOptions
{
    // cleanup buffer between screenings (minutes)
    public int CleanupBufferMinutes { get; init; } = 10;
}
