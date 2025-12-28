namespace MovieBooking.Application.Interfaces;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
