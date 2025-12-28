namespace MovieBooking.Api.Contracts.Responses;

public record SeatReservationResponse(
    int RowNumber,
    int SeatNumber,
    string Status,
    Guid? HoldId,
    DateTimeOffset? ExpiresAt);
