using MovieBooking.Application.Interfaces;

namespace MovieBooking.Infrastructure.Services;

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
