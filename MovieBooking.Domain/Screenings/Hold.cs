using MovieBooking.Domain.Enums;

namespace MovieBooking.Domain.Screenings;

public class Hold
{
    public Guid HoldId { get; private set; }
    public Guid ScreeningId { get; private set; }
    public HoldStatus Status { get; private set; }
    public DateTimeOffset ExpiresAt { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private Hold() { } // EF

    public Hold(Guid screeningId, DateTimeOffset expiresAt, DateTimeOffset createdAt)
    {
        HoldId = Guid.NewGuid();
        ScreeningId = screeningId;
        Status = HoldStatus.Active;
        ExpiresAt = expiresAt;
        CreatedAt = createdAt;
    }

    public bool IsExpired(DateTimeOffset now) => now >= ExpiresAt;

    public void MarkExpired()
    {
        if (Status == HoldStatus.Active)
            Status = HoldStatus.Expired;
    }

    public void Cancel()
    {
        if (Status == HoldStatus.Active)
            Status = HoldStatus.Cancelled;
    }
}
