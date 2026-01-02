using MovieBooking.Application.Interfaces;

namespace MovieBooking.UnitTests.Helpers;

public sealed class FakeClock : IClock
{
    public DateTimeOffset UtcNow { get; private set; }

    public FakeClock(DateTimeOffset? initial = null)
    {
        UtcNow = initial ?? new DateTimeOffset(2025, 1, 1, 10, 0, 0, TimeSpan.Zero);
    }

    public void Set(DateTimeOffset value) => UtcNow = value;
    public void Advance(TimeSpan delta) => UtcNow = UtcNow.Add(delta);
}
