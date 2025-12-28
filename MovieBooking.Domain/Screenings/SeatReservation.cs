using MovieBooking.Domain.Enums;

namespace MovieBooking.Domain.Screenings;

public class SeatReservation
{
    public Guid ScreeningId { get; private set; }
    public int RowNumber { get; private set; }
    public int SeatNumber { get; private set; }

    public SeatReservationStatus Status { get; private set; }
    public Guid? HoldId { get; private set; }
    public DateTimeOffset? ExpiresAt { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private SeatReservation() { } // EF

    public SeatReservation(Guid screeningId, int rowNumber, int seatNumber, DateTimeOffset createdAt)
    {
        ScreeningId = screeningId;
        RowNumber = rowNumber;
        SeatNumber = seatNumber;
        Status = SeatReservationStatus.Available;
        CreatedAt = createdAt;
    }

    public bool IsExpired(DateTimeOffset now)
        => Status == SeatReservationStatus.Held &&
           ExpiresAt.HasValue &&
           now >= ExpiresAt.Value;

    public void Hold(Guid holdId, DateTimeOffset expiresAt)
    {
        Status = SeatReservationStatus.Held;
        HoldId = holdId;
        ExpiresAt = expiresAt;
    }

    public void ReleaseToAvailable()
    {
        Status = SeatReservationStatus.Available;
        HoldId = null;
        ExpiresAt = null;
    }

    public void MarkBooked()
    {
        Status = SeatReservationStatus.Booked;
        HoldId = null;
        ExpiresAt = null;
    }
}
