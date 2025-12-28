namespace MovieBooking.Api.Contracts.Responses;

public record ScreeningResponse(
    Guid ScreeningId,
    Guid RoomId,
    Guid MovieId,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime);
